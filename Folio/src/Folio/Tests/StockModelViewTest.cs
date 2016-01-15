using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Folio.Tests
{
    public class StockModelViewTest
    {
        [Fact]
        void TestExpectedRetrun()
        {
            Models.Stock stock = new Models.Stock();
            stock.Name = "google";
            stock.Symbol = "googl";
            TestBetween tb = new TestBetween(stock, 10);
            decimal expecteReturn = tb.TestExpectedReturn(2);
            Assert.Equal(expecteReturn, 9.98m);
        }
    }
}
