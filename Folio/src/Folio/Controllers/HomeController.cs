using Microsoft.AspNet.Mvc;

namespace Folio.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Folio";

            return View();
        }

        public IActionResult News()
        {
            ViewData["Message"] = "Your news page.";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "[Folio] members in alphabetical order";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
