using Folio.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class StockDomainModel
    {
        private const decimal _sp500avgReturn = 6.34m;
        private const decimal _riskFreeReturn = 2.70m;
        public string Name { get; private set; }
        public string Exchange { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public double[] DailyReturns1Year { get; private set; }
        public string Ticker { get; private set; }
        [DataType("Currency")]
        public decimal CurrentPrice { get; set; }
        [DataType("Currency")]
       // public decimal PurchasePrice { get; private set; }
        public int SharesOwned { get; private set; }
        public decimal Worth
        {
            get { return CurrentPrice * SharesOwned; }
        }
        public decimal Weight { get; set; }
        public decimal ExpectedReturn { get; private set; }
        public decimal Variance { get; private set; }
        public string GetDailyReturns1YearAsJSON
        {
            get { return JsonConvert.SerializeObject(DailyReturns1Year); }
        }

        public StockDomainModel(string ticker, int sharesOwned)
        {
            Ticker = ticker;
           // PurchasePrice = purchasePrice;
            SharesOwned = sharesOwned;
            UpdateStockInformation();
        }

        public StockDomainModel(string ticker, string name, string exchange)
        {
            Ticker = ticker;
            Name = name;
            Exchange = exchange;
            UpdateStockInformation();
        }

       


        private void UpdateStockInformation()
        {
            CurrentPrice = UpdateCurrentPrice();
            if ((DateTime.UtcNow - LastUpdated) > TimeSpan.FromDays(7))
            {
                ExpectedReturn = CalculateExpectedReturn();
                Variance = CalculateVariance();
                decimal[] priceData = YahooAPICalls
                    .GetStockHistoricalPrices
                    (Ticker, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day), new DateTime(DateTime.UtcNow.Year))
                    .ToArray();
                DailyReturns1Year = CalculateDailyReturnsToArray(priceData);
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
            const int yearSearchLimit = 2006;
            int numYears = DateTime.UtcNow.Year - yearSearchLimit;
            decimal prob = numYears / 100;

            for (int i = yearSearchLimit; i < DateTime.UtcNow.Year; i++)
            {
                List<decimal> prices = YahooAPICalls.GetStockHistoricalPrices(Ticker, new DateTime(i, 1, 1), new DateTime(i, 12, 31));
                decimal annualReturn = ((prices[prices.Count - 1] - prices[0]) / prices[0]);
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(ExpectedReturn - annualReturn), 2))*prob;
                sumSquared += squared;
            }
            return sumSquared / numYears;
        }

        private decimal CalculateExpectedReturn()
        {
            decimal beta = YahooAPICalls.GetStockBeta(Ticker);
            decimal marketRiskPremium = _sp500avgReturn - _riskFreeReturn;
            decimal riskPremium = beta * marketRiskPremium;
            return (_riskFreeReturn + riskPremium)/100;
        }

        private double[] CalculateDailyReturnsToArray(decimal[] prices)
        {
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
