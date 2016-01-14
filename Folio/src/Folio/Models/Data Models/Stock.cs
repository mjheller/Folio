using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class Stock : Asset
    {
        [Key]
        public string Symbol { get; set; }
        public string Name { get; set; }
        //public DateTime LastUpdate { get; set; }
        //public decimal? Variance { get; set; }
        //public decimal? ExpectedReturn { get; set; }
        //public string DailyReturns1Year { get; set; }
        public string Exchange { get; set; }
    }
}
