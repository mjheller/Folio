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
        public int ID { get; private set; }
        public DateTime DateCreated { get; private set; }
        public List<StockDomainModel> Stocks { get; private set; }
        public decimal ExpectedReturn { get; private set; }
        public decimal Variance { get; private set; }
        public  decimal DollarValue { get; private set; }
        public string Name { get; private set; }

        public PortfolioDomainModel(List<StockDomainModel> stocks, Portfolio portfolio)
        {
            Stocks = stocks;
            ID = portfolio.ID;
            Name = portfolio.Name;
            DateCreated = portfolio.DateCreated;
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
                count += s.Worth;
            }
            DollarValue = count;
        }

        private void SetWeights()
        {
            foreach (StockDomainModel s in Stocks)
            {
                s.Weight = s.Worth / DollarValue;
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
                        decimal covariance = CalculateCovariance(stocks[i].DailyReturns1YearAsArray(), stocks[j].DailyReturns1YearAsArray());
                        decimal pair = 2 * stocks[i].Weight * stocks[j].Weight * covariance;
                        localVariance += pair;
                    }
                }
                localVariance += nthWeightedVariance;
            }
            Variance = localVariance;
        }

        private void CalculateExpectedReturn()
        {
            decimal sum = 0;
            foreach (StockDomainModel s in Stocks)
            {
                decimal weightedReturn = s.Weight * s.ExpectedReturn;
                sum += weightedReturn;
            }
            this.ExpectedReturn = sum;
        }
    }
}
