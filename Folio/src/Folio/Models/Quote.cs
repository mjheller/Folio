using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class Quote
    {
        public string Symbol { get; set; }
        public string Last { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Change { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Volume { get; set; }
        public string Bid { get; set; }
        public string Ask { get; set; }
    }
}
