using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels.MonteCarlo
{
    public class MonteCarloViewModel
    {
        public PortfolioViewModel portfolioViewModel { get; set; }
        public double AnnualContribution { get; set; }
        public int PreferredRetirementAge { get; set; }
        public int EstimatedRetirementSpan { get; set; }
        public double AnnualRetirementIncomeDraw { get; set; }
    }
}
