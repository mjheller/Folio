using AForge;
using Folio.Logic;
using Folio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Folio.SeedData
{
    public class MrClean
    {
        public static void Clean(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();

            List<Stock> stocks = context.Stock.Select(s => s).ToList();
            List<Task> tasks = new List<Task>();
            int count = 0;
            foreach (Stock stock in stocks)
            {
                Task task = Task.Run(() => CheckIfTickerExists(stock, context));
                tasks.Add(task);
                count++;
            }
            Task.WaitAll(tasks.ToArray());
            context.SaveChanges();
        }

        private static void CheckIfTickerExists(Stock stock, ApplicationDbContext context)
        {
            try
                {
                    YahooAPICalls.GetCurrentStockPrice(stock.Symbol);
                }
                catch (Exception ex)
                {
                    context.Remove(stock);
                }
        }

    }
}
