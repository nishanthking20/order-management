using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Order_Management.Data;
using Order_Management.Models;

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

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminDashboard(Item item)
        {
            var existingItem = DB.Items.FirstOrDefault(u => u.ItemName == item.ItemName);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                TempData["itemAlertMessage"] = "Item SuccessFully Modified";
            }
            else
            {
                TempData["itemAlertMessage"] = "Item SuccessFully added";
                DB.Items.Add(item);
            }
            DB.SaveChanges();
            return View();
        }

        public IActionResult Items()
        {
            List<Item> items = DB.Items.ToList<Item>();
            // foreach(Item item in items) {
            //     Console.WriteLine(item.ItemName);
            // }
            return View(items);
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
