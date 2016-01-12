using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folio.Models.MattsModels;
using Folio.Models;
using Folio.ViewModels;

namespace Folio.Builders
{
    public class StockBuilder
    {
        private ApplicationDbContext _context;
        public StockBuilder(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Stock> GetStocksForPortolio(int portfolioID)
        {
            List<PortfolioAsset> portfolioAssets = _context.Portfolio
            .Single(p => p.ID == portfolioID)
            .PortfolioAssets.Where(pa => pa.AssetType == "stock")
            .ToList();
            List<Stock> stocks = new List<Stock>();
            foreach (PortfolioAsset asset in portfolioAssets)
            {
                stocks.Add(new Stock(asset.AssetSymbol, asset.AveragePurchasePrice, asset.NumberOfAsset));
            }
            return stocks;
        }

        public List<PortfolioViewModel> GetPortfolioViewModels(ApplicationUser User)
        {
            List<Portfolio> portfolios = _context.Portfolio.Where(p => p.User == User).ToList();;
        }
    }
}
