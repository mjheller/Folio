using folio.Services;
using Folio.Models;
using Folio.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Folio.ViewModels.MonteCarlo;
using Folio.Services.MonteCarlo;
using Folio.Builders;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Folio.Controllers
{
    [Authorize]
    public class MonteCarloController : Controller
    {
        private ApplicationDbContext _context;

        public MonteCarloController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            PortfolioViewModel portfolioViewModel = HttpContext.Session.GetObjectFromJson<PortfolioViewModel>("selected_port_viewmodel");
            if (portfolioViewModel == null)
            {
                Portfolio portfolio = _context.Portfolio.Include(p => p.PortfolioAssets).Single(m => m.ID == id);
                if ((portfolio == null) || (portfolioViewModel.ID != id))
                {
                    return HttpNotFound();
                }
                Builder builder = new Builder(_context);
                PortfolioDomainModel portfolioDomainModel = builder.GetPortfolioDomainModel(portfolio);
                portfolioViewModel = builder.GetPortfolioViewModel(portfolioDomainModel);
                HttpContext.Session.SetObjectAsJson("selected_port_viewmodel", portfolioViewModel);
            }
            MonteCarloViewModel blankMonte = new MonteCarloViewModel();
           // blankMonte.PortfolioViewModel = portfolioViewModel;
            return View(blankMonte);
        }

        [HttpPost]
        public IActionResult Index(MonteCarloViewModel fullMonte)
        {
            fullMonte.PortfolioViewModel = HttpContext.Session.GetObjectFromJson<PortfolioViewModel>("selected_port_viewmodel");

            bool inputCheck = true;
            if (string.IsNullOrWhiteSpace(fullMonte.AnnualContribution.ToString()))
            {
                inputCheck = false;
            }
            if (string.IsNullOrWhiteSpace(fullMonte.PreferredRetirementAge.ToString()))
            {
                inputCheck = false;
            }
            if (string.IsNullOrWhiteSpace(fullMonte.EstimatedRetirementSpan.ToString()))
            {
                inputCheck = false;
            }
            if (string.IsNullOrWhiteSpace(fullMonte.AnnualRetirementIncomeDraw.ToString()))
            {
                inputCheck = false;
            }

            if (inputCheck == false)
            {
                return View(fullMonte.PortfolioViewModel.ID); 
            }

            ApplicationUser currentUser = _context.Users.Single(u => u.Id == HttpContext.User.GetUserId());
            fullMonte.StartingAge = (DateTime.Now.Year - currentUser.DateOfBirth.Year);
            int steps = (fullMonte.PreferredRetirementAge - fullMonte.StartingAge) + fullMonte.EstimatedRetirementSpan;
            PortfolioPath[] monteCarloPathResults = MonteCarloProcessor.CalculateMonteCarlo(fullMonte, currentUser);
            List<decimal> monteCarloAvgResults = PathAnalysis.GetAveragePath(monteCarloPathResults, steps);
            List<decimal> monteCarloMaxResults = PathAnalysis.GetMaximumPath(monteCarloPathResults, steps);
            List<decimal> monteCarloMinResults = PathAnalysis.GetMinimumPath(monteCarloPathResults, steps);
            List<string> ageList = new List<string>();
            for (int i = fullMonte.StartingAge; i < fullMonte.PreferredRetirementAge + fullMonte.EstimatedRetirementSpan; i++)
            {
                ageList.Add(i.ToString());
            }

            fullMonte.MonteCarloAvgResults = monteCarloAvgResults;
            fullMonte.MonteCarloMaxResults = monteCarloMaxResults;
            fullMonte.MonteCarloMinResults = monteCarloMinResults;
            fullMonte.ageSpan = ageList;

            return View(fullMonte);
        }
    }
}
