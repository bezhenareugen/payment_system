using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Bll.Services
{
    public interface IConverterService
    {
        public decimal ConvertedCurrency(string sourceCurrency, string destinationCurrency, decimal transferAmount);       
    }
}
