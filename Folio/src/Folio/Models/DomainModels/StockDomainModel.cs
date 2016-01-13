using Folio.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Folio.Models
{
    public class StockDomainModel
    {
        private const decimal _sp500avgReturn = 6.34m;
        private const decimal _riskFreeReturn = 2.70m;
        private DateTime _lastUpdated;
        //public double[] PriceHistory1Year { get; set; }
        public double[] dailyReturns1Year;
        public string Ticker { get; private set; }
        [DataType("Currency")]
        public decimal CurrentPrice { get; set; }
        [DataType("Currency")]
        public decimal PurchasePrice { get; private set; }
        public int SharesOwned { get; private set; }
        public decimal Worth
        {
            get { return CurrentPrice * SharesOwned; }
        }
        public decimal Weight { get; set; }
        public decimal ExpectedReturn { get; private set; }
        public decimal Variance { get; private set; }

        public StockDomainModel(string ticker, decimal purchasePrice, int sharesOwned)
        {
            Ticker = ticker;
            PurchasePrice = purchasePrice;
            SharesOwned = sharesOwned;
            UpdateStockInformation();
            
        }

        private string GetPriceHistory1YearAsJSON()
        {
            return JsonConvert.SerializeObject(PriceHistory1Year);
        }

        private void UpdateStockInformation()
        {
            CurrentPrice = UpdateCurrentPrice();
            ExpectedReturn = CalculateExpectedReturn();
            Variance = CalculateVariance();
            dailyReturns1Year = CalculateDailyReturnsToArray(priceData);
            _lastUpdated = DateTime.UtcNow;
        }

        private decimal UpdateCurrentPrice()
        {
            return YahooAPICalls.GetCurrentStockPrice(Ticker);
        }

        //private double[] UpdatePriceHistory1Year()
        //{
        //    decimal[] priceData = YahooAPICalls.GetStockHistoricalPrices(Ticker, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day), DateTime.UtcNow).ToArray();
        //    double[] priceHistory1Year = new double[priceData.Length];
        //    Parallel.For(0, priceData.Length, i => { PriceHistory1Year[i] = Convert.ToDouble(priceData[i]); });
        //    return priceHistory1Year;
        //}

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
            for (int i = yearSeachLimit; i < DateTime.UtcNow.Year; i++)
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

            //List<decimal> dailyRet = new List<decimal>();
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
