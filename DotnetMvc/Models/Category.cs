using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotnetMvc.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        // Navigation property for related comic books
        public virtual ICollection<ComicBook> ComicBooks { get; set; }
    }
}
