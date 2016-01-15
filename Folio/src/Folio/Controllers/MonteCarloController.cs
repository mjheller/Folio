using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Folio.ViewModels.MonteCarlo;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Folio.Controllers
{
    public class MonteCarloController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MonteCarloViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
