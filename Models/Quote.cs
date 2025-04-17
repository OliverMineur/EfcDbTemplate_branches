using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Quote
    {
        [Key]
        public Guid QuoteId { get; set; }

        public string QuoteText { get; set; }
        public string Author { get; set; }

        //Navigation property many-to-many
        public List<Friend> Friends { get; set; }

        //constructor
        public Quote()
        {
            QuoteId = Guid.NewGuid();
        }
    }
}
