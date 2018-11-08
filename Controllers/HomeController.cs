using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ISA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
