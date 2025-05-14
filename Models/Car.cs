using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Car  : ISeed<Car>
	{   
        [Key]
        public Guid CarId {get; set;}

        public string RegNumber {get; set;}

        //Nav props
        public Owner Owner {get; set;} = null;
        

        public bool Seeded { get; set; } = false;

        public Car Seed(SeedGenerator seeder)
        {
            string regchar = seeder.FromString("ABC, EFT, HJY, HGT, GTR");
            int regnr = seeder.Next(111,999);

            return new Car {
                CarId = Guid.NewGuid(),
                RegNumber = $"{regchar} {regnr}",
                Seeded = true
            };
        }
    }
}