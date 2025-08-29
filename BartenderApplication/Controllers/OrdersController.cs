using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            order.Status = OrdersStatus.Placed;

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
                .Where(o => o.Status == OrdersStatus.Placed ||
                            o.Status == OrdersStatus.InProgress)
                            .OrderBy(o => o.Status == OrdersStatus.Completed)
                            .Where(o => o.Status != OrdersStatus.Canceled)
                .OrderBy(o => o.OrderedAt)
                .ToListAsync();

            return View(queue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = OrdersStatus.Canceled;
            await _db.SaveChangesAsync();

            TempData["msg"] = $"Order #{order.Id} canceled.";
            return RedirectToAction(nameof(Queue));
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Advance(int id)
{
    var order = await _db.Orders.FindAsync(id);
    if (order == null) return NotFound();

    // Move the order forward one step
    switch (order.Status)
    {
        case OrdersStatus.Placed:
            order.Status = OrdersStatus.InProgress;
            break;

        case OrdersStatus.InProgress:
            order.Status = OrdersStatus.Completed;
            break;

        // Completed or Canceled stay as-is
        default:
            break;
    }

    await _db.SaveChangesAsync();
    TempData["msg"] = $"Order #{order.Id} → {order.Status}";
    return RedirectToAction(nameof(Queue));
}
       
    }
}
