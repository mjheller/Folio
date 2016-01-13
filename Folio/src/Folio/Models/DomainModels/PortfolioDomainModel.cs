using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.Statistics;
using Folio.Logic;

namespace Folio.Models
{
    public class PortfolioDomainModel
    {
        public List<StockDomainModel> Stocks;
        public decimal expectedReturn { get; private set; }
        public decimal variance { get; private set; }
        private decimal dollarValue;

        public PortfolioDomainModel(List<StockDomainModel> stocks)
        {
            Stocks = stocks;
            UpdatePortfolioDollarValue();
            SetWeights();
            CalculateExpectedReturn();
            CalculateVariance(Stocks);
        }

        private void UpdatePortfolioDollarValue()
        {
            decimal count = 0;
            foreach (StockDomainModel s in Stocks)
            {
                count += (s.Worth);
            }
            this.dollarValue = count;
        }

        private void UpdateCurrentPrices()
        {
            foreach (StockDomainModel s in Stocks)
            {
                s.CurrentPrice = YahooAPICalls.GetCurrentStockPrice(s.Ticker);
            }
        }

        private void AddToPortfolio(StockDomainModel stock)
        {
            Stocks.Add(stock);
        }

        private void SetWeights()
        {
            foreach (StockDomainModel s in Stocks)
            {
                s.Weight = (s.Worth) / dollarValue;
            }
        }

        private decimal CalculateCovariance(double[] stock1, double[] stock2)
        {
            double covariance = Tools.Covariance(stock1, stock2);
            return (decimal)covariance;
        }

        private void CalculateVariance(List<StockDomainModel> stocks)
        {
            decimal localVariance = 0;

            for (int i = 0; i < stocks.Count; i++)
            {
                decimal nthWeightedVariance = (decimal)Math.Pow((double)stocks[i].Weight, 2) * stocks[i].Variance;

                for (int j = i + 1; j < stocks.Count; j++)
                {
                    if (!(i == stocks.Count - 1))
                    {
                        decimal covariance = CalculateCovariance(stocks[i].PriceHistory1Year, stocks[j].PriceHistory1Year);
                        decimal pair = 2 * stocks[i].Weight * stocks[j].Weight * covariance;
                        localVariance += pair;
                    }
                }
                localVariance += nthWeightedVariance;
            }
            variance = localVariance;
        }

        private void CalculateExpectedReturn()
        {
            decimal sum = 0;
            foreach (StockDomainModel s in Stocks)
            {
                decimal weightedReturn = s.Weight * s.ExpectedReturn;
                sum += weightedReturn;
            }
            this.expectedReturn = sum;
        }
    }
}
