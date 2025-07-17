using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetMvc.Models
{
    public class ComicBook
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Author { get; set; }
        
        public string Artist { get; set; }
        
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
