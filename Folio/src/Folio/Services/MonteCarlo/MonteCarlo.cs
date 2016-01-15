using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public static class MonteCarlo
    {
        public static PortfolioPath[] RunSimulation(int yearsUntilRetirement, 
            int nPaths, double expectedReturn, double variance, double initialPortfolioValue, 
            double annualContributions, double incomeDraw, int yearsPlannedRetirement)
        {
            PortfolioPath[] paths = new PortfolioPath[nPaths];
            Func<int, PortfolioPath> creator = x => new PortfolioPath(yearsUntilRetirement, expectedReturn, variance, initialPortfolioValue, annualContributions, incomeDraw, yearsPlannedRetirement);
            IEnumerable<int> indices = Enumerable.Range(0, nPaths - 1);
            paths = indices.AsParallel().Select(creator).ToArray();
            
            return paths;
        }
    }
}
