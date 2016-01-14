using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public class PathAnalysis
    {
        public static List<double> GetAveragePath(PortfolioPath[] paths, int steps)
        {
            List<double> avgPathSequence = new List<double>();
            for (int i = 0; i < steps - 1; i++)
            {
                double[] avgValuesArray = paths.Select(x => x.portfolioValueList[i]).ToArray();
                double avgValue = avgValuesArray.Average();
                avgPathSequence.Add(avgValue);
            }
            return avgPathSequence;
        }
        public static List<double> GetMaximumPath(PortfolioPath[] paths, int steps)
        {
            IEnumerable<double> endingValues = paths.Select(x => x.endingPortfolioValue).ToArray();
            double maxEndingValue = endingValues.Max();
            var maxPathQuery = paths.Where(path => path.endingPortfolioValue == maxEndingValue).ToArray();
            List<double> maxPathSequence = maxPathQuery[0].portfolioValueList;
            return maxPathSequence;
        }
        public static List<double> GetMinimumPath(PortfolioPath[] paths, int steps)
        {
            IEnumerable<double> endingValues = paths.Select(x => x.endingPortfolioValue).ToArray();
            double minEndingValue = endingValues.Min();
            var maxPathQuery = paths.Where(path => path.endingPortfolioValue == minEndingValue).ToArray();
            List<double> minPathSequence = maxPathQuery[0].portfolioValueList;
            return minPathSequence;
        }
    }
}
