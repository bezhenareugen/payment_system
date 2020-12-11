using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Models
{
    public class PrivateBankCurrencyApi
    {
        public int Id { get; set; }
        public string Ccy { get; set; }
        public string Base_Ccy { get; set; }
        public string Buy { get; set; }
        public string Sale { get; set; }
    }
}
