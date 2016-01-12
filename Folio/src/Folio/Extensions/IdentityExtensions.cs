using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace folio.Services
{
    public static class IdentityExtensions
    {
        public static ApplicationUser GetCurrentUser(this IIdentity identity, ApplicationDbContext context)
        {
            string userName = identity.Name;
            ApplicationUser currentUser = context.Users.Single(u => u.UserName == userName);
            return currentUser;
        }
    }
}
