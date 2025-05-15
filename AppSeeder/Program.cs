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
        const int nrOfGarage = 10;
        static void Main(string[] args)
        {
            #region run below to test the model only

            Console.WriteLine($"\nSeeding the Model...");
            var modelList = SeedModel(nrItemsSeed);
            //var garageList = Makegarage(nrOfGarage, modelList);

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
                //SeedDataBaseGarage(garageList).Wait();
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
        private static void WriteModel(List<Car> modelList)
        {
            Console.WriteLine($"NrOfCars: {modelList.Count()}");
               
            Console.WriteLine($"First Car: {modelList.First().RegNumber} owned by {modelList.First().Owner.FirstName}");
            Console.WriteLine($"Last Car: {modelList.Last().RegNumber} owned by {modelList.Last().Owner.FirstName}");
        }

        // private static List<Garage> Makegarage(int nrItems, List<Car> cars) {

        //     var seeder = new SeedGenerator();

        //     var garages = seeder.ItemsToList<Garage>(nrItems);
        //     foreach (var item in garages)
        //     {
        //         item.Cars = new List<Car>();
        //         int tempNum = seeder.Next(1, 100);
        //         if (tempNum > 75)
        //         {
                    
        //         }
        //     }
        //     return garages;
        // }

        private static List<Car> SeedModel(int nrItems)
        {
            var seeder = new SeedGenerator();
            var garages = seeder.ItemsToList<Garage>(10);
            //Seed Cars
            var cars = seeder.ItemsToList<Car>(nrItems);
            foreach (var item in cars)
            {
                item.Owner = new Owner().Seed(seeder);
            }
            foreach (var item in garages)
            {
                int tempNum = seeder.Next(1, 4);
                    for (int i = 0; tempNum > i; i++)
                    {
                        var car = cars[seeder.Next(0, cars.Count)];
                        car.Garage = item;
                        
                    }
            }
            return cars;
        }
        private static async Task SeedDataBase(List<Car> _modelList)
        {
            using (var db = MainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var item in _modelList)
                {
                    db.Cars.Add(item);
                }
                #endregion

                await db.SaveChangesAsync();
            }
        }
        private static async Task SeedDataBaseGarage(List<Garage> _modelList)
        {
            using (var db = MainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var item in _modelList)
                {
                    db.Garages.Add(item);
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
                var _modelList = await db.Cars
                    .Include(x => x.Owner)
                    .ToListAsync();
                #endregion

                WriteModel(_modelList);
            }
        }
        #endregion
    }
}
