using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public abstract class Asset
    {
        public string Name { get; protected set; }

        [Key]
        public string Symbol { get; protected set; }
    }
}
