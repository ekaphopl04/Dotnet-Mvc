using Microsoft.EntityFrameworkCore;
using DotnetMvc.Models;
using System;

namespace DotnetMvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships properly
            modelBuilder.Entity<ComicBook>()
                .HasOne(c => c.Category)
                .WithMany(c => c.ComicBooks)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed some initial data for comic book categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Superhero", Description = "Comic books featuring superheroes and their adventures" },
                new Category { Id = 2, Name = "Manga", Description = "Japanese comic books and graphic novels" },
                new Category { Id = 3, Name = "Science Fiction", Description = "Comic books with science fiction themes" },
                new Category { Id = 4, Name = "Fantasy", Description = "Comic books with fantasy elements and worlds" },
                new Category { Id = 5, Name = "Horror", Description = "Comic books with horror and supernatural themes" }
            );

            // Seed comic book data with static dates to avoid non-deterministic model issues
            modelBuilder.Entity<ComicBook>().HasData(
                new ComicBook 
                { 
                    Id = 1, 
                    Title = "Batman: Year One", 
                    Author = "Frank Miller", 
                    Artist = "David Mazzucchelli",
                    Description = "Batman's early days fighting crime in Gotham City", 
                    Price = 19.99M, 
                    IsAvailable = true,
                    CategoryId = 1,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new ComicBook 
                { 
                    Id = 2, 
                    Title = "One Piece", 
                    Author = "Eiichiro Oda", 
                    Artist = "Eiichiro Oda",
                    Description = "Monkey D. Luffy and his pirate crew search for the greatest treasure, the One Piece", 
                    Price = 9.99M, 
                    IsAvailable = true,
                    CategoryId = 2,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new ComicBook 
                { 
                    Id = 3, 
                    Title = "Saga", 
                    Author = "Brian K. Vaughan", 
                    Artist = "Fiona Staples",
                    Description = "Epic space opera/fantasy comic book series", 
                    Price = 14.99M, 
                    IsAvailable = true,
                    CategoryId = 3,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new ComicBook 
                { 
                    Id = 4, 
                    Title = "The Sandman", 
                    Author = "Neil Gaiman", 
                    Artist = "Sam Kieth",
                    Description = "The Lord of Dreams has been imprisoned for decades and must reclaim his power", 
                    Price = 24.99M, 
                    IsAvailable = true,
                    CategoryId = 4,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new ComicBook 
                { 
                    Id = 5, 
                    Title = "Uzumaki", 
                    Author = "Junji Ito", 
                    Artist = "Junji Ito",
                    Description = "A town is haunted by a supernatural spiral pattern", 
                    Price = 22.99M, 
                    IsAvailable = true,
                    CategoryId = 5,
                    CreatedDate = new DateTime(2025, 1, 1)
                }
            );
        }
    }
}
