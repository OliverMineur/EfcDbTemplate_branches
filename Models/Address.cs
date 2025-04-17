using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Address : ISeed<Address>
	{   
        [Key]
        public Guid AddressId {get; set;}

        public string Street { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString() => $"{Street}, {ZipCode} {City}, {Country}";

        //Navigation property one-to-many
        public List<Friend> Residents{ get; set; } = null;

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public Address Seed(SeedGenerator seeder)
        {
            var country = seeder.Country;
            return new Address
            {
                AddressId = Guid.NewGuid(),
                
                Street = seeder.StreetAddress(country),
                ZipCode = seeder.ZipCode,
                City = seeder.City(country),
                Country = country,
                Seeded = true
            };
        }
        #endregion
    }
}

