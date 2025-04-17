using System;
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
            var modelList = SeedModel(nrItemsSeed);

            Console.WriteLine($"\nTesting Model...");
            WriteModel(modelList);
            #endregion


            #region  run below only when Database i created
            Console.WriteLine($"\nConnecting to database...");
            Console.WriteLine($"Database type: {AppConfig.DbSetActive.DbServer}");
            Console.WriteLine($"Connection used: {AppConfig.DbSetActive.DbConnection}");
  
            Console.WriteLine($"\nSeeding database...");
            try
            {
                SeedDataBase(modelList).Wait();
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
        private static void WriteModel(List<Friend> modelList)
        {
            Console.WriteLine($"NrOfFriends: {modelList.Count()}");
            Console.WriteLine($"NrOfFriends without any pets: {modelList.Count(
                f => f.Pets == null || f.Pets?.Count == 0)}");
            Console.WriteLine($"NrOfFriends without an adress: {modelList.Count(
                f => f.Address == null)}");
               
            Console.WriteLine($"First Friend: {modelList.First()}");
            Console.WriteLine($"Last Friend: {modelList.Last()}");
        }

        private static List<Friend> SeedModel(int nrItems)
        {
            var seeder = new SeedGenerator();
            
            //Create a list of friends, adresses and pets
            var goodfriends = seeder.ItemsToList<Friend>(nrItems);
            var addresses = seeder.ItemsToList<Address>(nrItems);

            var _quotes = seeder.AllQuotes.Select (q => new Quote() { QuoteText = q.Quote, Author = q.Author}).ToList(); 

            //Assign adress and pet to friends
            for (int i = 0; i < nrItems; i++)
            {
                //assign an address randomly
                goodfriends[i].Address = (seeder.Bool) ? seeder.FromList(addresses) :null;

                //Create between 0 and 3 pets
                var _pets = new List<Pet>();
                for (int c = 0; c < seeder.Next(0,4); c++)
                {
                    _pets.Add(new Pet().Seed(seeder)); 
                }
                goodfriends[i].Pets = (_pets.Count > 0) ? _pets : null;

                //Quotes
                goodfriends[i].Quotes = new List<Quote>();
                for (int c = 0; c < seeder.Next(0,6); c++)
                {
                    var q = seeder.FromList(_quotes); 
                    goodfriends[i].Quotes.Add(q);
                }
            }
            return goodfriends;
        }
        private static async Task SeedDataBase(List<Friend> _modelList)
        {
            using (var db = MainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var item in _modelList)
                {
                    db.Friends.Add(item);
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
                var _modelList = await db.Friends
                    .Include(item => item.Address)
                    .Include(item => item.Pets)
                    .Include(item => item.Quotes)
                    .ToListAsync();                
                #endregion

                WriteModel(_modelList);
            }
        }
        #endregion
    }
}
