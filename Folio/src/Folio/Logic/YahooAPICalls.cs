using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;
using HtmlAgilityPack;
using YSQ.core.Quotes;

namespace Folio.Logic
{
    public static class YahooAPICalls
    {
        public static IEnumerable<HistoricalPrice> GetStockHistoricalPrices(string ticker, DateTime startDate, DateTime endDate)
        {
            HistoricalPriceService hps = new HistoricalPriceService();
            IEnumerable<HistoricalPrice> historicalPrices = null;
            try
            {
                historicalPrices = hps.Get(ticker, startDate, endDate, Period.Daily);
            }
            catch(Exception ex)
            {
                return null;
            }
            return historicalPrices;
        }

        public static decimal GetCurrentStockPrice(string ticker)
        {
            HistoricalPriceService hps = new HistoricalPriceService();
            IEnumerable<HistoricalPrice> price;
            decimal currentPrice; 
            DateTime mostRecentDate;
            try
            {
                price = hps.Get(ticker, DateTime.Now, DateTime.Now, Period.Daily);
                mostRecentDate = price.Select(p => p.Date).Max();
                currentPrice = price.Single(p => p.Date == mostRecentDate).Price;
                return currentPrice;
            }
            catch (Exception ex)
            {
                return GetCurrentStockPrice(hps, ticker, DateTime.Now.AddDays(-1), DateTime.Now);
            }
        }

        private static decimal GetCurrentStockPrice(HistoricalPriceService hps, string ticker, DateTime start, DateTime end)
        {
            IEnumerable<HistoricalPrice> price;
            decimal currentPrice; 
            DateTime mostRecentDate;
            try
            {
                price = hps.Get(ticker, start, end, Period.Daily);
                mostRecentDate = price.Select(p => p.Date).Max();
                currentPrice = price.Single(p => p.Date == mostRecentDate).Price;
                return currentPrice;
            }
            catch (Exception Ex)
            {
                return GetCurrentStockPrice(hps, ticker, start.AddDays(-1), end);
            }
        }

        public static decimal GetStockBeta(string ticker)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            decimal beta = 0;
            HtmlDocument document = htmlWeb.Load(string.Format("http://finance.yahoo.com/q?s={0}&ql=1", ticker));
            HtmlNode node = document.GetElementbyId("table1");
            if (node != null)
            {
                HtmlNode[] td = node.Descendants("td").ToArray();
                try
                {
                    beta = Convert.ToDecimal(td[5].InnerHtml);
                }
                catch(Exception ex)
                {
                    beta = 1;
                }
            }
            return beta;
        }
    }
}

