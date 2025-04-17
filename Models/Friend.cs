using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class Friend : ISeed<Friend>
	{
        [Key]
        public Guid FriendId {get; set;}

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }

        //Navigation property one-to-many
        public Address Address { get; set; } = null;    //null = no address        

        //Navigation property many-to-one
        public List<Pet> Pets { get; set; } = null;      //null = no pets 

        //Navigation property many-to-many
        public List<Quote> Quotes { get; set; } = null;  //null = no quote


        public string FullName => $"{FirstName} {LastName}";
        public override string ToString()
        {
            var sRet = FullName;

            if (Address != null)
            {
                sRet += $". lives at {Address}";
            }

            if (Pets != null)
            {
                sRet += $". Has pets ";
                foreach (var pet in Pets)
                {
                    sRet += $"{pet}, ";
                }
            }
            return sRet;
        }

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public Friend Seed(SeedGenerator seeder)
        {
            var fn = seeder.FirstName;
            var ln = seeder.LastName;
            var country = seeder.Country;

            return new Friend
            {
                FriendId = Guid.NewGuid(),

                FirstName = fn,
                LastName = ln,
                Email = seeder.Email(fn, ln),
                Birthday = seeder.Bool ?seeder.DateAndTime(1950, 2020) : null,

                Seeded = true
            };
        }
        #endregion
    }
}

