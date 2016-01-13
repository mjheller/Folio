using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Folio.Models
{
    public class Portfolio
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Portfolio Name:")]
        public string Name { get; set; }
        [Display(Name = "Date Created:")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public virtual ICollection<PortfolioAsset> PortfolioAssets { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
