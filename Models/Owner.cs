using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Owner  : ISeed<Owner>
	{   
        [Key]
        public Guid OwnerId {get; set;}

        public string Name {get; set;}

        //Nav props
        public List<Car> Cars {get; set;} = null;


        public bool Seeded { get; set; } = false;

        public Owner Seed(SeedGenerator seeder)
        {
            return new Owner{
                OwnerId = Guid.NewGuid(),
                Name = seeder.FullName,
                Seeded = true
            };
        }
    }
}