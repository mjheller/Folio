using Folio.Models;
using Microsoft.AspNet.Mvc;
using System.Xml;

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

        [HttpGet]
        public IActionResult ContactEmail(int? id)
        {
            Email email = new Email();
            email.ContactEmail = Services.Emails.GetContactEmailAddress(id);
            email.ContactName = Services.Emails.GetContactName(id);
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactEmail(Email email, int? id)
        {
            email.ContactEmail = Services.Emails.GetContactEmailAddress(id);
            email.ContactName = Services.Emails.GetContactName(id);
            Services.Emails.ProcessEmail(email.ContactEmail, email.CustomerEmail, email.EmailSubject, email.EmailBody);
            email.ErrorMessage = Services.Emails.errorMessage;
            return RedirectToAction("ProcessRequest", email);
        }
        [HttpGet]
        public IActionResult ProcessRequest(Email email)
        {
            return View(email);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
