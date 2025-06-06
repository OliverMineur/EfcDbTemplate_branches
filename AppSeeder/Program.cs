﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data.Common;

using Seido.Utilities.SeedGenerator;

using Configuration;
using Models;
using DbContext;

namespace AppConsole
{
    static class MyLinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> collection)
        {
            collection.ToList().ForEach(item => Console.WriteLine(item));
        }
    }


    class Program
    {
        const int nrItemsSeed = 1000;
        static void Main(string[] args)
        {
            #region run below to test the model only

            Console.WriteLine($"\nSeeding the Model...");
            var _modelList = SeedModel(nrItemsSeed);

            Console.WriteLine($"\nTesting Model...");
            WriteModel(_modelList);
            #endregion


            #region  run below only when Database i created
            Console.WriteLine($"\nConnecting to database...");
            Console.WriteLine($"Database type: {AppConfig.DbSetActive.DbServer}");
            Console.WriteLine($"Connection used: {AppConfig.DbSetActive.DbConnection}");
  
            Console.WriteLine($"\nSeeding database...");
            try
            {
                SeedDataBase(_modelList).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: Database could not be seeded. Ensure the database is correctly created");
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"\nError: {ex.InnerException.Message}");
                return;
            }

            Console.WriteLine("\nQuery database...");
            QueryDatabaseAsync().Wait();
            #endregion

        }


        #region Update to reflect you new Model
        private static void WriteModel(List<MusicGroup> _modelList)
        {
            Console.WriteLine($"Nr of great music bands: {_modelList.Count()}");
            Console.WriteLine($"Total nr of albums produced: {_modelList.Sum(b => b.Albums.Count)}");
            Console.WriteLine($"Total nr of music band members: {_modelList.Sum(b => b.Members.Count)}");

            Console.WriteLine($"First Music group: {_modelList.First()}");
            _modelList.First().Albums.ForEach(album => Console.WriteLine($"  - {album.Name}"));

            Console.WriteLine($"Last Music group: {_modelList.Last()}");
            _modelList.Last().Albums.ForEach(album => Console.WriteLine($"  - {album.Name}"));

        }

        private static List<MusicGroup> SeedModel(int nrItems)
        {
            var _seeder = new SeedGenerator();

            //Create a list of 20 great bands
            var _musicgroups = _seeder.ItemsToList<MusicGroup>(nrItems);
            var _artists = _seeder.ItemsToList<Artist>(nrItems*8);

            _musicgroups.ForEach(m => {

                //pick 4 to 8 members from the list of _artists
                m.Members = _seeder.UniqueIndexPickedFromList(_seeder.Next(4, 9), _artists);

                //Create between 5 and 16 Albums
                m.Albums = new List<Album>();
                for (int i = 5; i < _seeder.Next(6, 17); i++)
                {
                    m.Albums.Add(new Album().Seed(_seeder));
                }

                m.EstablishedYear = m.Albums.Min(a => a.ReleaseYear);
            });

            return _musicgroups;
        }

        private static async Task SeedDataBase(List<MusicGroup> _modelList)
        {
            using (var db = MainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var _friend in _modelList)
                {
                    db.MusicGroups.Add(_friend);
                }
                #endregion

                await db.SaveChangesAsync();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = MainDbContext.DbContext())
            {
                #region Reading the database using EFC
                var _modelList = await db.MusicGroups
                    .Include(mg => mg.Albums)
                    .Include(mg => mg.Members)
                    .ToListAsync();                
                #endregion

                WriteModel(_modelList);
            }
        }
        #endregion
    }
}
