using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class csMusicGroup : ISeed<csMusicGroup>
	{
        [Key]
        public Guid MusicGroupId {get; set;}

		public string Name { get; set; }
		public int EstablishedYear { get; set; }

		public List<csArtist> Members { get; set; }
		public List<csAlbum>  Albums { get; set; }

        public override string ToString() =>
            $"{Name} with {Members.Count} members was esblished {EstablishedYear} and made {Albums.Count} great albums. ";

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public csMusicGroup Seed(csSeedGenerator _seeder)
        {

            return new csMusicGroup
            {
                MusicGroupId = Guid.NewGuid(),
                
                Name = _seeder.MusicGroupName,
                EstablishedYear = 0,

                Seeded = true
            };
        }
        #endregion
    }
}

