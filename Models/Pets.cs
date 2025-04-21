using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public enum AnimalKind { Dog, Cat, Rabbit, Fish, Bird };
    public enum AnimalMood { Happy, Hungry, Lazy, Sulky, Buzy, Sleepy };
    public class Pet : ISeed<Pet>
	{
        [Key]
        public Guid PetId {get; set;}

        public AnimalKind AnimalKind { get; set; }
        public string AnimalKindString { get => AnimalKind.ToString(); set { }}  //set is needed by EFC. do nothing

        public AnimalMood AnimalMood { get; set; }
        public virtual string AnimalMoodString { get => AnimalMood.ToString(); set { } }

		public string Name { get; set; }
        public int Age { get; set; }
        
        //Navigation property one-to-one
        public Friend Owner{ get; set; } = null;

        public override string ToString() => $"{Name}, the {AnimalMood} {AnimalKind}, is {Age} years old";

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public Pet Seed(SeedGenerator seeder)
        {
            var country = seeder.Country;
            return new Pet
            {
                PetId = Guid.NewGuid(),
                
                AnimalKind = seeder.FromEnum<AnimalKind>(),
                AnimalMood = seeder.FromEnum<AnimalMood>(),
                Age = seeder.Next(0, 11),

                Name = seeder.PetName,
                Seeded = true
            };
        }
        #endregion
    }
}

