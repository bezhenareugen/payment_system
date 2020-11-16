using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Models
{
    public class PagenetedTransferHistory
    {
        public List<Transaction> Transactions { get; set; }
        public decimal MaxPageNumber { get; set; }
    }
}
