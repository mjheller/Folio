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

        public IActionResult News(int? id)
        {
            ViewData["XMLNodeList"] = Services.RSSFeed.GetXMLNodeList(Services.RSSFeed.GetRSSHttp(id));
            ViewData["RSSTitle"] = Services.RSSFeed.GetTitleName(id);
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
