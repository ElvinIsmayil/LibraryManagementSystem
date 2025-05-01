using Library_Management_System.DAL;
using Library_Management_System.Extensions;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryManagementSystemDbContext _context;

        public BookController(LibraryManagementSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.AsNoTracking().ToListAsync();
           
            var bookVMs = books.Select(x => new BookVM()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                PublishedDate = x.PublishedDate,
                ImageUrl = x.ImageUrl,
            }).ToList();
            return View(bookVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM bookCreateVM)
        {
            try
            {
                await LoadDropdownsAsync();

                bookCreateVM.Image.FileTypeCheck(ModelState);

                if (bookCreateVM.SelectedAuthorIds == null || !bookCreateVM.SelectedAuthorIds.Any())
                {
                    ModelState.AddModelError("SelectedAuthorIds", "At least one author must be selected.");
                    return View(bookCreateVM);
                }
                if (!ModelState.IsValid)
                {
                    return View(bookCreateVM);
                }


                Book book = new Book()
                {
                    Title = bookCreateVM.Title,
                    Description = bookCreateVM.Description,
                    PublishedDate = bookCreateVM.PublishedDate,
                    PageCount = bookCreateVM.PageCount,
                    BookCategoryId = bookCreateVM.BookCategoryId,
                    PublisherId = bookCreateVM.PublisherId
                };

                if (bookCreateVM.Image == null)
                {
                    book.ImageUrl = "/img/default/book-cover-placeholder.png";
                }
                else
                {
                    book.ImageUrl = await bookCreateVM.Image.SaveImage("books");
                }

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync(); 

                foreach (var authorId in bookCreateVM.SelectedAuthorIds)
                {
                    var bookAuthor = new BookAuthor
                    {
                        BookId = book.Id,
                        AuthorId = authorId
                    };
                    _context.BookAuthors.Add(bookAuthor);
                }

                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Book successfully created!";
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           var book = await _context.Books.FirstOrDefaultAsync(x=>x.Id==id);

           if(book is null)
            {
                TempData[AlertHelper.Error] = "Book not found!";
                return RedirectToAction(nameof(Index));
            }
            book.ImageUrl.DeleteImageFromLocal();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Book successfully deleted!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           await LoadDropdownsAsync();

            var book = await _context.Books
            .Include(b => b.BookCategory)
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookUpdateVM = new BookUpdateVM()
            {
                Title = book.Title,
                Description = book.Description,
                PageCount = book.PageCount,
                PublishedDate = book.PublishedDate,
                ImageUrl = book.ImageUrl,
                BookCategoryId = book.BookCategoryId,
                PublisherId = book.PublisherId,
                SelectedAuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList() 
            };


            return View(bookUpdateVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BookUpdateVM bookUpdateVM)
        {
            await LoadDropdownsAsync(); 

            var book = await _context.Books
                .Where(x => x.Id == id)
                .Include(b => b.BookAuthors)  
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync();

            if (book is null)
            { 
                return NotFound();  
            }

            bookUpdateVM.ImageUrl = book.ImageUrl;

            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to save the book's details.";
                return View(bookUpdateVM);
            }

            if (bookUpdateVM.Image == null)
            {
                book.Title = bookUpdateVM.Title;
                book.Description = bookUpdateVM.Description;
                book.PageCount = bookUpdateVM.PageCount;
                book.PublisherId = bookUpdateVM.PublisherId;
                book.BookCategoryId = bookUpdateVM.BookCategoryId;

                _context.Update(book);
                UpdateAuthors(book, bookUpdateVM.SelectedAuthorIds);
                await _context.SaveChangesAsync();


                TempData[AlertHelper.Success] = "Book successfully updated!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                bookUpdateVM.ImageUrl.DeleteImageFromLocal();
                bookUpdateVM.Image.FileTypeCheck(ModelState); 

                if (!ModelState.IsValid)
                {
                    return View(bookUpdateVM);  
                }

                book.ImageUrl = await bookUpdateVM.Image.SaveImage("books");
                book.Title = bookUpdateVM.Title;
                book.Description = bookUpdateVM.Description;
                book.PageCount = bookUpdateVM.PageCount;
                book.PublishedDate = bookUpdateVM.PublishedDate;
                book.BookCategoryId = bookUpdateVM.BookCategoryId;
                book.PublisherId = bookUpdateVM.PublisherId;

                UpdateAuthors(book, bookUpdateVM.SelectedAuthorIds);

                _context.Update(book);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Book successfully updated!";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _context.Books
                .Include(b=>b.BookCategory)
                .Include(b=>b.Publisher)
                .Include(b=>b.BookAuthors)
                .ThenInclude(b=>b.Author)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (book is null) return NotFound();

            var bookVM = new BookDetailVM()
            {
                Id = id,
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                PageCount = book.PageCount,
                PublishedDate = book.PublishedDate,
                BookCategoryName = book.BookCategory.Name,
                AuthorNames = book.BookAuthors.Select(ba => ba.Author.Name).ToList(),
                PublisherName = book.Publisher.Name,
            };

            return View(bookVM);

            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                var books = await _context.Books.AsNoTracking().ToListAsync();

                if (!books.Any())
                {
                    TempData[AlertHelper.Error] = "No books found to delete!";
                    return RedirectToAction(nameof(Index));
                }
               foreach (var book in books)
                {
                    book.ImageUrl.DeleteImageFromLocal();   
                }

                _context.Books.RemoveRange(books);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "All books successfully deleted!";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("_Error");
            }
        }

        private void UpdateAuthors(Book book, List<int> selectedAuthorIds)
        {
            var existingAuthors = _context.BookAuthors.Where(ba => ba.BookId == book.Id).ToList();
            _context.BookAuthors.RemoveRange(existingAuthors);

            foreach (var authorId in selectedAuthorIds)
            {
                var bookAuthor = new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = authorId
                };
                _context.BookAuthors.Add(bookAuthor);
            }
        }

        private async Task LoadDropdownsAsync()
        {
            ViewBag.Categories = await _context.BookCategories
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            ViewBag.Authors = await _context.Authors
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            ViewBag.Publishers = await _context.Publishers
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();
        }


    }
}
