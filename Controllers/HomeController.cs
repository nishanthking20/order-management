using Microsoft.AspNetCore.Mvc;
using Order_Management.Models;
using System.Diagnostics;

namespace Order_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public int id;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            // Console.WriteLine(id);
            return View();
        }

        public IActionResult Login()
        {
            // Console.WriteLine(id);
            return View();
        }
        public IActionResult Register()
        {
            // Console.WriteLine(id);
            return View();
        }
        public IActionResult Dashboard()
        {
            // Console.WriteLine(id);
            return View("~/Views/Shared/Dashboard.cshtml");
        }
        public IActionResult AdminDashboard(){
            return View("~/Views/Home/AdminDashboard.cshtml");

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
