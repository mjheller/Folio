using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels.Portfolios
{
    public class DeleteStockFromPortfolioViewModel
    {
        public Portfolio WorkingPortfolio { get; set; }
        public List<Portfolio> UserPortfolios { get; set; }
    }
}
