using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class MusicGroup : ISeed<MusicGroup>
	{
        [Key]
        public Guid MusicGroupId {get; set;}

		public string Name { get; set; }
		public int EstablishedYear { get; set; }

        //Navigation property one-to-many
		public List<Artist> Members { get; set; }
		public List<Album>  Albums { get; set; }

        public override string ToString() =>
            $"{Name} with {Members.Count} members was esblished {EstablishedYear} and made {Albums.Count} great albums. ";

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public MusicGroup Seed(SeedGenerator _seeder)
        {

            return new MusicGroup
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

