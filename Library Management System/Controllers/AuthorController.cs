using Library_Management_System.DAL;
using Library_Management_System.Enums;
using Library_Management_System.Extensions;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Author;
using Library_Management_System.ViewModels.AuthorContact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibraryManagementSystemDbContext _context;

        public AuthorController(LibraryManagementSystemDbContext context)
        {
            _context = context;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
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
            catch
            {
                return View("_Error");
            }
        }
        [HttpGet("authors/search")]
        public async Task<IActionResult> Index(string search)
        {

            ViewData["SearchTerm"] = search;

            if (string.IsNullOrEmpty(search))
            {
                var authors = await _context.Authors.ToListAsync();
                var mappedAuthors = authors.Select(a => new AuthorVM()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    BirthDate = a.BirthDate,
                    ImageUrl = a.ImageUrl

                }).ToList();
                return View(mappedAuthors);
            }


            var filteredAuthors = await _context.Authors
                .Where(x => x.Name.Contains(search))
                .ToListAsync();

            if (!filteredAuthors.Any())
            {
                TempData[AlertHelper.Error] = "No authors found matching the search term!";
                return View();
            }

            var mappedFilteredAuthors = filteredAuthors.Select(a => new AuthorVM()
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname,
                BirthDate = a.BirthDate,
                ImageUrl = a.ImageUrl
            }).ToList();

            return View(mappedFilteredAuthors);
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
        public async Task<IActionResult> Create(AuthorCreateVM authorCreateVM)
        {
            if (authorCreateVM.Image is not null)
            {
                authorCreateVM.Image.FileTypeCheck(ModelState);
            }

            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to create the author";
                return View(authorCreateVM);
            }
            try
            {

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
                    ImageUrl = await SetAuthorImage(authorCreateVM.Image, authorCreateVM.Gender),
                    Biography = authorCreateVM.Biography,
                    ContactDetails = authorContact
                };

                author.ValidateBirthdate(ModelState);
                authorContact.ValidateContactInfo(ModelState);

                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Validation failed. Unable to create the author";
                    return View(authorCreateVM);
                }

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();


                TempData[AlertHelper.Success] = "Author successfully created!";


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
                var author = await _context.Authors.FindAsync(id);

                if (author is null)
                {
                    TempData[AlertHelper.Error] = "Author not found!";
                    return RedirectToAction(nameof(Index));
                }

                author.ImageUrl.DeleteImageFromLocal();

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Author successfully deleted!";
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
                var authors = await _context.Authors.ToListAsync();

                if (!authors.Any())
                {
                    TempData[AlertHelper.Error] = "No authors found to delete!";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var author in authors)
                {
                    author.ImageUrl.DeleteImageFromLocal();
                }

                _context.Authors.RemoveRange(authors);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "All authors successfully deleted!";

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
            var author = await _context.Authors
            .Include(a => a.ContactDetails)
            .FirstOrDefaultAsync(x => x.Id == id);


            if (author is null)
            {
                TempData[AlertHelper.Error] = "Author not found.";
                return NotFound();
            }

            LoadGenderDropdown(author);

            var authorUpdateVM = new AuthorUpdateVM()
            {
                Name = author.Name,
                Surname = author.Surname,
                Gender = author.Gender,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                ImageUrl = author.ImageUrl,
                AuthorContactUpdateVM = new AuthorContactUpdateVM()
                {
                    Email = author.ContactDetails.Email,
                    PhoneNumber = author.ContactDetails.PhoneNumber
                }
            };
            return View(authorUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, AuthorUpdateVM authorUpdateVM)
        {
            try
            {
                var author = await _context.Authors
                    .Where(a => a.Id == id)
                    .Include(a => a.ContactDetails)
                    .FirstOrDefaultAsync();

                if (author is null)
                {
                    TempData[AlertHelper.Error] = "Author not found.";
                    return NotFound();
                }

                LoadGenderDropdown(author);

                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Validation failed. Unable to save the author's details.";
                    return View(authorUpdateVM);
                }

                author.Name = authorUpdateVM.Name;
                author.Surname = authorUpdateVM.Surname;
                author.Gender = authorUpdateVM.Gender;
                author.BirthDate = authorUpdateVM.BirthDate;
                author.Biography = authorUpdateVM.Biography;

                author.ContactDetails = new AuthorContact
                {
                    Email = authorUpdateVM.AuthorContactUpdateVM.Email,
                    PhoneNumber = authorUpdateVM.AuthorContactUpdateVM.PhoneNumber
                };

                if (authorUpdateVM.Image is null)
                {
                    author.ImageUrl = SetDefaultImage(authorUpdateVM.Gender);
                }
                else
                {
                    authorUpdateVM.ImageUrl.DeleteImageFromLocal();
                    authorUpdateVM.Image.FileTypeCheck(ModelState);
                    author.ImageUrl = await authorUpdateVM.Image.SaveImage("authors");
                }

                author.ValidateBirthdate(ModelState);
                author.ContactDetails.ValidateContactInfo(ModelState);

                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Validation failed. Unable to save the author's details.";
                    return View(authorUpdateVM);
                }

                _context.Update(author);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Author successfully updated!";
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

            var author = await _context.Authors
                .Include(a => a.ContactDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (author is null)
            {
                TempData[AlertHelper.Error] = "Author not found.";
                return NotFound();
            }

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

        #endregion

        #region Helper Methods


        private string SetDefaultImage(Gender gender)
        {
            string imageUrl;

            string maleImage = "/img/default/male-avatar.png";
            string femaleImage = "/img/default/female-avatar.png";

            if (gender is Enums.Gender.Male)
            {
                imageUrl = maleImage;
            }
            else
            {
                imageUrl = femaleImage;
            }

            return imageUrl;
        }

        private void LoadGenderDropdown(Author author)
        {
            var genderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Male", Text = "Male" },
                new SelectListItem { Value = "Female", Text = "Female" }
            };

            ViewBag.GenderList = new SelectList(genderList, "Value", "Text", author.Gender);
        }

        private async Task<string> SetAuthorImage(IFormFile image, Gender gender)
        {
            if (image is null)
                return SetDefaultImage(gender);
            else
                return await image.SaveImage("authors");
        }


        #endregion
    }
}
