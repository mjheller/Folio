using Microsoft.AspNet.Identity;
using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Extensions
{
    public static class UserManagerExtensions
    {
        public async static Task<bool> HasPortfolios(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            bool hasPortfolios = false;
            if (user.Portfolios != null) hasPortfolios = true;
            return hasPortfolios;
        }
    }
}
