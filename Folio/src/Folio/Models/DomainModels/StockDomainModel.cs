using Folio.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;

namespace Folio.Models
{
    public class StockDomainModel
    {

        private const decimal _sp500avgReturn = 6.34m;
        private const decimal _riskFreeReturn = 2.70m;

        public double[] PriceHistory1Year;

        public string Ticker { get; set; }

        [DataType("Currency")]
        public decimal CurrentPrice { get; set; }

        [DataType("Currency")]
        public decimal PurchasePrice { get; set; }

        public int SharesOwned { get; set; }

        public decimal Worth
        { get { return CurrentPrice * SharesOwned; } }

        public decimal Weight { get; set; }
        public decimal ExpectedReturn { get; set; }
        public decimal Variance { get; set; }

        public StockDomainModel(string ticker, decimal purchasePrice, int sharesOwned)
        {
            Ticker = ticker;
            PurchasePrice = purchasePrice;
            SharesOwned = sharesOwned;
            CalculateExpectedReturn();
            CalculateVariance();
            decimal[] priceData = YahooAPICalls.GetStockHistoricalPricesToNow(ticker, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day)).ToArray();
            PriceHistory1Year = new double[priceData.Length];
            Parallel.For(0, priceData.Length, i => { PriceHistory1Year[i] = (double)priceData[i]; });
        }

        private void addShares(int amount)
        {
            SharesOwned += amount;
        }

        private void CalculateVariance()
        {
            int year;
            decimal sumSquared = 0;
            int numYears = DateTime.UtcNow.Year - 2006;
            decimal prob = numYears / 100;
            for (int i = 0; i < numYears; i++)
            {
                if (i < 10)
                { year = Convert.ToInt32(string.Format("200{0}", i.ToString())); } else
                { year = Convert.ToInt32(string.Format("20{0}", i.ToString())); }
                List<decimal> prices = YahooAPICalls.GetStockHistoricalPricesCustom(Ticker, new DateTime(year, 1, 1), new DateTime(year, 12, 31));
                decimal annualReturn = (prices[prices.Count - 1] / prices[0]) - 1;
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(ExpectedReturn - annualReturn), 2));
                sumSquared += squared;
            }
            Variance = sumSquared / numYears;
        }

        private void CalculateExpectedReturn()
        {
            decimal beta = YahooAPICalls.GetStockBeta(Ticker);
            decimal marketRiskPremium = _sp500avgReturn - _riskFreeReturn;
            decimal riskPremium = beta * marketRiskPremium;
            ExpectedReturn = _riskFreeReturn + riskPremium;
        }
    }

}
