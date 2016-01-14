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
            IEnumerable<HistoricalPrice> price = hps.Get(ticker, DateTime.Today.AddDays(-1), DateTime.UtcNow, Period.Daily);
            decimal currentPrice = price.ElementAt(0).Price;
            return currentPrice;
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

