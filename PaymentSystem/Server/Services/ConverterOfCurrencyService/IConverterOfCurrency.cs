using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Services.ConverterOfCurrencyService
{
    public interface IConverterOfCurrency
    {
        public decimal ConvertCurrency(string sourceCurrency, string destCurrency, decimal amount);
    }
}
