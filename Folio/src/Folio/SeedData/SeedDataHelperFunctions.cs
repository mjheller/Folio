using Folio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.SeedData
{
    public static class SeedDataHelperFunctions
    {
        public static List<SeedStock> ParseStockCSV(string filePath, string exchange)
        {
            var lines = File.ReadAllLines(filePath).Select(a => a.Split(';'));
            var csv = from line in lines
                      select (from piece in line select piece).ToArray();

            int rowCount = 1;
            int innerCount = 1;
            List<SeedStock> stocks = new List<SeedStock>();
            string ticker ="";
            foreach (var row in csv)
            {
                if (rowCount > 4)
                {
                    string line = row[0];
                    if (innerCount == 1)
                    {
                        string[] lineSplit = line.Split(',');
                        ticker = lineSplit[0];
                    }
                    if (innerCount == 6)
                    {
                        string[] lineSplit = line.Split(',');
                        string name = lineSplit[lineSplit.Length-1];
                        if (name.Length < 3)
                        {
                            name = null;
                        }
                        stocks.Add(new SeedStock {
                            Symbol = ticker,
                            Name = name,
                            Exchange = exchange
                        });
                        ticker = "";
                        innerCount = 0;
                    }
                    innerCount++;
                }
                rowCount++;
            }
            return stocks;
        }

        public static List<Stock> SeedStockData(List<SeedStock> seedStocks)
        {
            List<StockDomainModel> stocks = new List<StockDomainModel>();
            foreach (SeedStock stock in seedStocks)
            {
                stocks.Add(new StockDomainModel(stock.Symbol, stock.Name, stock.Exchange)); 
            }

            List<Stock> dataStocks = new List<Stock>(); 
            foreach (StockDomainModel s in stocks)
            {
                dataStocks.Add(new Stock {
                    Symbol = s.Ticker,
                    Name = s.Name,
                    LastUpdate = s.LastUpdated,
                    Variance = s.Variance,
                    ExpectedReturn = s.ExpectedReturn,
                    DailyReturns1Year = s.GetDailyReturns1YearAsJSON,
                    Exchange = s.Exchange,
                });
            }
            return dataStocks;
        }
    }
}
