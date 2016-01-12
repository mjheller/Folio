using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels
{
    public class AddStockToPortfolioViewModel
    {
        Portfolio Portfolio { get; set; }
        List<StockViewModel> AvailableAssetTickers { get; set; }
    }
}
