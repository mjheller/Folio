using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;

namespace Folio.ViewModels
{
    public class StockViewModel
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public decimal Worth { get; set; }
        public IEnumerable<HistoricalPrice> DailyReturns1Year { get; set; }
        public int SharesOwned { get; set; }
        public decimal ExpectedReturn { get; set; }
        public decimal Variance { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
