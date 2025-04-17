using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class csArtist : ISeed<csArtist>
	{
        [Key]
        public Guid ArtistId {get; set;}

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        
        //Navigation property many-to-many
		public List<csMusicGroup>  MusicGroups { get; set; }

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public csArtist Seed(csSeedGenerator _seeder)
        {
            return new csArtist
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

