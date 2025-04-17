using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Artist : ISeed<Artist>
	{
        [Key]
        public Guid ArtistId {get; set;}

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        
        //Navigation property many-to-many
		public List<MusicGroup>  MusicGroups { get; set; }

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public Artist Seed(SeedGenerator _seeder)
        {
            return new Artist
            {
                ArtistId = Guid.NewGuid(),

                FirstName = _seeder.FirstName,
                LastName = _seeder.LastName,
                BirthDay = _seeder.DateAndTime(1940, 2020),
    
                Seeded = true
            };
        }
        #endregion
    }
}

