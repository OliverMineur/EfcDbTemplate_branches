using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class Album : ISeed<Album>
	{
        [Key]
        public Guid AlbumId {get; set;}

        public string Name { get; set; }
		public int ReleaseYear { get; set; }
        public int CopiesSold { get; set;}

        //Navigation property one-to-one
		public MusicGroup  MusicGroups { get; set; }
    
        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public Album Seed(SeedGenerator _seeder)
        {
            return new Album
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

