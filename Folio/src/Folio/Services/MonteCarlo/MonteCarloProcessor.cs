using Folio.Models;
using Folio.ViewModels;
using Folio.ViewModels.MonteCarlo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services.MonteCarlo
{
    public class MonteCarloProcessor
    {
        public static PortfolioPath[] CalculateMonteCarlo(MonteCarloViewModel fullMonte, ApplicationUser user )
        {
            int yearsUntilRetirement = fullMonte.PreferredRetirementAge - (DateTime.Now.Year - user.DateOfBirth.Year);
            int steps = yearsUntilRetirement + fullMonte.EstimatedRetirementSpan;
            const int nPaths = 250000;
            PortfolioViewModel p = fullMonte.PortfolioViewModel;
            PortfolioPath[] paths = MonteCarlo.RunSimulation(yearsUntilRetirement, nPaths, p.ExpectedReturnDouble, (double)p.Variance, (double)p.DollarValue, fullMonte.AnnualContribution, fullMonte.AnnualRetirementIncomeDraw, fullMonte.EstimatedRetirementSpan);
            return paths;
        }


    }
}
