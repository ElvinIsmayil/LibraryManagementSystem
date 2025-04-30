using Library_Management_System.DAL;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Author;
using Library_Management_System.ViewModels.BookCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class BookCategoryController : Controller
    {
        private readonly LibraryManagementSystemDbContext _context;

        public BookCategoryController(LibraryManagementSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookCategory = await _context.BookCategories.AsNoTracking().ToListAsync();
            var categoryVMs = bookCategory.Select(bc => new BookCategoryVM()
            {
                Id = bc.Id,
                Name = bc.Name,
            }).ToList();

            return View(categoryVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCategoryCreateVM bookCategoryCreateVM)
        {
            if(!ModelState.IsValid)
            {
                return View(bookCategoryCreateVM);
            }

            BookCategory bookCategory = new BookCategory()
            {
                Name = bookCategoryCreateVM.Name,
            };

            await _context.AddAsync(bookCategory);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Book Category successfully created!";


            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var bookCategory = await _context.BookCategories.FirstOrDefaultAsync(x => x.Id == id);
                if (bookCategory is null)
                {
                    TempData[AlertHelper.Error] = "bookCategory not found!";
                    return RedirectToAction(nameof(Index));
                }

              
                _context.BookCategories.Remove(bookCategory);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "bookCategory successfully deleted!";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("_Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var bookCategory = await _context.BookCategories.FirstOrDefaultAsync(x=>x.Id == id);

            if(bookCategory is null)
            {
                return NotFound();
            }

            var bookCategoryVM = new BookCategoryUpdateVM()
            {
                Name = bookCategory.Name
            };

            return View(bookCategoryVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BookCategoryUpdateVM bookCategoryUpdateVM)
        {
            var bookCategory = await _context.BookCategories.FirstOrDefaultAsync(x => x.Id == id);

            if(!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to save the author's details.";
                return View(bookCategoryUpdateVM);
            }
            bookCategory.Name = bookCategoryUpdateVM.Name;

            _context.Update(bookCategory);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Author successfully updated!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var bookCategory = await _context.BookCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (bookCategory is null) return NotFound();

            var bookCategoryVM = new BookCategoryDetailVM()
            {
                Id = id,
                Name = bookCategory.Name,
            };

            return View(bookCategoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                var bookCategories = await _context.BookCategories.AsNoTracking().ToListAsync();

                if (!bookCategories.Any())
                {
                    TempData[AlertHelper.Error] = "No bookCategories found to delete!";
                    return RedirectToAction(nameof(Index));
                }

                _context.BookCategories.RemoveRange(bookCategories);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "All bookCategories successfully deleted!";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("_Error");
            }
        }


    }
}
