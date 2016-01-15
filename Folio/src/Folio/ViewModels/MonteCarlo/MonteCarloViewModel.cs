using Folio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels.MonteCarlo
{
    public class MonteCarloViewModel
    {
        public PortfolioViewModel PortfolioViewModel { get; set; }
        [Display(Name = "Annual Contribution")]
        public double AnnualContribution { get; set; }
        [Display(Name = "Retirement Age")]
        public int PreferredRetirementAge { get; set; }
        [Display(Name = "Retirement Span")]
        public int EstimatedRetirementSpan { get; set; }
        [Display(Name = "Retirement Income Draw")]
        public double AnnualRetirementIncomeDraw { get; set; }
        public IEnumerable<decimal> MonteCarloResults { get; set; }
        public int StartingAge { get; set; }
    }
}
