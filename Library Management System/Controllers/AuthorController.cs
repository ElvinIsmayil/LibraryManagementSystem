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
            catch (Exception ex)
            {
                return View("_Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM authorCreateVM)
        {
            try
            {
                authorCreateVM.Image.FileTypeCheck(ModelState);
                authorCreateVM.AuthorContactCreateVM.ValidateContactInfo(ModelState);

                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Validation failed. Unable to create the author";
                    return View(authorCreateVM);
                }

                AuthorContact authorContact = new AuthorContact
                {
                    PhoneNumber = authorCreateVM.AuthorContactCreateVM.PhoneNumber,
                    Email = authorCreateVM.AuthorContactCreateVM.Email
                };

                if(authorCreateVM.Image is null)
                {
                    authorCreateVM.ImageUrl =  SetDefaultImage(authorCreateVM.Gender);
                }
                else
                {
                    authorCreateVM.ImageUrl = await authorCreateVM.Image.SaveImage("authors");
                }

                Author author = new Author
                {
                    Name = authorCreateVM.Name,
                    Surname = authorCreateVM.Surname,
                    BirthDate = authorCreateVM.BirthDate,
                    Gender = authorCreateVM.Gender,
                    ImageUrl = authorCreateVM.ImageUrl,
                    Biography = authorCreateVM.Biography,
                    ContactDetails = authorContact
                };


                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();


                TempData[AlertHelper.Success] = "Author successfully created!";

                
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("_Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);

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

            }catch (Exception ex)
            {
                return View("_Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {

                var author = await _context.Authors
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Include(a => a.ContactDetails)
                .FirstOrDefaultAsync();

                if (author == null)
                {
                    return NotFound(); 
                }

                var authorVM = new AuthorUpdateVM()
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

                var genderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Male", Text = "Male" },
                new SelectListItem { Value = "Female", Text = "Female" }
            };

                ViewBag.GenderList = new SelectList(genderList, "Value", "Text", author.Gender);


                return View(authorVM);
            }
            catch (Exception ex)
            {
                return View("_Error");
            }
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
                    return NotFound();
                }

            authorUpdateVM.ImageUrl = author.ImageUrl;

            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to save the author's details.";
                return View(authorUpdateVM);
            }

            if (authorUpdateVM.Image is null)
            {
                author.ImageUrl = SetDefaultImage(authorUpdateVM.Gender);
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

                TempData[AlertHelper.Success] = "Author successfully updated!";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                authorUpdateVM.ImageUrl.DeleteImageFromLocal();
                authorUpdateVM.Image.FileTypeCheck(ModelState);

                authorUpdateVM.ImageUrl = await authorUpdateVM.Image.SaveImage("authors");

                author.Name = authorUpdateVM.Name;
                author.Surname = authorUpdateVM.Surname;
                author.Gender = authorUpdateVM.Gender;
                author.BirthDate = authorUpdateVM.BirthDate;
                author.Biography = authorUpdateVM.Biography;
                author.ImageUrl = authorUpdateVM.ImageUrl;
                author.ContactDetails = new AuthorContact
                {
                    Email = authorUpdateVM.AuthorContactUpdateVM.Email,
                    PhoneNumber = authorUpdateVM.AuthorContactUpdateVM.PhoneNumber
                };

                _context.Update(author);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Author successfully updated!";

                return RedirectToAction(nameof(Index));

            }
            }catch(Exception ex)
            {
                return View("_Error");
            }


        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
            var author = await _context.Authors
                .Where(a => a.Id == id)
                .Include(a => a.ContactDetails)
                .FirstOrDefaultAsync();

            if (author is null) return NotFound();


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

            }catch(Exception ex)
            {
                return View("_Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
            var authors = await _context.Authors.AsNoTracking().ToListAsync();

            if (!authors.Any())
            {
                TempData[AlertHelper.Error] = "No authors found to delete!";
                return RedirectToAction(nameof(Index));
            }
            foreach(var author in authors)
                {
                    author.ImageUrl.DeleteImageFromLocal();
                }
          
            _context.Authors.RemoveRange(authors);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "All authors successfully deleted!";

            return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                return View("_Error");
            }
        }

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

        
    }
}
