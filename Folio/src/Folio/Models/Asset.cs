using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class Asset
    {
        public string Name { get; set; }
        [Key]
        public string Symbol { get; set; }
    }
}
