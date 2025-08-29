using Microsoft.AspNetCore.Mvc;


namespace BartenderApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();
    }
}