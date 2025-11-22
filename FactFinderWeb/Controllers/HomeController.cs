using FactFinderWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FactFinderWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		[HttpGet]
        public IActionResult Index()
        {
			return RedirectToAction("login", "User");
            
        }
 
    }
}
