using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Order_Management.Models;

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
            return RedirectToAction("LoginUser", "Authentication");
        }

        public IActionResult Register()
        {
            // Console.WriteLine(id);
            return RedirectToAction("RegisterUser", "Authentication");
        }

        public IActionResult Dashboard()
        {
            // Console.WriteLine(id);
            return RedirectToAction("Dashboard", "Dashboard");
        }

        public IActionResult AdminDashboard()
        {
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
