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

        [Display(Name = "Expected Return")]
        public string ExpectedReturnString { get; set; }

        public double ExpectedReturnDouble { get; set; }

        public decimal Variance { get; set; }

        [Display(Name = "Dollar Value")]
        [DataType(DataType.Currency)]
        public decimal DollarValue { get; set; }

        public IEnumerable<StockViewModel> Stocks { get; set; }
    }
}
