using PaymentSystem.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Services.ConverterOfCurrencyService
{
    public class ConverterOfCurrency : IConverterOfCurrency
    {
        private readonly ApplicationDbContext _context;

        public ConverterOfCurrency(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ConvertCurrency(string sourceCurrency, string destCurrency, decimal amount)
        {
            var bankCurrency = _context.PrivateBankCurrencies.ToList();

            var usd = bankCurrency.Find(x => x.Ccy == "USD");
            var eur = bankCurrency.Find(x => x.Ccy == "EUR");
            var rur = bankCurrency.Find(x => x.Ccy == "RUR");
            var btc = bankCurrency.Find(x => x.Ccy == "BTC");

            decimal convertedAmount = 0;

            switch (sourceCurrency)
            {
                case "USD":
                    switch (destCurrency)
                    {
                        case "EUR":
                            convertedAmount = amount / usd.Buy * eur.Sale;
                            break;
                        case "RUR":
                            convertedAmount = amount / usd.Buy * eur.Sale;
                            break;
                        case "BTC":
                            convertedAmount = amount * btc.Sale;
                            break;
                        case "UAH":
                            convertedAmount = amount * usd.Buy;
                            break;
                        case "USD":
                            convertedAmount = amount;
                            break;
                        default:
                            break;
                    }
                    break;

                case "EUR":
                    switch (destCurrency)
                    {
                        case "USD":
                            convertedAmount = amount / eur.Buy * usd.Sale;
                            break;
                        case "RUR":
                            convertedAmount = amount / eur.Buy * rur.Sale;
                            break;
                        case "BTC":
                            convertedAmount = amount / eur.Buy * usd.Sale *  btc.Sale;
                            break;
                        case "UAH":
                            convertedAmount = amount * eur.Buy;
                            break;
                        case "EUR":
                            convertedAmount = amount;
                            break;
                        default:
                            break;
                    }
                    break;

                case "RUR":
                    switch (destCurrency)
                    {
                        case "EUR":
                            convertedAmount = amount / rur.Buy * eur.Sale;
                            break;
                        case "RUR":
                            convertedAmount = amount;
                            break;
                        case "BTC":
                            convertedAmount = amount / rur.Buy * usd.Sale * btc.Sale;
                            break;
                        case "UAH":
                            convertedAmount = amount * rur.Buy;

                            break;
                        case "USD":
                            convertedAmount = amount / rur.Buy * usd.Sale;
                            break;
                        default:
                            break;
                    }
                    break;

                case "BTC":
                    switch (destCurrency)
                    {
                        case "USD":
                            convertedAmount = amount * btc.Sale;
                            break;
                        case "RUR":
                            convertedAmount = amount / btc.Sale * usd.Buy * rur.Sale;
                            break;
                        case "BTC":
                            convertedAmount = amount;
                            break;
                        case "UAH":
                            convertedAmount = amount * btc.Sale * usd.Buy;
                            break;
                        case "EUR":
                            convertedAmount = amount / btc.Sale * usd.Buy * eur.Sale;
                            break;
                        default:
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
