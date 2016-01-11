using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSQ.core.Historical;
using HtmlAgilityPack;

namespace Folio.Models.MattsModels
{
    public class StockHelper
    {
        static HistoricalPriceService historical_price_service;
        HtmlWeb HtmlWeb;
        public StockHelper()
        {
            historical_price_service = new HistoricalPriceService();
            HtmlWeb = new HtmlWeb();
        }
        //public Stock findStock(int riskLevel)
        //{
        //    //input a specified risk level (say 1-10) and will return a Stock or List<Stock> corresponding to specified risk
        //    return Stock;
        //}

        public static List<decimal> GetHistoricalPricesToNow(string ticker, DateTime startDate) /* <= new DateTime(2000, 1, 1)*/
        {

            IEnumerable<HistoricalPrice> historicalPrices = historical_price_service.Get(ticker, startDate, DateTime.UtcNow, Period.Daily);

            List<decimal> priceData = new List<decimal>();
            foreach (var price in historicalPrices)
            {
                priceData.Add(price.Price);
            }
            return priceData;
        }
        public static List<decimal> GetHistoricalPricesCustom(string ticker, DateTime startDate, DateTime endDate)
        {
            IEnumerable<HistoricalPrice> historicalPrices = historical_price_service.Get(ticker, startDate, endDate, Period.Daily);

            List<decimal> priceData = new List<decimal>();
            foreach (var price in historicalPrices)
            {
                priceData.Add(price.Price);
            }
            return priceData;
        }


        public static Dictionary<DateTime, decimal> HistoricalPricesToDict(string ticker, DateTime startDate) /*{startDate format = DateTime(2000, 1, 1)}*/
        {

            IEnumerable<HistoricalPrice> historicalPrices = historical_price_service.Get(ticker, startDate, DateTime.UtcNow, Period.Daily);

            Dictionary<DateTime, decimal> priceData = new Dictionary<DateTime, decimal>();
            foreach (var price in historicalPrices)
            {
                priceData.Add(price.Date, price.Price);
            }
            return priceData;
        }
        public decimal getCurrentPrice(string ticker)
        {
            IEnumerable<HistoricalPrice> price = historical_price_service.Get(ticker, DateTime.UtcNow, DateTime.UtcNow, Period.Daily);
            decimal currentPrice = price.ElementAt(0).Price;
            return currentPrice;

        }

        public decimal getBeta(string ticker)
        {
            decimal beta = 0;

            HtmlDocument document = HtmlWeb.Load(String.Format("http://finance.yahoo.com/q?s={0}&ql=1", ticker));
            HtmlNode someNode = document.GetElementbyId("table1");
            if (someNode != null)
            {
                int count = 0;
                IEnumerable<HtmlNode> td = someNode.Descendants("td");
                foreach (HtmlNode d in td)
                {
                    if (count == 5)
                    {
                        beta = Convert.ToDecimal(d.InnerHtml);

                    }
                    count++;
                }
            }
            return beta;
        }
    }
}
