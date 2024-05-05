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
        private int? userId;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("LoginUser", "Authentication");
            return RedirectToAction("Items");
        }

        public IActionResult History()
        {
            var histories = _context.History.ToList();
            return View(histories);
        }

        public IActionResult AddToHistory()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if(userId == null)
                return RedirectToAction("LoginUser", "Authentication");
            var transactionId = new Random().Next(100000, 999999);
            var serializedItems = HttpContext.Session.GetString("CartItems");
            var store = !string.IsNullOrEmpty(serializedItems)
                ? JsonSerializer.Deserialize<List<Item>>(serializedItems)
                : new List<Item>();
            var purchaseHistoryItems = new List<History>();
            foreach (var item in store!)
            {
                var purchaseHistoryItem = new History(
                    (int) userId!,
                    DateTime.Now,
                    (string) item.ItemName!,
                    item.Quantity,
                    item.Price,
                    transactionId
                );
                _context.History.Add(purchaseHistoryItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            userId = HttpContext.Session.GetInt32("userId");
            var serializedItems = HttpContext.Session.GetString("CartItems");
            var cartItems = !string.IsNullOrEmpty(serializedItems)
                ? JsonSerializer.Deserialize<List<Item>>(serializedItems)
                : new List<Item>();

            ViewData["User"] = userId;
            return View(cartItems);
        }

        public IActionResult Items()
        {
            var items = _context.Items.ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult Cart(long itemId, int quantity)
        {
            var item = _context.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (item != null)
            {
                var serializedItems = HttpContext.Session.GetString("CartItems");
                var cartItems = !string.IsNullOrEmpty(serializedItems)
                    ? JsonSerializer.Deserialize<List<Item>>(serializedItems)
                    : new List<Item>();

                var existingItem = cartItems!.FirstOrDefault(ci => ci.ItemId == itemId);

                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                }
                else
                {
                    item.Quantity = 1;
                    cartItems!.Add(item);
                }

                var updatedSerializedItems = JsonSerializer.Serialize(cartItems);
                HttpContext.Session.SetString("CartItems", updatedSerializedItems);

                TempData["CartItemAdded"] =
                    $"{quantity} {item.ItemName}(s) added to cart successfully.";
            }
            else
            {
                TempData["CartItemAdded"] = "Item not found.";
            }

            return RedirectToAction("Items");
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
