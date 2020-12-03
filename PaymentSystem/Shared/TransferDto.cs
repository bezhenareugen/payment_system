using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Shared
{
   public class TransferDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
