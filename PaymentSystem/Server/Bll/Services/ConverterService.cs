using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PaymentSystem.Shared;
using System.Net.Http.Json;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PaymentSystem.Shared.CurrencyApi;

namespace PaymentSystem.Server.Bll.Services
{
    public class ConverterService : IConverterService
    {
        private decimal convertedAmount;
        private decimal buyUsd, buyEur;
        private Task<string> response;
        string buyResult;

   

        public async Task<decimal> ConvertedCurrencyAsync(string sourceCurrency, string destinationCurrency, decimal transferAmount)
        {
            var client = new HttpClient();

            HttpResponseMessage result = await client.GetAsync("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");

            response = result.Content.ReadAsStringAsync();

            List<PrivatBankCurrencies> parsedResponse = JsonConvert.DeserializeObject<List<PrivatBankCurrencies>>(response.Result);

            var buyUsd = parsedResponse.Find(x => x.Ccy == "USD");

            
            switch (sourceCurrency)
            {
                case "USD":
                    switch (destinationCurrency)
                    {
                        case "EUR":
                            convertedAmount = transferAmount * Convert.ToDecimal(buyUsd.Buy) / 1;
                            break;
                        case "MDL":
                            convertedAmount = transferAmount * Convert.ToDecimal(buyUsd.Buy);
                            break;
                    }
                    break;
                case "EUR":
                    switch (destinationCurrency)
                    {
                        case "USD":
                            convertedAmount = transferAmount * Convert.ToDecimal(buyUsd.Buy) / 1;
                            break;
                        case "MDL":
                            convertedAmount = transferAmount * 1;
                            break;
                    }
                    break;
                case "MDL":
                    switch (destinationCurrency)
                    {
                        case "EUR":
                            convertedAmount = transferAmount / 1;
                            break;
                        case "USD":
                            convertedAmount = transferAmount / 1;
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
