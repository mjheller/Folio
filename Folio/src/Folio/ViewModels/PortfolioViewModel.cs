using Folio.Models.MattsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels
{
    public class PortfolioViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int ExpectedReturn { get; set; }
        public int Variance { get; set; }
        public IEnumerable<StockViewModel> Stocks { get; set; }
    }
}
