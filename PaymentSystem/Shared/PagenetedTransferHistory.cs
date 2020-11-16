using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Shared
{
   public  class PagenetedTransferHistory
    {
        public List<Transaction> Transactions { get; set; }
        public decimal MaxPageNumber { get; set; }
    }
}  
