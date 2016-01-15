using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Models
{
    public class Email
    {
        public string CustomerEmail { get; set; }
        public string ContactEmail { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string ErrorMessage { get; set; }

    }
}
