using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Folio.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AnnualIncome { get; set; }
        [Range(1, 5)]
        public int RatedInvestmentKnowledge { get; set; }
        [Range(1, 5)]
        public int RiskTolerance { get; set; }
        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
