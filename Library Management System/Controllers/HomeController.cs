using System.Diagnostics;
using Library_Management_System.DAL;
using Library_Management_System.Models;
using Library_Management_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryManagementSystemDbContext _context;

        public HomeController(LibraryManagementSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var totalBooks = await _context.Books.CountAsync();
            var totalAuthors = await _context.Authors.CountAsync();
            var totalCategories = await _context.BookCategories.CountAsync();
            var totalPublishers = await _context.Publishers.CountAsync();

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var booksAddedThisMonth = await _context.Books
                .CountAsync(b => b.CreatedDate >= startOfMonth);

            var latestBooks = await _context.Books
           .Include(b => b.BookAuthors)
               .ThenInclude(ba => ba.Author)
           .OrderByDescending(b => b.CreatedDate)
           .Take(5)
           .Select(b => new LatestBookItemVM
           {
               Title = b.Title,
               AuthorNames = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.Name + " " + ba.Author.Surname)),
               CreatedDate = b.CreatedDate
           })
           .ToListAsync();


            var dashboardVM = new DashboardVM
            {
                TotalBooks = totalBooks,
                TotalAuthors = totalAuthors,
                TotalCategories = totalCategories,
                TotalPublishers = totalPublishers,
                BooksAddedThisMonth = booksAddedThisMonth,
                LatestBooks = latestBooks
            };

            var categoryData = await _context.BookCategories
    .Select(c => new
    {
        Name = c.Name,
        BookCount = c.Books.Count()
    })
    .ToListAsync();

            dashboardVM.CategoryNames = categoryData.Select(c => c.Name).ToList();
            dashboardVM.CategoryBookCounts = categoryData.Select(c => c.BookCount).ToList();

            return View(dashboardVM);
        }






    }
}
