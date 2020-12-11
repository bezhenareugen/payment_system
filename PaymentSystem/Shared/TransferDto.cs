using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Shared
{
   public class TransferDto
    {
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
    }
}
