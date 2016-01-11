using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class PortfolioAsset : Asset
    {
        public int ID { get; set; }

        [ForeignKey("Portfolio")]
        public int PortfolioID { get; set; }

        [ForeignKey("Asset")] //not actually in DB
        public  string AssetSymbol { get; set; }

        public decimal AveragePurchasePrice { get; set; }

        public int NumberOfAsset { get; set; }


        //stock, bond, etc.

        string AssetType { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
