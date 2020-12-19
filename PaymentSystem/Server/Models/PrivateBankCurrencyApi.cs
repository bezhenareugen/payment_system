using System;


namespace PaymentSystem.Server.Models
{
    public class PrivateBankCurrencyApi
    {
        public int Id { get; set; }
        public string Ccy { get; set; }
        public string Base_Ccy { get; set; }
        public decimal Buy { get; set; }
        public decimal Sale { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
