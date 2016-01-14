using Folio.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YSQ.core.Historical;
using System.Linq;

namespace Folio.Models
{
    public class StockDomainModel
    {
        public string Name { get; private set; }
        public string Exchange { get; private set; }
        //public DateTime LastUpdated { get; private set; }
        public IEnumerable<HistoricalPrice> DailyReturns1Year { get; private set; }
        public string Ticker { get; private set; }
        [DataType("Currency")]
        public decimal CurrentPrice { get; set; }
        [DataType("Currency")]
        public int SharesOwned { get; private set; }
        public decimal Worth
        {
            get { return CurrentPrice * SharesOwned; }
        }
        public decimal Weight { get; set; }
        public decimal ExpectedReturn { get; private set; }
        public decimal Variance { get; private set; }
        public string DailyReturns1YearAsJSON
        {
            get { return JsonConvert.SerializeObject(DailyReturns1Year); }
            set { DailyReturns1Year = JsonConvert.DeserializeObject<IEnumerable<HistoricalPrice>>(value); }
        }

        public StockDomainModel(Stock stock, int sharesOwned)
        {
            Ticker = stock.Symbol;
            Name = stock.Name;
            Exchange = stock.Exchange;
            SharesOwned = sharesOwned;
            UpdateStockInformation();
        }

        private void UpdateStockInformation()
        {
            CurrentPrice = UpdateCurrentPrice();
            ExpectedReturn = CalculateExpectedReturn();
            Variance = CalculateVariance();
            UpdateDailyReturns1Year();
           // LastUpdated = DateTime.UtcNow;
        }

        private void UpdateDailyReturns1Year()
        {
            IEnumerable<HistoricalPrice> priceData = YahooAPICalls
                    .GetStockHistoricalPrices
                    (Ticker, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            DailyReturns1Year = priceData;
        }

        private decimal UpdateCurrentPrice()
        {
            return YahooAPICalls.GetCurrentStockPrice(Ticker);
        }

        private void addShares(int amount)
        {
            SharesOwned += amount;
        }

        private decimal CalculateVariance()
        {
            decimal sumSquared = 0;
            int yearSearchLimit = 2006;
            List<decimal> annualReturns = new List<decimal>();
            for (int i = yearSearchLimit; i < DateTime.UtcNow.Year; i++)
            {
                IEnumerable<HistoricalPrice> prices = YahooAPICalls.GetStockHistoricalPrices(Ticker, new DateTime(i, 1, 1), new DateTime(i, 12, 31));
                if (prices == null)
                {
                    yearSearchLimit += 1;
                    continue;
                }
                decimal yearStart = prices
                    .Single(p => p.Date == prices
                    .Select(pp => pp.Date).Min()).Price;
                decimal yearEnd = prices
                    .Single(p => p.Date == prices
                    .Select(pp => pp.Date).Max()).Price;
                decimal annualReturn = (yearEnd - yearStart) / yearStart;
                annualReturns.Add(annualReturn);
            }
            int numYears = DateTime.Now.Year - yearSearchLimit;
            decimal prob = numYears / 100m;
            foreach (decimal annualReturn in annualReturns)
            {
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(ExpectedReturn - annualReturn), 2))*prob;
                sumSquared += squared;
            }
            return sumSquared / numYears;
        }

        private decimal CalculateExpectedReturn()
        {
            const decimal _sp500avgReturn = 6.34m;
            const decimal _riskFreeReturn = 2.70m;
            decimal beta = YahooAPICalls.GetStockBeta(Ticker);
            decimal marketRiskPremium = _sp500avgReturn -_riskFreeReturn;
            decimal riskPremium = beta * marketRiskPremium;
            return (_riskFreeReturn + riskPremium)/100;
        }

        public double[] DailyReturns1YearAsArray()
        {
            decimal[] prices = DailyReturns1Year.Select(p => p.Price).ToArray();
            double[] dailyReturnsDoubles = new double[prices.Length];
            dailyReturnsDoubles[0] = Convert.ToDouble(prices[1]);
            Parallel.For(1, prices.Length, i => {
                double dailyReturn = (Math.Log((double)prices[i]) - Math.Log((double)prices[i - 1]) / Math.Log((double)prices[i - 1]));
                dailyReturnsDoubles[i] = dailyReturn;
            });
            return dailyReturnsDoubles;
        }
    }
}
