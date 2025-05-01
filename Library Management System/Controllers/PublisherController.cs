using Library_Management_System.DAL;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.ViewModels.Publisher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    public class PublisherController : Controller
    {
        private readonly LibraryManagementSystemDbContext _context;

        public PublisherController(LibraryManagementSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var publisher = await _context.Publishers.AsNoTracking().ToListAsync();
            var publisherVMs = publisher.Select(p => new PublisherVM()
            {
                Id = p.Id,
                Name = p.Name,
                Adress = p.Adress,
                Rating = p.Rating,
            }).ToList();

            return View(publisherVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateVM publisherCreateVM)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to create the publisher.";
                return View(publisherCreateVM);
            }

            Publisher publisher = new Publisher()
            {
                Name = publisherCreateVM.Name,
                Adress = publisherCreateVM.Adress,
                Rating = publisherCreateVM.Rating
            };

            await _context.AddAsync(publisher);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Publisher successfully created!";


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var publisher = await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);

                if (publisher is null)
                {
                    TempData[AlertHelper.Error] = "Publisher not found!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "Publisher successfully deleted!";
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
            var publisher = await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);

            if (publisher is null)
            {
                return NotFound();
            }

            var publisherVM = new PublisherUpdateVM()
            {
                Name = publisher.Name,
                Adress = publisher.Adress,
                Rating = publisher.Rating,
            };

            return View(publisherVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, PublisherUpdateVM publisherUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Validation failed. Unable to save the publisher's details.";
                return View(publisherUpdateVM);
            }

            var publisher = await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);

            if(publisher is null)
            {
                return NotFound();
            }

            publisher.Name = publisherUpdateVM.Name;
            publisher.Adress = publisherUpdateVM.Adress;
            publisher.Rating = publisherUpdateVM.Rating;

            _context.Update(publisher);
            await _context.SaveChangesAsync();

            TempData[AlertHelper.Success] = "Publisher successfully updated!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var publisher = await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);

            if (publisher is null) return NotFound();

            var publisherVM = new PublisherDetailVM()
            {
                Id = id,
                Name = publisher.Name,
                Adress = publisher.Adress,
                Rating = publisher.Rating,
            };

            return View(publisherVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                var publishers = await _context.Publishers.ToListAsync();

                if (!publishers.Any())
                {
                    TempData[AlertHelper.Error] = "No Publishers found to delete!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Publishers.RemoveRange(publishers);
                await _context.SaveChangesAsync();

                TempData[AlertHelper.Success] = "All Publishers successfully deleted!";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("_Error");
            }
        }


    }
}
