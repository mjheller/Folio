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

namespace Folio.SeedData
{
    public class MrClean
    {
        public static void Clean(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            List<Stock> stocks = context.Stock.Select(s => s).ToList();
            //Parallel.For(0, stocks.Count(), async i =>
            //{
            //    try
            //    {
            //        YahooAPICalls.GetCurrentStockPrice(stocks[i].Symbol);
            //    }
            //    catch (Exception ex)
            //    {
            //        context.Remove(stocks[i]);
            //        var x = context.SaveChangesAsync();
            //        await x;
            //    }
            //});
            foreach (Stock stock in stocks)
            {
                try
                {
                    YahooAPICalls.GetCurrentStockPrice(stock.Symbol);
                }
                catch (Exception ex)
                {
                    context.Remove(stock);
                    context.SaveChangesAsync();
                }
            }
        }

    }
}
