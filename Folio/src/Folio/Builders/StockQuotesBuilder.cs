using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folio.Models;
using System.Net;
using System.IO;
using System.Text;

namespace Folio.Builders
{
    public class StockQuotesBuilder
    {
        public Stocks createQuotes (string m_symbol)
        {
            const string stringForSearchData = "&f=sl1d1t1c1hgvbap2";
            Stocks stocks = new Stocks();
            
            stocks.companies = m_symbol.Replace(",", " ").Split(' ');
            // Update the textbox value.
            //txtSymbol.Value = m_symbol;

            if (m_symbol.Trim() != "")
            {
                try
                {
                    // Return the stock quote data in XML format.
                    string symbol = m_symbol.Trim();
                    // Use Yahoo finance service to download stock data from Yahoo
                    string yahooURL = @"http://download.finance.yahoo.com/d/quotes.csv?s=" + symbol + stringForSearchData;
                    string[] symbols = symbol.Replace(",", " ").Split(' ');

                        // Initialize a new WebRequest.
                    HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(yahooURL);
                    // Get the response from the Internet resource.
                    HttpWebResponse webresp = (HttpWebResponse)webreq.GetResponse();
                    while (webresp == null)
                    {
                        webresp = (HttpWebResponse)webreq.GetResponse();
                    }
                    // Read the body of the response from the server.
                    StreamReader strm = new StreamReader(webresp.GetResponseStream(), Encoding.ASCII);

                    // Build our data model.
                    string content = "";
                    for (int i = 0; i < symbols.Length; i++)
                    {
                        Quote quote = new Quote();
                        if (symbols[i].Trim() == "")
                            continue;

                        content = strm.ReadLine().Replace("\"", "");
                        string[] contents = content.ToString().Split(',');
                        // If contents[2] = "N/A". the stock symbol is invalid.
                        if (contents[2] == "N/A")
                        {
                            quote.Symbol = symbols[i].ToUpper() + " is invalid";
                            quote.Last = "N/A";
                            quote.Date = "N/A";
                            quote.Time = "N/A";
                            quote.Change = "N/A";
                            quote.High = "N/A";
                            quote.Low = "N/A";
                            quote.Volume = "N/A";
                            quote.Volume = "N/A";
                            quote.Ask = "N/A";
                            quote.Ask += "N/A";
                        }
                        else
                        {
                            // Build model.
                            quote.Symbol = contents[0];
                            try
                            {
                                quote.Last = String.Format("{0:c}", Convert.ToDouble(contents[1]));
                            }
                            catch
                            {
                                quote.Last = contents[1];
                            }
                            quote.Date = contents[2];
                            quote.Time = contents[3];
                            if (contents[4].Trim().Substring(0, 1) == "-")
                                quote.Change = contents[4] + "(" + contents[10] + ")";
                            else if (contents[4].Trim().Substring(0, 1) == "+")
                                quote.Change += contents[4] + "(" + contents[10] + ")";
                            else
                                quote.Change += contents[4] + "(" +
                                       contents[10] + ")";
                            quote.High = contents[5];
                            quote.Low = contents[6];
                            try
                            {
                                quote.Volume = String.Format("{0:0,0}", Convert.ToInt64(contents[7]));
                            }
                            catch
                            {
                                quote.Volume = contents[7];
                            }
                            quote.Ask = contents[8];
                            quote.Bid = contents[9];

                        }
                        stocks.stocks.Add(quote);
                    }
                    strm.Close();
                }
                catch
                {
                    return null;
                }
            }
            return stocks;
        }
    }
}
