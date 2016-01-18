using Folio.Models;
using Folio.ViewModels;
using Microsoft.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Builders
{
    public class Builder
    {
        private ApplicationDbContext _context;
        public Builder(ApplicationDbContext context)
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
            PortfolioDomainModel output = new PortfolioDomainModel(stockDomainModels, portfolio);
            return output;
        }

        public PortfolioDomainModel GetPortfolioDomainModel(Portfolio portfolio)
        {
            IEnumerable<string> symbols = portfolio.PortfolioAssets.Select(a => a.AssetSymbol);
            List<Stock> stocks = _context.Stock
                .Where(s => symbols.Contains(s.Symbol)).ToList();
            List<StockDomainModel> stockDomainModels = new List<StockDomainModel>();
            Parallel.For(0, stocks.Count(), i =>
            {
                int amountOwned = portfolio.PortfolioAssets.Single(a => a.AssetSymbol == stocks[i].Symbol).NumberOfAssetOwned;
                stockDomainModels.Add(new StockDomainModel(stocks[i], amountOwned));
            });
            PortfolioDomainModel output = new PortfolioDomainModel(stockDomainModels, portfolio);
            return output;
        }

        private IEnumerable<StockViewModel> GetStockViewModels(List<StockDomainModel> stocks)
        {
            List<StockViewModel> stockViewModels = new List<StockViewModel>();
            Parallel.For(0, stocks.Count(), i =>
            {
                stockViewModels.Add(new StockViewModel {
                    Ticker = stocks[i].Ticker,
                    Name = stocks[i].Name,
                    Exchange = stocks[i].Exchange,
                    Worth = stocks[i].Worth,
                    DailyReturns1Year = stocks[i].DailyReturns1Year,
                    SharesOwned = stocks[i].SharesOwned,
                    ExpectedReturn = stocks[i].ExpectedReturn,
                    Variance = stocks[i].Variance,
                    CurrentPrice= stocks[i].CurrentPrice
                });
            });
            return stockViewModels;
        }

        public PortfolioViewModel GetPortfolioViewModel(PortfolioDomainModel portfolioDomainModel)
        {
            string expectedReturn;
            if (((portfolioDomainModel.ExpectedReturn)*100M).ToString().Length > 3)
            {
                expectedReturn = ((portfolioDomainModel.ExpectedReturn)*100M).ToString().Substring(0, 4);
            }
            else
            {
                expectedReturn = ((portfolioDomainModel.ExpectedReturn)*100M).ToString();
            }

            PortfolioViewModel portfolioViewModel = new PortfolioViewModel {
                ID = portfolioDomainModel.ID,
                Name = portfolioDomainModel.Name,
                DateCreated = portfolioDomainModel.DateCreated,
                ExpectedReturnString = string.Format("{0}%", expectedReturn),
                ExpectedReturnDouble = (double)portfolioDomainModel.ExpectedReturn,
                Variance = portfolioDomainModel.Variance,
                DollarValue = portfolioDomainModel.DollarValue
            };
            portfolioViewModel.Stocks = (GetStockViewModels(portfolioDomainModel.Stocks));
            return portfolioViewModel;
        }
    }
}
