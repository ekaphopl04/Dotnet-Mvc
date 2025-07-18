using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetMvc.Models
{
    public class ComicBook
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Artist { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
        
        [Range(0, 10000)]
        public int PageCount { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        public bool IsAvailable { get; set; }
        
        public int CategoryId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
