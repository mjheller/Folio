using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//used for the quote builder
namespace Folio.Models
{
    public class Stocks : IEnumerable<Quote>
    {
        public string[] companies;
        public List<Quote> stocks = new List<Quote>();
        public Quote this[int index]
        {
            get { return stocks[index]; }
            set { stocks.Insert(index, value); }
        }

        public IEnumerator<Quote> GetEnumerator()
        {
            return stocks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
