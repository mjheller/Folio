using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.Statistics;


namespace Folio.Models
{
    public class Portfolio
    {
        List<Stock> portfolio;
        public decimal expectedReturn { get; private set; }
        public decimal variance { get; private set; }
        private decimal dollarValue;
        StockHelper StockHelper;

        public Portfolio()
        {
            this.portfolio = new List<Stock>();

        }
        public Portfolio(List<Stock> stocks)
        {
            StockHelper = new StockHelper();
            this.portfolio = stocks;
            this.UpdatePortfolioDollarValue();
            this.SetWeights();
            CalculateExpectedReturn();
            CalculateVariance(this.portfolio);
        }
        private void UpdatePortfolioDollarValue()
        {
            decimal count = 0;
            this.UpdateCurrentPrices();
            foreach (Stock s in portfolio)
            {
                count += (s.Worth);
            }
            this.dollarValue = count;
        }
        private void UpdateCurrentPrices()
        {
            foreach (Stock s in portfolio)
            {
                s.CurrentPrice = StockHelper.getCurrentPrice(s.Symbol);
            }
        }
        private void AddToPortfolio(Stock stock)
        {
            this.portfolio.Add(stock);

        }

        private void SetWeights()
        {
            foreach (Stock s in portfolio)
            {
                s.Weight = (s.Worth) / this.dollarValue;
            }
        }

        private decimal CalculateCovariance(double[] stock1, double[] stock2)
        {
            double covariance = Tools.Covariance(stock1, stock2);
            return (decimal)covariance;
        }
        private void CalculateVariance(List<Stock> stocks)
        {
            decimal localVariance = 0;

            for (int i = 0; i < stocks.Count; i++)
            {
                decimal nthWeightedVariance = (decimal)Math.Pow((double)stocks[i].Weight, 2) * stocks[i].Variance;

                for (int j = i + 1; j < stocks.Count; j++)
                {
                    if (i == stocks.Count - 1)
                    {/*do nothing*/} else
                    {
                        decimal covariance = CalculateCovariance(stocks[i].priceHistory1Year, stocks[j].priceHistory1Year);
                        decimal pair = 2 * stocks[i].Weight * stocks[j].Weight * covariance;
                        variance += pair;
                    }
                }
                localVariance += nthWeightedVariance;
            }
            this.variance = localVariance;
        }

        private void CalculateExpectedReturn()
        {
            decimal sum = 0;
            foreach (Stock s in portfolio)
            {
                decimal weightedReturn = s.Weight * s.ExpectedReturn;
                sum += weightedReturn;
            }
            this.expectedReturn = sum;
        }
    }
}
