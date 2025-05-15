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
        
        public string Make {get; set;}

        public string Model {get; set;}

        //Nav props
        public Owner Owner { get; set; } = null;
        
        public Garage Garage { get; set; }

        public bool Seeded { get; set; } = false;

        public Car Seed(SeedGenerator seeder)
        {
            string regchar = seeder.FromString("ABC, EFT, HJY, HGT, GTR");
            int regnr = seeder.Next(111,999);
            string make = seeder.FromString("BMW, Fiat, Volvo, VW");
            string model = seeder.FromString("S-500, Clio, V70, Polo");

            return new Car
            {
                CarId = Guid.NewGuid(),
                RegNumber = $"{regchar} {regnr}",
                Make = make,
                Model = model,
                Seeded = true
            };
        }
    }
}