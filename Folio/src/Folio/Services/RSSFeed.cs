using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Folio.Services
{
    public static class RSSFeed
    {
        public static string GetRSSHttp(int? id)
        {
            if (id == 1)
            {
                return "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Stocks";
            }
            else if (id == 2)
            {
                return "http://finance.yahoo.com/rss/headline?s=yhoo";
            }
            else if (id == 3)
            {
                return "http://rss.cnn.com/rss/money_markets.rss";
            }
            else if (id == 4)
            {
                return "http://feeds.marketwatch.com/marketwatch/StockstoWatch/";
            }
            else if (id == 5)
            {
                return "http://feeds.reuters.com/reuters/hotStocksNews";
            }
            else if (id == 6)
            {
                return "http://economictimes.indiatimes.com/rssfeeds/2146842.cms";
            }
            else
            {
                return "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Stocks";
            }
        }

        public static XmlNodeList GetXMLNodeList(string Http)
        {
            XmlTextReader RSS = new XmlTextReader(Http);
            XmlDocument XML = new XmlDocument();
            XML.Load(RSS);
            return XML.SelectNodes("//item");
        }

        public static string GetTitleName(int? id)
        {
            if (id == 1)
            {
                return "NASDAQ - Stocks";
            }
            else if (id == 2)
            {
                return "Yahoo Finance - Top Stories";
            }
            else if (id == 3)
            {
                return "CNN - Money Markets";
            }
            else if (id == 4)
            {
                return "Marketwatch - Stocks to Watch";
            }
            else if (id == 5)
            {
                return "Reuters - Hot Stock News";
            }
            else if (id == 6)
            {
                return "Economic Times - Stock Market Update";
            }
            else
            {
                return "NASDAQ - Stocks";
            }
        }
    }
}
