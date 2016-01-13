using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels
{
    public class AddStockToPortfolioViewModel
    {
        public Portfolio WorkingPortfolio { get; set; }
        public IEnumerable<Portfolio> UserPortfolios { get; set; }
        public List<StockViewModel> AvailableAssetTickers { get; set; }
        public StockViewModel ChosenStock { get; set; }
    }
}
