using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Tests
{
    public class TestBetween : Models.StockDomainModel
    {
        public TestBetween(Models.Stock stock, int sharesOwned) : base(stock, sharesOwned) { }

        public decimal TestExpectedReturn(decimal beta)
        {
            decimal expectedRetrun = MathExpectedReturn(beta);
            return expectedRetrun;
        }

    }
}
