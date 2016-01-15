using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels
{
    public class PortfolioViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Currency)]
        public decimal ExpectedReturn { get; set; }

        [DataType(DataType.Currency)]
        public decimal Variance { get; set; }

        [DataType(DataType.Currency)]
        public decimal DollarValue { get; set; }

        public IEnumerable<StockViewModel> Stocks { get; set; }
    }
}
