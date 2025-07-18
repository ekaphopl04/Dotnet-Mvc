using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetMvc.Models
{
    public class ComicBook
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Artist { get; set; } = string.Empty;
        
        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; }
        
        [Range(0, 10000)]
        public int? PageCount { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        [StringLength(500)]
        public string? CoverImageUrl { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        // Navigation property
        public virtual Category? Category { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
