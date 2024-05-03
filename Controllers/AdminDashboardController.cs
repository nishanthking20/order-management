using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Order_Management.Data;
using Order_Management.Models;

namespace Order_Management.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext DB;
        public int id;

        public AdminDashboardController(ApplicationDbContext db_context)
        {
            DB = db_context;
        }
        public IActionResult AddCustomer(){
            return View();
        }
        public IActionResult AddItem(){
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(Item item)
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
            return RedirectToAction("AddItem");
        }
    }
}