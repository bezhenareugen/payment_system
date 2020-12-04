using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PaymentSystem.Server.Models;
using System.Net.Http.Json;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace PaymentSystem.Server.Bll.Services
{
    public class ConverterService : IConverterService
    {
        private decimal convertedAmount;
  
        public decimal ConvertedCurrency(string sourceCurrency, string destinationCurrency, decimal transferAmount)
        {
            switch (sourceCurrency)
            {
                case "USD":
                    switch (destinationCurrency)
                    {
                        case "EUR":
                            convertedAmount = transferAmount * 18 /20;
                            break;
                        case "MDL":
                            convertedAmount = transferAmount * 18;
                            break;
                    }
                    break;
                case "EUR":
                    switch (destinationCurrency)
                    {
                        case "USD":
                            convertedAmount = transferAmount * 20 / 18;
                            break;
                        case "MDL":
                            convertedAmount = transferAmount * 20;
                            break;
                    }
                    break;
                case "MDL":
                    switch (destinationCurrency)
                    {
                        case "EUR":
                            convertedAmount = transferAmount / 20;
                            break;
                        case "USD":
                            convertedAmount = transferAmount / 18;
                            break;
                    }
                    break;
                default:
                    break;
            }

            return convertedAmount;
        }
    }
}
