using folio.Services;
using Folio.Builders;
using Folio.Models;
using Folio.Models.Data_Models;
using Folio.ViewModels;
using Folio.ViewModels.Portfolios;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Folio.Controllers
{
    [Authorize]
    public class MonteCarloController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager; 

        public MonteCarloController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: /<controller>/
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            PortfolioViewModel portfolioViewModel = HttpContext.Session.GetObjectFromJson<PortfolioViewModel>("selected_port_viewmodel");
            if (portfolioViewModel == null)
            {
                Portfolio portfolio = await _context.Portfolio.Include(p => p.PortfolioAssets).SingleAsync(m => m.ID == id);
                if (portfolio == null)
                {
                    return HttpNotFound();
                }
                Builder builder = new Builder(_context);
                PortfolioDomainModel portfolioDomainModel = builder.GetPortfolioDomainModel(portfolio);
                portfolioViewModel = builder.GetPortfolioViewModel(portfolioDomainModel);
                HttpContext.Session.SetObjectAsJson("selected_port_viewmodel", portfolioViewModel);
            }
            return View();
        }
    }
}
