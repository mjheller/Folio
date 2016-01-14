using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Folio.Services
{
    public static class RSSFeed
    {
        public static string GetRSSHttp(int? value)
        {
            if (value == 1)
            {
                return "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Stocks";
            }
            else if (value == 2)
            {
                return "http://finance.yahoo.com/rss/headline?s=yhoo";
            }
            else if (value == 3)
            {
                return "http://rss.cnn.com/rss/money_markets.rss";
            }
            else if (value == 4)
            {
                return "http://feeds.marketwatch.com/marketwatch/StockstoWatch/";
            }
            else if(value == 5)
            {
                return "http://feeds.reuters.com/reuters/hotStocksNews";
            }
            else if(value == 6)
            {
                return "http://rss.briefing.com/Investor/RSS/StockMarketUpdate.xml";
            }
            else
            {
                return "null";
            }
        }

        public static XmlNodeList GetXMLNodeList(string Http)
        {
            XmlTextReader RSS = new XmlTextReader(Http);
            XmlDocument XML = new XmlDocument();
            XML.Load(RSS);
            return XML.SelectNodes("//item");
        }
    }
}
