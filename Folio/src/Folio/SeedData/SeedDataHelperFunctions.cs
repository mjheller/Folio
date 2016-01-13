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
        public static List<Stock> ParseStockCSV(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(a => a.Split(';'));
            var csv = from line in lines
                      select (from piece in line select piece).ToArray();

            int rowCount = 1;
            int innerCount = 1;
            List<Stock> stocks = new List<Stock>();
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
                        string description = lineSplit[lineSplit.Length-1];
                        if (description.Length < 3)
                        {
                            description = null;
                        }
                        stocks.Add(new Stock {
                            Symbol = ticker,
                            Description = description
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
    }
}
