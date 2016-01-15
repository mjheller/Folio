using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Tests
{
    public class TestBetweenStocks : Models.StockDomainModel
    {
        public TestBetweenStocks(Models.Stock stock, int sharesOwned) : base(stock, sharesOwned) { }

        public decimal TestExpectedReturn(decimal beta)
        {
            decimal expectedRetrun = MathExpectedReturn(beta);
            return expectedRetrun;
        }
        public void testStockAdd(int sharesToAdd)
        {
            addShares(sharesToAdd);
        }

    }
    public class TestBetweenPortfolio : Models.PortfolioDomainModel
    {
        public TestBetweenPortfolio(List<Models.StockDomainModel> stocks, Models.Portfolio portfolio)
            : base(stocks, portfolio) { }
    }
}
