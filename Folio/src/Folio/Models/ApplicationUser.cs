using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Folio.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        string firstName { get; set; }
        string lastName { get; set; }
        string phoneNumber { get; set; }
        string desiredRisk { get; set; }
        int income { get; set; }

    }
}
