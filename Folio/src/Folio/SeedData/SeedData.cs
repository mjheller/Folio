using Folio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio.SeedData
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            if (context.Database == null)
            {
                throw new Exception("DB is null");
            }
            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                InitializeRoleAdmin(context, roleManager);
            }
            if (!(context.Users.Any(u => u.UserName == "admin@gmail.com")))
            {
                UserManager<ApplicationUser> userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                InitializeUserAdmin(context, userManager);
            }
            if (!(context.Stock.Any()))
            {
                SeedStocks(context);
            }
        }

        private static void SeedStocks(ApplicationDbContext context)
        {
            Dictionary<string, string> stockFiles = new Dictionary<string, string> {
                { "NasdaqQuote", "NASDAQ" },
                { "NyseAmexQuote","NYSE AMEX" },
                { "NYSEQuote","NYSE" },
                { "SP500Quote", "S&P 500" }
            };
            List<SeedStock> seedStocks = new List<SeedStock>();
            foreach (string stock in stockFiles.Keys)
            {
               seedStocks.AddRange(SeedDataHelperFunctions.ParseStockCSV($"c:\\users\\chris\\github\\folio\\folio\\src\\folio\\stockdata\\{stock}.csv", stockFiles[stock]));
            }
            List<Stock> dataStocks = SeedDataHelperFunctions.SeedStockData(seedStocks);
            context.AddRange(dataStocks);
            context.SaveChanges();
        }

        private static void InitializeRoleAdmin(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            IdentityRole role = new IdentityRole { Name = "admin" };
            roleManager.CreateAsync(role);
            context.SaveChanges();
        }

        private static void InitializeUserAdmin(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com"
            };

            context.SaveChanges();
        }
    }
}
