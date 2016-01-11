using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;

namespace Folio.Models.MattsModels
{
    //Asset Class
    public class Asset
    {
        protected string Name;
        protected decimal purchasePrice, currentPrice;
        
    }
    //Stock Class
    public class Stock : Asset
    {
        public string symbol { get; private set; }
        private int sharesOwned;
        private decimal weight;
        private const decimal marketRiskPremium = 6.34M;
        private const decimal riskFreeReturn = 2.70M;
        private decimal expectedReturn;
        private decimal variance;


        public decimal CurrentPrice { get { return currentPrice; } set { currentPrice = value; } }
        //use StockHelper.getCurrentPrice()
        public string Symbol { get { return symbol; } }
        public decimal PurchasePrice { get { return purchasePrice; } }

        public int SharesOwned { get { return sharesOwned; } }
        public decimal Worth
        {
            get { return CurrentPrice * SharesOwned; }
        }

        public decimal Weight { get { return weight; } set { weight = value; } }
        public decimal ExpectedReturn { get { return expectedReturn; } set { expectedReturn = value; } }
        public decimal Variance { get { return variance; } set { variance = value; } }



        public Stock(string symbol, decimal purchasePrice, int sharesOwned)
        {
            this.symbol = symbol;
            this.purchasePrice = currentPrice = purchasePrice;
            this.sharesOwned = sharesOwned;
            CalculateExpectedReturn();
            CalculateVariance();
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
            int numYears = DateTime.UtcNow.Year - 2000;
            decimal prob = 100 / numYears;
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
            decimal riskPremium = beta * marketRiskPremium;
            this.expectedReturn = riskFreeReturn + riskPremium;

            //risk-free return + risk premium = expected return

        }

    }
}
