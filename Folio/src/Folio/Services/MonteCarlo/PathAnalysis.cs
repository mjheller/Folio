using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public class PathAnalysis
    {
        public static List<decimal> GetAveragePath(PortfolioPath[] paths, int steps)
        {
            List<decimal> avgPathSequence = new List<decimal>();
            for (int i = 0; i < steps - 1; i++)
            {
                decimal[] avgValuesArray = paths.Select(x => (decimal)x.portfolioValueList[i]).ToArray();
                decimal avgValue = avgValuesArray.Average();
                avgPathSequence.Add(avgValue);
            }
            return avgPathSequence;
        }
        public static List<decimal> GetMaximumPath(PortfolioPath[] paths, int steps)
        {
            IEnumerable<decimal> endingValues = paths.Select(x => (decimal)x.endingPortfolioValue).ToArray();
            decimal maxEndingValue = endingValues.Max();
            var maxPathQuery = paths.Where(path => (decimal)path.endingPortfolioValue == maxEndingValue).ToArray();
            List<decimal> maxPathSequence = maxPathQuery[0].portfolioValueList;

            return maxPathSequence;
        }
        public static List<decimal> GetMinimumPath(PortfolioPath[] paths, int steps)
        {
            IEnumerable<decimal> endingValues = paths.Select(x => (decimal)x.endingPortfolioValue).ToArray();
            decimal minEndingValue = endingValues.Min();
            var minPathQuery = paths.Where(path => (decimal)path.endingPortfolioValue == minEndingValue).ToArray();
            List<decimal> minPathSequence = minPathQuery[0].portfolioValueList;

            return minPathSequence;
        }
    }
}