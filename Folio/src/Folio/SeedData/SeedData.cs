using Folio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                Thread.Sleep(5000);
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
            HashSet<string> tickers = new HashSet<string>();
            foreach (string stock in stockFiles.Keys)
            {
               seedStocks.AddRange(SeedDataHelperFunctions.ParseStockCSV($"c:\\users\\phantom\\github\\folio\\folio\\src\\folio\\stockdata\\{stock}.csv", stockFiles[stock], tickers));
            }
            List<Stock> dataStocks = SeedDataHelperFunctions.SeedStockData(seedStocks);
            context.Stock.AddRange(dataStocks);
            context.SaveChanges();
        }

        private async static void InitializeRoleAdmin(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            IdentityRole role = new IdentityRole { Name = "admin" };
            await roleManager.CreateAsync(role);
            context.SaveChanges();
        }

        private async static void InitializeUserAdmin(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                EmailConfirmed = true
            };
            Thread.Sleep(2000);
            await userManager.CreateAsync(admin, "Beast@2"); //password must match constraints of 6 char min, case-change, min 1 number and non-letter character
            Thread.Sleep(2000);
            await userManager.AddToRoleAsync(admin, "admin");
            context.SaveChanges();
        }
    }
}
