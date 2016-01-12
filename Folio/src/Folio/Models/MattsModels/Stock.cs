using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;

namespace Folio.Models.MattsModels
{
    //Stock Class
    public class Stock : Asset
    {
        private const decimal marketRiskPremium = 6.34M;
        private const decimal riskFreeReturn = 2.70M;

        [DataType("Currency")]
        public decimal CurrentPrice { get; set; }

        [DataType("Currency")]
        public decimal PurchasePrice { get; set; }

        public decimal Weight { get; set; }
        public decimal ExpectedReturn { get; set; }
        public decimal Variance { get; set; }
       
        public int SharesOwned { get; private set; }
        public decimal Worth
        {
            get { return CurrentPrice * SharesOwned; }
        }

   
        public Stock(string symbol, decimal purchasePrice, int sharesOwned)
        {
            this.Symbol = symbol;
            this.PurchasePrice = purchasePrice;
            this.SharesOwned = sharesOwned;
            CalculateExpectedReturn();
            CalculateVariance();
        }

        private void addShares(int amount)
        {
            this.SharesOwned += amount;
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
                decimal squared = Convert.ToDecimal(Math.Pow(Convert.ToDouble(this.ExpectedReturn - annualReturn), 2));
                sumSquared += squared;
            }
            this.Variance = sumSquared / numYears;
        }

        private void CalculateExpectedReturn()
        {
            StockHelper StockHelper = new StockHelper();
            decimal beta = StockHelper.getBeta(this.Symbol);
            decimal riskPremium = beta * marketRiskPremium;
            this.ExpectedReturn = riskFreeReturn + riskPremium;

            //risk-free return + risk premium = expected return

        }

    }
}
