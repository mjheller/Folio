using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class Portfolio
    {
        public int ID { get; set; }
        [Display(Name = "Portfolio Name:")]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<PortfolioAsset> PortfolioAssets { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
