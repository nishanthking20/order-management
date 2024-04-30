using Microsoft.AspNetCore.Mvc;
using Order_Management.Models;
using Order_Management.Data;
using System.Diagnostics;

namespace Order_Management.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext DB;
        public int id;

        public DashboardController(ApplicationDbContext db_context)
        {
            DB = db_context;
        }

        public IActionResult Items()
        {
            List<Item> items = DB.Items.ToList<Item>();
            // foreach(Item item in items) {
            //     Console.WriteLine(item.ItemName);
            // }
            return View(items);
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
