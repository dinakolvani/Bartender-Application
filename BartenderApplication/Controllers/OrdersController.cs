using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// 👇 use your actual namespaces here
using BartenderApp.Data;
using BartenderApp.Models;

namespace BartenderApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db) => _db = db;

        // GET: /Orders/Create?cocktailId=1
        public async Task<IActionResult> Create(int cocktailId)
        {
            var cocktail = await _db.Cocktails.FindAsync(cocktailId);
            if (cocktail == null) return NotFound();

            ViewBag.Cocktail = cocktail;
            return View(new Order { CocktailId = cocktailId });
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cocktail = await _db.Cocktails.FindAsync(order.CocktailId);
                return View(order);
            }

            order.OrderedAt = System.DateTime.UtcNow;
            order.Status = OrderStatus.Placed;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            TempData["msg"] = $"Order #{order.Id} placed!";
            return RedirectToAction("Menu", "Cocktails");
        }

        // GET: /Orders/Queue (bartender view)
        public async Task<IActionResult> Queue()
        {
            var queue = await _db.Orders
                .Include(o => o.Cocktail)
                .Where(o => o.Status == OrderStatus.Placed ||
                            o.Status == OrderStatus.InPreparation)
                .OrderBy(o => o.OrderedAt)
                .ToListAsync();

            return View(queue);
        }
       
    }
}
