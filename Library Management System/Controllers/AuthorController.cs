using Library_Management_System.DAL;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Author;
using Library_Management_System.ViewModels.AuthorContact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly LibraryManagementSystemDbContext _context;

        public AuthorController(IWebHostEnvironment env, LibraryManagementSystemDbContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var author = await _context.Authors.AsNoTracking().ToListAsync();
            var authorVMs = author.Select(a => new AuthorVM()
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname,
                BirthDate = a.BirthDate,
                ImageUrl = a.ImageUrl
            }).ToList();
            return View(authorVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateVM authorCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(authorCreateVM);
            }
            string imageUrl;

            if (authorCreateVM.Gender == Enums.Gender.Male)
            {
             imageUrl = "/img/maleAuthor.png";  
            }
            else
            {
            imageUrl = "/img/femaleAuthor.png";  

            }

            if (authorCreateVM.Image != null)
            {
                if (!authorCreateVM.Image.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Image", "You can only upload image files!");
                    return View(authorCreateVM);
                }

                if (authorCreateVM.Image.Length > 50 * 1024)
                {
                    ModelState.AddModelError("Image", "The image file size should not be larger than 50 KB");
                    return View(authorCreateVM);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(authorCreateVM.Image.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "authors");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await authorCreateVM.Image.CopyToAsync(stream);
                }

                imageUrl = "/uploads/authors/" + fileName;
            }

            AuthorContact authorContact = new AuthorContact
            {
                PhoneNumber = authorCreateVM.AuthorContactCreateVM.PhoneNumber,
                Email = authorCreateVM.AuthorContactCreateVM.Email
            };

            Author author = new Author
            {
                Name = authorCreateVM.Name,
                Surname = authorCreateVM.Surname,
                BirthDate = authorCreateVM.BirthDate,
                Gender = authorCreateVM.Gender,
                ImageUrl = imageUrl, 
                Biography = authorCreateVM.Biography,
                ContactDetails = authorContact
            };

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Author successfully created!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                TempData["ErrorMessage"] = "Author not found!";

                return RedirectToAction(nameof(Index));
            }

            string oldImagePath = Path.Combine(_env.WebRootPath, "uploads", "authors", author.ImageUrl);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Author successfully deleted!";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x=> x.Id == id);

            if (author is null)
            {
                return NotFound();
            }

            AuthorContactUpdateVM authorContactUpdateVM = new AuthorContactUpdateVM
            {
                PhoneNumber = author.ContactDetails?.PhoneNumber,
                Email = author.ContactDetails?.Email,    
               
            };

            var authorVM = new AuthorUpdateVM()
            {
                Name = author.Name,
                Surname = author.Surname,
                Gender = author.Gender,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                ImageUrl = author.ImageUrl,
                AuthorContactUpdateVM = authorContactUpdateVM,
            };


            return View(authorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, AuthorUpdateVM authorUpdateVM)
        {
            var author = await _context.Authors
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Include(a => a.ContactDetails)
                .FirstOrDefaultAsync();


            authorUpdateVM.ImageUrl = author.ImageUrl;

            if(!ModelState.IsValid)
            {
                return View(authorUpdateVM);
            }

            if(authorUpdateVM.Image is null)
            {
                author.Name = authorUpdateVM.Name;
                author.Surname = authorUpdateVM.Surname;
                author.Gender = authorUpdateVM.Gender;
                author.BirthDate = authorUpdateVM.BirthDate;
                author.ContactDetails = new AuthorContact
                {
                    Email = authorUpdateVM.AuthorContactUpdateVM.Email,
                    PhoneNumber = authorUpdateVM.AuthorContactUpdateVM.PhoneNumber
                };

                _context.Update(author);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            else
            {
              
                string imageUrl;

                if (authorUpdateVM.Gender == Enums.Gender.Male)
                {
                    imageUrl = "/img/maleAuthor.png";
                }
                else
                {
                    imageUrl = "/img/femaleAuthor.png";

                }

                if (authorUpdateVM.Image != null)
                {
                    if (!authorUpdateVM.Image.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("Image", "You can only upload image files!");
                        return View(authorUpdateVM);
                    }

                    if (authorUpdateVM.Image.Length > 50 * 1024)
                    {
                        ModelState.AddModelError("Image", "The image file size should not be larger than 50 KB");
                        return View(authorUpdateVM);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(authorUpdateVM.Image.FileName);
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "authors");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await authorUpdateVM.Image.CopyToAsync(stream);
                    }

                    imageUrl = "/uploads/authors/" + fileName;
                }

                author.Name = authorUpdateVM.Name;
                author.Surname = authorUpdateVM.Surname;
                author.Gender = authorUpdateVM.Gender;
                author.BirthDate = authorUpdateVM.BirthDate;
                author.Biography = authorUpdateVM.Biography;
                author.ImageUrl = imageUrl;

                 _context.Update(author);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Author successfully updated!";

                return RedirectToAction(nameof(Index));
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var author = await _context.Authors
                .Where(a=> a.Id==id)
                .Include(a=> a.ContactDetails)
                .FirstOrDefaultAsync();

            if(author is null) return NotFound();
            

            var authorVM = new AuthorDetailVM()
            {
                Id = id,
                Name = author.Name,
                Surname = author.Surname,
                ImageUrl = author.ImageUrl,
                BirthDate = author.BirthDate,
                Biography = author.Biography,
                Gender = author.Gender,
                AuthorContactDetailVM = new AuthorContactDetailVM()
                {
                    PhoneNumber = author.ContactDetails.PhoneNumber,
                    Email = author.ContactDetails.Email
                }
            };

            return View(authorVM);
        }

        

    }
}
