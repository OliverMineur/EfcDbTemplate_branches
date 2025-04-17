using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class csAlbum : ISeed<csAlbum>
	{
        [Key]
        public Guid AlbumId {get; set;}

        public string Name { get; set; }
		public int ReleaseYear { get; set; }
        public int CopiesSold { get; set;}

        //Navigation property one-to-one
		public csMusicGroup  MusicGroups { get; set; }
    
        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public csAlbum Seed(csSeedGenerator _seeder)
        {
            return new csAlbum
            {
                AlbumId = Guid.NewGuid(),

                Name = _seeder.MusicAlbumName,
                ReleaseYear = _seeder.Next(1970, 1990),
                CopiesSold = _seeder.Next(10, 10_000_000),
    
                Seeded = true
            };
        }
        #endregion
    }
}

