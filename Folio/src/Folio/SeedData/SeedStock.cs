using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Folio.Models;

namespace Folio.SeedData
{
    public class SeedStock : Asset
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
    }
}
