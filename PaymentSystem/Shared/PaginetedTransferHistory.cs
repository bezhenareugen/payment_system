using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Shared
{
   public  class PaginetedTransferHistory
    {
        public List<Transaction> Transactions { get; set; }
        public decimal MaxPageNumber { get; set; }
        public int TransactionsCount { get; set; }
    }
}  
