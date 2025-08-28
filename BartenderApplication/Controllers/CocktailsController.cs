using BartenderApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BartenderApp.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly AppDbContext _db;
        public CocktailsController(AppDbContext db) => _db = db;


        // GET: /Cocktails/Menu
        public async Task<IActionResult> Menu()
        {
            var items = await _db.Cocktails.AsNoTracking().ToListAsync();
            return View(items);
        }
    }
}