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
        public double[] PriceHistory1Year { get; set; }
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
            PriceHistory1Year = UpdatePriceHistory1Year();
            _lastUpdated = DateTime.UtcNow;
        }

        private decimal UpdateCurrentPrice()
        {
            return YahooAPICalls.GetCurrentStockPrice(Ticker);
        }

        private double[] UpdatePriceHistory1Year()
        {
            decimal[] priceData = YahooAPICalls.GetStockHistoricalPrices(Ticker, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day), DateTime.UtcNow).ToArray();
            double[] priceHistory1Year = new double[priceData.Length];
            Parallel.For(0, priceData.Length, i => { PriceHistory1Year[i] = Convert.ToDouble(priceData[i]); });
            return priceHistory1Year;
        }

        private void addShares(int amount)
        {
            SharesOwned += amount;
        }

        private decimal CalculateVariance()
        {
            int year;
            decimal sumSquared = 0;
            const int yearSearchLimit = 2006;
            int numYears = DateTime.UtcNow.Year - yearSearchLimit;
            decimal prob = numYears / 100;
            for (int i = 0; i < numYears; i++)
            {
                if (i < 10)
                {
                    year = Convert.ToInt32($"200{i}");
                }
                else
                {
                    year = Convert.ToInt32($"20{i}");
                }
                List<decimal> prices = YahooAPICalls.GetStockHistoricalPrices(Ticker, new DateTime(year, 1, 1), new DateTime(year, 12, 31));
                decimal annualReturn = (prices[prices.Count - 1] / prices[0]) - 1;
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(ExpectedReturn - annualReturn), 2));
                sumSquared += squared;
            }
            return sumSquared / numYears;
        }

        private decimal CalculateExpectedReturn()
        {
            decimal beta = YahooAPICalls.GetStockBeta(Ticker);
            decimal marketRiskPremium = _sp500avgReturn - _riskFreeReturn;
            decimal riskPremium = beta * marketRiskPremium;
            return _riskFreeReturn + riskPremium;
        }
    }
}
