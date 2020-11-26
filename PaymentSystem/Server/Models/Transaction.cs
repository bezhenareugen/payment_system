using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Models
{
    public class Transaction
    {   
        public Guid Id { get; set; }
   
        public string SourceUsername { get; set; }
        public string DestinationUsername { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
