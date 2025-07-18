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

        // GET: ComicBooks
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            // Create a LINQ query to select comic books
            var comicBooksQuery = _context.ComicBooks.AsQueryable();
            
            // Apply filtering based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                comicBooksQuery = comicBooksQuery.Where(c => 
                    c.Title.Contains(searchString) || 
                    c.Author.Contains(searchString) || 
                    c.Description.Contains(searchString));
            }
            
            // Apply filtering based on category
            if (categoryId.HasValue)
            {
                comicBooksQuery = comicBooksQuery.Where(c => c.CategoryId == categoryId.Value);
            }
            
            // Get categories for the filter dropdown
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = categoryId;
            
            // Execute the query and return the results
            var comicBooks = await comicBooksQuery
                .Include("Category")
                .OrderBy(c => c.Title)
                .ToListAsync();
                
            return View(comicBooks);
        }

        // GET: ComicBooks/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ComicBooks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: ComicBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Author,Artist,IssueNumber,PublicationDate,PageCount,Price,IsAvailable,CoverImageUrl,CategoryId")] ComicBook comicBook)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,Artist,IssueNumber,PublicationDate,PageCount,Price,IsAvailable,CoverImageUrl,CategoryId,CreatedDate")] ComicBook comicBook)
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
