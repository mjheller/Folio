using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models.MattsModels
{
    public class Portfolio
    {
        Dictionary<Stock, int> portfolio;
        int expectedReturn;
        int variance;
        private decimal dollarValue;
        StockHelper StockHelper;

        public Portfolio()
        {
            this.portfolio = new Dictionary<Stock, int>();
        }
        public Portfolio(Dictionary<Stock, int> Stocks)
        {
            StockHelper = new StockHelper();
            this.portfolio = Stocks;
            this.updatePortfolioDollarValue();
            this.setWeights();
        }
        private void updatePortfolioDollarValue()
        {
            decimal count = 0;
            this.updateCurrentPrices();
            foreach (KeyValuePair<Stock, int> kvp in portfolio)
            {
                count += (kvp.Key.CurrentPrice * kvp.Value);
            }
            this.dollarValue = count;
        }
        private void updateCurrentPrices()
        {
            foreach (Stock s in portfolio.Keys)
            {
                s.CurrentPrice = StockHelper.getCurrentPrice(s.Symbol);
            }
        }
        private void addToPortfolio(Stock stock, int shares)
        {
            this.portfolio.Add(stock, shares);

        }

        private void setWeights()
        {
            foreach (Stock s in portfolio.Keys)
            {
                //weight of security in portfolio is dollar value of a security by the total dollar value of the portfolio
                // may be able to get rid of dictionary int shares depending if have a ParentPortfolio link on each stock linking it to the portfolio it belongs to. Could then reference the stock.worth;
                s.Weight = (s.CurrentPrice * s.SharesOwned) / this.dollarValue;
            }
        }

        //private int Variance(List<Stock> stocks)
        //{
        //    foreach (Stock s in stocks)
        //    {

        //    }
        //    decimal variance = (weight(1) ^ 2 * variance(1) + weight(2) ^ 2 * variance(2) + 2 * weight(1) * weight(2) * covariance(1, 2)



        //}
    }
}
