using System.Diagnostics;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

      

        
    }
}
