using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Order_Management.Data;
using Order_Management.Models;

namespace Order_Management.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Cart()
        {
            // Retrieve the serialized items from session
            var serializedItems = HttpContext.Session.GetString("CartItems");
            var itemsInCart = serializedItems != null ? JsonSerializer.Deserialize<List<Item>>(serializedItems) : new List<Item>();
            // Deserialize the JSON string to a list of items
            var cartItems = !string.IsNullOrEmpty(serializedItems)
                ? JsonSerializer.Deserialize<List<Item>>(serializedItems)
                : new List<Item>();

            return View(cartItems);
        }

        public IActionResult Items()
        {
            var items = _context.Items.ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult Cart(long? itemId)
        {
            // Find the item in the database based on its ID
            var item = _context.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (item != null)
            {
                // Retrieve existing cart items from session
                var serializedItems = HttpContext.Session.GetString("CartItems");
                var cartItems = !string.IsNullOrEmpty(serializedItems)
                    ? JsonSerializer.Deserialize<List<Item>>(serializedItems)
                    : new List<Item>();

                // Add the new item to the cart
                cartItems.Add(item);

                // Serialize the updated list of items to JSON and store it in session
                var updatedSerializedItems = JsonSerializer.Serialize(cartItems);
                HttpContext.Session.SetString("CartItems", updatedSerializedItems);

                TempData["CartItemAdded"] = $"{item.ItemName} added to cart successfully.";
            }
            else
            {
                TempData["CartItemAdded"] = "Item not found.";
            }

            // Redirect back to the items page
            return RedirectToAction("Items");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
