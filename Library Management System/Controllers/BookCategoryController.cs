using Library_Management_System.DAL;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Book;
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

        #region Index

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

        [HttpGet("bookcategoriess/search")]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["SearchTerm"] = search;

            if (string.IsNullOrEmpty(search))
            {
                var bookCategories = await _context.BookCategories.ToListAsync();
                var mappedBookCategories = bookCategories.Select(b => new BookCategoryVM()
                {
                   Id=b.Id,
                   Name = b.Name,
                }).ToList();
                return View(mappedBookCategories);
            }

            var filteredBookCategories = await _context.BookCategories.Where(x => x.Name.Contains(search)).ToListAsync();

            if (!filteredBookCategories.Any())
            {
                TempData[AlertHelper.Error] = "No book categories found matching the search term!";
                return View();
            }

            var mappedFilteredBookCategories = filteredBookCategories.Select(x => new BookCategoryVM()
            {
               Id=x.Id,
               Name = x.Name,
            }).ToList();

            return View(mappedFilteredBookCategories);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCategoryCreateVM bookCategoryCreateVM)
        {
            try
            {
            if (!ModelState.IsValid)
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
            catch
            {
                return View("_Error");
            }
        }
        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var bookCategory = await _context.BookCategories.FindAsync(id);
                if (bookCategory is null)
                {
                    TempData[AlertHelper.Error] = "Book Category not found!";
                    return RedirectToAction(nameof(Index));
                }

                _context.BookCategories.Remove(bookCategory);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Book Category successfully deleted!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("_Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                var bookCategories = await _context.BookCategories.ToListAsync();

                if (!bookCategories.Any())
                {
                    TempData[AlertHelper.Error] = "No Book Categories found to delete!";
                    return RedirectToAction(nameof(Index));
                }

                _context.BookCategories.RemoveRange(bookCategories);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "All Book Categories successfully deleted!";

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("_Error");
            }
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var bookCategory = await _context.BookCategories.FindAsync(id);

            if (bookCategory is null)
            {
                TempData[AlertHelper.Error] = "Book Category not found.";
                return RedirectToAction(nameof(Index));
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
            try
            {

            var bookCategory = await _context.BookCategories.FindAsync(id);

            if (bookCategory is null)
            {
                TempData[AlertHelper.Error] = "Book Category not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to save the book category's details.";
                return View(bookCategoryUpdateVM);
            }


            bookCategory.Name = bookCategoryUpdateVM.Name;

            _context.Update(bookCategory);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Book Category successfully updated!";

            return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("_Error");
            }
        }

        #endregion

        #region Detail

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var bookCategory = await _context.BookCategories
                .Include(bc=>bc.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(bc=>bc.Id == id);

            if (bookCategory is null)
            {
                TempData[AlertHelper.Error] = "Book Category not found.";
                return RedirectToAction(nameof(Index));
            }

            var bookCategoryDetailVM = new BookCategoryDetailVM()

            {
                Id = id,
                Name = bookCategory.Name,
                BookVMs = bookCategory.Books.Select(x => new BookVM()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList()

            };

            return View(bookCategoryDetailVM);
        }

        #endregion

    }
}
