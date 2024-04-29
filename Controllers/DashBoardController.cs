using Microsoft.AspNetCore.Mvc;
using Order_Management.Models;
using System.Diagnostics;

namespace Order_Management.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public int id;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Items()
        {
            return View();
        }
        public IActionResult History(){
            return View();
        }
        public IActionResult Cart(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
