using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public static class MonteCarlo
    {
        //const int nSimulations = 1000;
        public static PortfolioPath[] RunSimulation(int nSteps, int nPaths, Func<int, PortfolioPath> creator)
        {
            PortfolioPath[] paths = new PortfolioPath[nPaths];
            IEnumerable<int> indices = Enumerable.Range(0, nPaths - 1);
            Console.WriteLine("Starting simulation.");
            paths = indices.AsParallel().Select(creator).ToArray(); // this is array that contains all paths, can go through all and calculate the odds of certain outcomes. 
            Console.WriteLine("End of simulation.");
            IEnumerable<double> endingValues = paths.Select(x => x.endingPortfolioValue);
            Console.WriteLine("Min portfolio from simulation: " + endingValues.Min().ToString());
            Console.WriteLine("Avg portfolio from simulation: " + endingValues.Average().ToString());
            Console.WriteLine("Max portfolio from simulation: " + endingValues.Max().ToString());

            return paths;
        }
        
    }
}
