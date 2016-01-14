using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folio.Models;
using Folio.ViewModels;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;

namespace Folio.Builders
{
    public class StockBuilder
    {
        private ApplicationDbContext _context;
        public StockBuilder(ApplicationDbContext context)
        {
            _context = context;
        }

        public PortfolioDomainModel GetPortfolioDomainModel(int portfolioID)
        {
            Portfolio portfolio = _context.Portfolio
                .Include(p => p.PortfolioAssets)
                .Single(p => p.ID == portfolioID);
            IEnumerable<string> symbols = portfolio.PortfolioAssets.Select(a => a.AssetSymbol);
            List<Stock> stocks = _context.Stock
                .Where(s => symbols.Contains(s.Symbol)).ToList();
            List<StockDomainModel> stockDomainModels = new List<StockDomainModel>();
            Parallel.For(0, stocks.Count(), i =>
            {
                int amountOwned = portfolio.PortfolioAssets.Single(a => a.AssetSymbol == stocks[i].Symbol).NumberOfAssetOwned;
                stockDomainModels.Add(new StockDomainModel(stocks[i], amountOwned));
            });
            PortfolioDomainModel output = new PortfolioDomainModel(stockDomainModels);
            return output;
        }
    }
}
