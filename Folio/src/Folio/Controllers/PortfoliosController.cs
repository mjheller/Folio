using folio.Services;
using Folio.Builders;
using Folio.Models;
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

namespace Folio.Controllers
{
    [Authorize]
    public class PortfoliosController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager; 

        public PortfoliosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Portfolios
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            IEnumerable<Portfolio> portfolios = _context.Portfolio.Where(p => p.User.Id == user.Id).Include(s => s.PortfolioAssets);
            return View(portfolios);
        }

        // GET: Portfolios/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            PortfolioViewModel portfolioViewModel = HttpContext.Session.GetObjectFromJson<PortfolioViewModel>("selected_port_viewmodel");
            if (id == null)
            {
                return HttpNotFound();
            }
            if ((portfolioViewModel == null) || (portfolioViewModel.ID != id))
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
            if (portfolioViewModel.Stocks.Count() < 1)
            {
                return RedirectToAction("AddStock", new { id = id });
            }
            return View(portfolioViewModel);
        }

        // GET: Portfolios/Stocks/5
        [HttpGet]
        public ActionResult Stocks(int? id)
        {
            Portfolio portfolio = _context.Portfolio.Include(p => p.PortfolioAssets).Single(p => p.ID == id);
            List<string> tickers = portfolio.PortfolioAssets.Select(p => p.AssetSymbol).ToList();
            string m_symbol = string.Join(" ", tickers.ToArray());
            Stocks model;
            StockQuotesBuilder dataModel = new StockQuotesBuilder();

            if (m_symbol == "")
                // Set the default stock symbol to YHOO.
                m_symbol = @"YHOO";

            model = dataModel.createQuotes(m_symbol);

            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Stocks", new { id = id });
            }
        }

        // GET: Portfolios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Portfolios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
                portfolio.User = user;
                portfolio.DateCreated = DateTime.Now;
                _context.Portfolio.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddStock", portfolio);
            }

            return View();
        }

        // GET: Portfolios/AddStock/5
        [HttpGet]
        public async Task<IActionResult> AddStock(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            List<Portfolio> userPortfolios = _context.Portfolio.Where(p => p.User.Id == HttpContext.User.GetUserId()).Include(s => s.PortfolioAssets).ToList();
            Portfolio workingPortfolio = userPortfolios.Find(p => p.ID == id);
            userPortfolios.Remove(userPortfolios.Find(p => p.Name == workingPortfolio.Name));

            if (HttpContext.Session.GetObjectFromJson<List<string>>("Tickers") == null)
            {
                List<Stock> stock = await _context.Stock.ToListAsync();
                List<string> tickers = stock.Select(s => s.Symbol).ToList();
                HttpContext.Session.SetObjectAsJson("Tickers", tickers);
            }

            AddStockToPortfolioViewModel model = new AddStockToPortfolioViewModel
            {
                UserPortfolios = userPortfolios,
                WorkingPortfolio = workingPortfolio,
                AvailableAssetTickers = HttpContext.Session.GetObjectFromJson<List<string>>("Tickers")
            };

            return View(model);
        }

        // POST: Portfolios/AddStock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(int? id, string tickerinput, string amount)
        {
            Portfolio portfolio = await _context.Portfolio.Include(p => p.PortfolioAssets).SingleAsync(p => p.ID == id);
            List<string> heldTickers = portfolio.PortfolioAssets.Select(p => p.AssetSymbol).ToList();
            PortfolioAsset newAsset = new PortfolioAsset { AssetSymbol = tickerinput.ToUpper(), NumberOfAssetOwned = Int32.Parse(amount) };

            if (portfolio.PortfolioAssets.Count == 0)
            {
                _context.PortfolioAsset.Add(newAsset);
                portfolio.PortfolioAssets = new List<PortfolioAsset>() { newAsset };
            } else if (!heldTickers.Contains(tickerinput))
            {
                portfolio.PortfolioAssets.Add(newAsset);
                _context.Update(portfolio);
            }
            else
            {
                PortfolioAsset asset = portfolio.PortfolioAssets.Single(p => p.AssetSymbol == tickerinput);
                asset.NumberOfAssetOwned += int.Parse(amount);
                _context.Update(asset);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("selected_port_viewmodel");
            return RedirectToAction("AddStock", new { id = id } );
        }

        // Portfolios/DeleteStock/5
        [HttpGet]
        public async Task<IActionResult> DeleteStock(int? id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            List<Portfolio> portfolios = _context.Portfolio.Include(a => a.PortfolioAssets).Where(p => p.User.Id == user.Id).ToList();
            Portfolio workingPortfolio = portfolios.Find(p => p.ID == id);
            portfolios.Remove(workingPortfolio);

            var model = new DeleteStockFromPortfolioViewModel { WorkingPortfolio = workingPortfolio, UserPortfolios = portfolios };
            return View(model);
        }

        // Portfolios/DeleteStock/5
        [HttpPost]
        public async Task<IActionResult> DeleteStock(int? id, string stockTicker, string amountRemove)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
            List<Portfolio> portfolios = _context.Portfolio.Where(p => p.User.Id == user.Id).Include(p => p.PortfolioAssets).ToList();
            Portfolio workingPortfolio = portfolios.Single(p => p.ID == id);
            portfolios.Remove(workingPortfolio);

            var model = new DeleteStockFromPortfolioViewModel { WorkingPortfolio = workingPortfolio, UserPortfolios = portfolios };

            PortfolioAsset asset = _context.PortfolioAsset.Single(p => p.PortfolioID == id && p.AssetSymbol == stockTicker);

            if (asset.NumberOfAssetOwned < int.Parse(amountRemove))
            {
                asset.NumberOfAssetOwned = 0;
            } else
            {
                asset.NumberOfAssetOwned -= int.Parse(amountRemove);
            }

            if (asset.NumberOfAssetOwned == 0)
            {
                _context.PortfolioAsset.Remove(asset);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Update(asset);
                await _context.SaveChangesAsync();
            }
            
            HttpContext.Session.Remove("selected_port_viewmodel");
            return View(model);
        }

        // GET: Portfolios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Portfolio portfolio = await _context.Portfolio.SingleAsync(m => m.ID == id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // POST: Portfolios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                _context.Update(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            HttpContext.Session.Remove("selected_port_viewmodel");
            return View(portfolio);
        }

        // GET: Portfolios/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Portfolio portfolio = await _context.Portfolio.SingleAsync(m => m.ID == id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }

            return View(portfolio);
        }

        // POST: Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Portfolio portfolio = await _context.Portfolio.Include(p => p.PortfolioAssets).SingleAsync(m => m.ID == id);
            
            foreach(PortfolioAsset asset in portfolio.PortfolioAssets)
            {
                PortfolioAsset assetToRemove = _context.PortfolioAsset.Single(p => p.ID == asset.ID);
                _context.PortfolioAsset.Remove(assetToRemove);
            }

            _context.Portfolio.Remove(portfolio);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("selected_port_viewmodel");
            return RedirectToAction("Index");
        }

        public JsonResult Autocomplete(string term)
        {
            List<string> items = HttpContext.Session.GetObjectFromJson<List<string>>("Tickers");
            var filteredItems = items.Where(item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0);
            return Json(filteredItems);
        }
    }
}