using Folio.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YSQ.core.Historical;
using System.Linq;

namespace Folio.Models
{
    public class StockDomainModel
    {
        //private const decimal _sp500avgReturn = 6.34m;
        //private const decimal _riskFreeReturn = 2.70m;
        public string Name { get; private set; }
        public string Exchange { get; private set; }
        public DateTime LastUpdated { get; private set; }
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

        public StockDomainModel(string ticker, int sharesOwned)
        {
            Ticker = ticker;
            SharesOwned = sharesOwned;
            CurrentPrice = UpdateCurrentPrice();
            UpdateStockInformation();
        }

        public StockDomainModel(string ticker, string name, string exchange)
        {
            Ticker = ticker;
            Name = name;
            Exchange = exchange;
            CurrentPrice = UpdateCurrentPrice();
            //UpdateStockInformation();

        }

        private void UpdateStockInformation()
        {
            if ((DateTime.UtcNow - LastUpdated) > TimeSpan.FromDays(7))
            {
                ExpectedReturn = CalculateExpectedReturn();
                Variance = CalculateVariance();
                IEnumerable<HistoricalPrice> priceData = YahooAPICalls
                    .GetStockHistoricalPrices
                    (Ticker, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day), new DateTime(DateTime.UtcNow.Year));
                DailyReturns1Year = priceData;
                LastUpdated = DateTime.UtcNow;
            }
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
            int numYears = DateTime.UtcNow.Year - yearSearchLimit;
            decimal prob = numYears / 100m;
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
            decimal[] dailyRet = new decimal[prices.Length];
            double[] dailyReturnsDoubles = new double[dailyRet.Length];
            dailyRet[0] = prices[1];
            for (int i = 1; i < prices.Length; i++)
            {
                decimal dailyReturn = (prices[i] - prices[i - 1]) / prices[i - 1];
                dailyRet[i] = dailyReturn;
            }
            Parallel.For(0, dailyRet.Length, i => { dailyReturnsDoubles[i] = (double)dailyRet[i]; });
            return dailyReturnsDoubles;
        }
    }
}
