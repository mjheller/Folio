using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;

namespace Folio.Models.MattsModels
{
<<<<<<< HEAD
    //Asset Class
    public class Asset
    {
        protected string Name;
        protected decimal purchasePrice, currentPrice;   
    }

=======
>>>>>>> a1754c17488be482293b9d8896f9d69c4e743a6a
    //Stock Class
    public class Stock : Asset
    {

        private string symbol;
        private int sharesOwned;
        private decimal weight;
        private decimal sp500avgReturn = (decimal)6.34;
        private decimal riskFreeReturn = (decimal)2.70;
        private decimal expectedReturn;
        private decimal variance;
<<<<<<< HEAD
        public decimal CurrentPrice { get { return currentPrice; } set { currentPrice = value; } }
        //use StockHelper.getCurrentPrice()
        public string Symbol { get { return symbol; } }
        public decimal PurchasePrice { get { return purchasePrice; } }
=======
        public double[] priceHistory1Year;




        public string Symbol { get { return symbol; } }
        [DataType("Currency")]
        public decimal CurrentPrice { get { return currentPrice; }  }
        [DataType("Currency")]
        public decimal PurchasePrice { get { return purchasePrice; } set; }

>>>>>>> a1754c17488be482293b9d8896f9d69c4e743a6a
        public int SharesOwned { get { return sharesOwned; } }
        public decimal Worth { get { return CurrentPrice * SharesOwned; } }
        public decimal Weight { get { return weight; } set { weight = value; } }
        public decimal ExpectedReturn { get { return expectedReturn; } set { expectedReturn = value; } }
        public decimal Variance { get { return variance; } set { variance = value; } }

        public Stock(string symbol, decimal purchasePrice, int sharesOwned)
        {
            this.symbol = symbol;
            this.PurchasePrice = CurrentPrice = purchasePrice;
            this.sharesOwned = sharesOwned;
            CalculateExpectedReturn();
            CalculateVariance();
            decimal[] priceData = StockHelper.GetHistoricalPricesToNow(symbol, new DateTime(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Month, DateTime.UtcNow.Day)).ToArray();
            priceHistory1Year = new double[priceData.Length];
            Parallel.For(0, priceData.Length, i => { priceHistory1Year[i] = (double)priceData[i]; });
        }

        private void addShares(int amount)
        {
            this.sharesOwned += amount;
            // this.PurchasePrice = CurrentPrice
        }

        private void CalculateVariance()
        {
            Int32 year;
            decimal sumSquared = 0;
            int numYears = DateTime.UtcNow.Year - 2006;
            decimal prob = numYears / 100;
            for (int i = 0; i < numYears; i++)
            {
                if (i < 10)
                { year = Convert.ToInt32(String.Format("200{0}", i.ToString())); } else
                { year = Convert.ToInt32(String.Format("20{0}", i.ToString())); }

                List<decimal> prices = StockHelper.GetHistoricalPricesCustom(this.Symbol, new DateTime(year, 1, 1), new DateTime(year, 12, 31));
                decimal annualReturn = (prices[prices.Count - 1] / prices[0]) - 1;
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(this.expectedReturn - annualReturn), 2));
                sumSquared += squared;
            }
            this.Variance = sumSquared / numYears;
        }

        private void CalculateExpectedReturn()
        {
            StockHelper StockHelper = new StockHelper();
            decimal beta = StockHelper.getBeta(this.Symbol);
            decimal marketRiskPremium = sp500avgReturn - riskFreeReturn;
            decimal riskPremium = beta * marketRiskPremium;
            this.expectedReturn = riskFreeReturn + riskPremium;
            //risk-free return + risk premium = expected return
        }
    }

}
