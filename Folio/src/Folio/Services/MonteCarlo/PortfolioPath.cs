using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Folio.Services.MonteCarlo
{
    public class PortfolioPath
    {
        int nSteps;
        int retirement;
        double incomeDraw;
        double annualContribution;
        double initialPortfolioValue;
        public double endingPortfolioValue;
        public double folioReturn;
        public double folioStDev;
        public List<double> portfolioValueList;
        int stepCount = 1;
        // remember changes made in Stock class with Math.Log etc marked with //add
        public PortfolioPath(int yearsUntilRetirement, double expectedReturn, double variance, double initialPortfolioValue, double annualContribution, double incomeDraw, int yearsPlannedRetirement)
        {
            this.folioReturn = expectedReturn;
            this.folioStDev = Math.Sqrt(variance) / 100;
            this.annualContribution = annualContribution;
            this.incomeDraw = incomeDraw;
            this.initialPortfolioValue = initialPortfolioValue;
            this.retirement = yearsUntilRetirement;
            this.nSteps = retirement + yearsPlannedRetirement;
            this.portfolioValueList = new List<double>();
            this.portfolioValueList.Add(initialPortfolioValue);
            LogNormal lognormal = LogNormal.WithMuSigma(folioReturn, folioStDev);
            IEnumerable<double> returns = lognormal.Samples().Take(nSteps);
            this.endingPortfolioValue = returns.Aggregate(initialPortfolioValue, ComputeNextPortfolioValue);
        }
        private double ComputeNextPortfolioValue(double currentValue, double randomReturn)
        {
            double value = ComputePortfolioChange(currentValue, randomReturn);
            stepCount++;
            portfolioValueList.Add(value);
            return value;
        }
        private double ComputePortfolioChange(double currentValue, double randomReturn)//Computes change in value between two points in the path
        {

            if (stepCount < retirement)
            {
                return currentValue * randomReturn + annualContribution;
            } 
            else
            {
                return currentValue * randomReturn - incomeDraw;
            }


        }
    }
}
