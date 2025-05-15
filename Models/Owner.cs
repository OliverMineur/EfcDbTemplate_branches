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

        public string FirstName {get; set;}
        
        public string LastName {get; set;} 

        public int Age {get; set;}

        //Nav props
        public List<Car> Cars { get; set; } = null;


        public bool Seeded { get; set; } = false;

        public Owner Seed(SeedGenerator seeder)
        {
            return new Owner{
                OwnerId = Guid.NewGuid(),
                FirstName = seeder.FirstName,
                LastName = seeder.LastName,
                Age = seeder.Next(18, 60),
                Seeded = true
            };
        }
    }
}