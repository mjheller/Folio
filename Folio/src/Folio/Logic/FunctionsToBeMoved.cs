using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folio.Models;
using folio.Services;

namespace Folio.Logic
{
    public class FunctionsToBeMoved
    {
        ApplicationDbContext _context;

        public FunctionsToBeMoved(ApplicationDbContext context)
        {
            _context = context;
        }
         
        public Portfolio AssignCurrentUserToPortfolio(Portfolio portfolio)
        {
            portfolio.User = System.Security.Claims.ClaimsPrincipal.Current.Identity.GetCurrentUser(_context);
            _context.Portfolio.Update(portfolio);
            _context.SaveChangesAsync();
            return portfolio;
        }
    }
}
