using Folio.Logic;
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
        //if error comment out in startup
        public static List<SeedStock> ParseStockCSV(string filePath, string exchange, HashSet<string> tickers)
        {
            var lines = File.ReadAllLines(filePath).Select(a => a.Split(';'));
            var csv = from line in lines
                      select (from piece in line select piece).ToArray();

            int rowCount = 1;
            int innerCount = 1;
            List<SeedStock> stocks = new List<SeedStock>();
            string ticker = "";
            foreach (var row in csv)
            {
                if (rowCount > 4)
                {
                    string line = row[0];
                    if (innerCount == 1)
                    {
                        string[] lineSplit = line.Split(',');
                        ticker = lineSplit[0].Trim().ToUpper();
                    }
                    if (innerCount == 6)
                    {
                        string[] lineSplit = line.Split(',');
                        string name = lineSplit[lineSplit.Length-1];
                        if (name.Length < 3)
                        {
                            name = null;
                        }
                        if (!(tickers.Contains(ticker)) && ticker != null)
                        {
                            stocks.Add(new SeedStock {
                                Symbol = ticker,
                                Name = name,
                                Exchange = exchange
                            });
                            tickers.Add(ticker);
                        }
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
            Stock[] dataStocks = new Stock[seedStocks.Count()];
            Parallel.For(0, seedStocks.Count(), i =>
            {
                dataStocks[i] = (new Stock {
                    Symbol = seedStocks[i].Symbol,
                    Name = seedStocks[i].Name,
                    Exchange = seedStocks[i].Exchange
                });
            });
            return dataStocks.ToList();
        }
    }
}
