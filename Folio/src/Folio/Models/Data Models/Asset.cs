using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    interface Asset
    {
        string Name { get; set; }
        [Key]
        string Symbol { get; set; }
    }
}
