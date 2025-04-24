using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
