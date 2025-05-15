using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class Garage : ISeed<Garage>
    {
        [Key]
        public Guid GarageId { get; set; }

        public string Name { get; set; }

        public List<Car> Cars { get; set; } = null;

        public bool Seeded { get; set; } = false;

        public Garage Seed(SeedGenerator seed)
        {
            Guid garageId = Guid.NewGuid();
            string name = seed.FromString("Joe, Frank, Bob");
            return new Garage
            {
                GarageId = garageId,
                Name = name,
                Seeded = true
            };
        }
    }
}