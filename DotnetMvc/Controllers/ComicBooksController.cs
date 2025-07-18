using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetMvc.Data;
using DotnetMvc.Models;

namespace DotnetMvc.Controllers
{
    public class ComicBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComicBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComicBooks - Enhanced with Sorting, Filtering, and Paging
        public async Task<IActionResult> Index(
            string searchString, 
            int? categoryId, 
            string sortOrder, 
            int? page,
            decimal? minPrice,
            decimal? maxPrice,
            bool? isAvailable)
        {
            // Sorting parameters
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            
            // Filter parameters
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;
            ViewData["CurrentAvailability"] = isAvailable;
            
            // Create a LINQ query to select comic books with includes
            var comicBooksQuery = _context.ComicBooks
                .Include(c => c.Category)
                .AsQueryable();
            
            // Apply filtering based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                comicBooksQuery = comicBooksQuery.Where(c => 
                    c.Title.Contains(searchString) || 
                    c.Author.Contains(searchString) || 
                    c.Artist.Contains(searchString) ||
                    c.Description.Contains(searchString));
            }
            
            // Apply filtering based on category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                comicBooksQuery = comicBooksQuery.Where(c => c.CategoryId == categoryId.Value);
            }
            
            // Apply price range filtering
            if (minPrice.HasValue)
            {
                comicBooksQuery = comicBooksQuery.Where(c => c.Price >= minPrice.Value);
            }
            
            if (maxPrice.HasValue)
            {
                comicBooksQuery = comicBooksQuery.Where(c => c.Price <= maxPrice.Value);
            }
            
            // Apply availability filtering
            if (isAvailable.HasValue)
            {
                comicBooksQuery = comicBooksQuery.Where(c => c.IsAvailable == isAvailable.Value);
            }
            
            // Apply sorting
            switch (sortOrder)
            {
                case "title_desc":
                    comicBooksQuery = comicBooksQuery.OrderByDescending(c => c.Title);
                    break;
                case "Author":
                    comicBooksQuery = comicBooksQuery.OrderBy(c => c.Author);
                    break;
                case "author_desc":
                    comicBooksQuery = comicBooksQuery.OrderByDescending(c => c.Author);
                    break;
                case "Price":
                    comicBooksQuery = comicBooksQuery.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    comicBooksQuery = comicBooksQuery.OrderByDescending(c => c.Price);
                    break;
                case "Date":
                    comicBooksQuery = comicBooksQuery.OrderBy(c => c.CreatedDate);
                    break;
                case "date_desc":
                    comicBooksQuery = comicBooksQuery.OrderByDescending(c => c.CreatedDate);
                    break;
                default:
                    comicBooksQuery = comicBooksQuery.OrderBy(c => c.Title);
                    break;
            }
            
            // Paging
            int pageSize = 10;
            int pageNumber = page ?? 1;
            
            // Get total count for pagination
            var totalItems = await comicBooksQuery.CountAsync();
            ViewData["TotalItems"] = totalItems;
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalItems / pageSize);
            
            // Execute the query with paging
            var comicBooks = await comicBooksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            // Get categories for the filter dropdown
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
                
            return View(comicBooks);
        }

        // GET: ComicBooks/GroupedByCategory - Example of Grouping with EF Core
        public async Task<IActionResult> GroupedByCategory()
        {
            var groupedComics = await _context.ComicBooks
                .Include(c => c.Category)
                .GroupBy(c => new { c.CategoryId, c.Category.Name })
                .Select(g => new
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    ComicCount = g.Count(),
                    AveragePrice = g.Average(c => c.Price),
                    TotalValue = g.Sum(c => c.Price),
                    MinPrice = g.Min(c => c.Price),
                    MaxPrice = g.Max(c => c.Price),
                    Comics = g.OrderBy(c => c.Title).ToList()
                })
                .OrderBy(g => g.CategoryName)
                .ToListAsync();

            return View(groupedComics);
        }

        // GET: ComicBooks/Statistics - Example of Aggregation with EF Core
        public async Task<IActionResult> Statistics()
        {
            var stats = new StatisticsViewModel
            {
                TotalComics = await _context.ComicBooks.CountAsync(),
                AvailableComics = await _context.ComicBooks.CountAsync(c => c.IsAvailable),
                TotalValue = await _context.ComicBooks.SumAsync(c => c.Price),
                AveragePrice = await _context.ComicBooks.AverageAsync(c => c.Price),
                MostExpensive = await _context.ComicBooks.MaxAsync(c => c.Price),
                Cheapest = await _context.ComicBooks.MinAsync(c => c.Price),
                CategoryStats = await _context.Categories
                    .Select(cat => new CategoryStatistic
                    {
                        CategoryId = cat.Id,
                        CategoryName = cat.Name,
                        ComicCount = cat.ComicBooks.Count(),
                        AveragePrice = cat.ComicBooks.Any() ? cat.ComicBooks.Average(c => c.Price) : 0
                    })
                    .ToListAsync(),
                RecentComics = await _context.ComicBooks
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(5)
                    .Select(c => new RecentComic { Title = c.Title, Author = c.Author, CreatedDate = c.CreatedDate })
                    .ToListAsync()
            };

            return View(stats);
        }

        // GET: ComicBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicBook = await _context.ComicBooks
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comicBook == null)
            {
                return NotFound();
            }

            // Pass category name to view
            ViewBag.CategoryName = comicBook.Category?.Name ?? "Unknown";

            return View(comicBook);
        }

        // GET: ComicBooks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: ComicBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Author,Artist,PublicationDate,PageCount,Price,IsAvailable,CoverImageUrl,CategoryId")] ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                comicBook.CreatedDate = DateTime.Now;
                _context.Add(comicBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", comicBook.CategoryId);
            return View(comicBook);
        }

        // GET: ComicBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", comicBook.CategoryId);
            return View(comicBook);
        }

        // POST: ComicBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,Artist,PublicationDate,PageCount,Price,IsAvailable,CoverImageUrl,CategoryId,CreatedDate")] ComicBook comicBook)
        {
            if (id != comicBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comicBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicBookExists(comicBook.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", comicBook.CategoryId);
            return View(comicBook);
        }

        // GET: ComicBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicBook = await _context.ComicBooks
                .Include(c => c.CategoryId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comicBook == null)
            {
                return NotFound();
            }

            return View(comicBook);
        }

        // POST: ComicBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook != null)
            {
                _context.ComicBooks.Remove(comicBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComicBookExists(int id)
        {
            return _context.ComicBooks.Any(e => e.Id == id);
        }
        
        // GET: ComicBooks/Search
        public IActionResult Search()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        
        // POST: ComicBooks/Search
        [HttpPost]
        public async Task<IActionResult> SearchResults(string searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.ComicBooks.AsQueryable();
            
            // Apply search filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => 
                    c.Title.Contains(searchTerm) || 
                    c.Author.Contains(searchTerm) || 
                    c.Artist.Contains(searchTerm) || 
                    c.Description.Contains(searchTerm));
            }
            
            if (categoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }
            
            if (minPrice.HasValue)
            {
                query = query.Where(c => c.Price >= minPrice.Value);
            }
            
            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= maxPrice.Value);
            }
            
            var results = await query
                .Include("Category")
                .OrderBy(c => c.Title)
                .ToListAsync();
                
            return View(results);
        }
    }
}
