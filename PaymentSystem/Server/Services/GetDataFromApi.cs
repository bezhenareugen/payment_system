using Newtonsoft.Json;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Services
{
    public class GetDataFromApi : IGetDataFromApi
    {
        private readonly ApplicationDbContext _context;
        
        public GetDataFromApi(ApplicationDbContext context)
        {
            _context = context;
        }

      public async Task GetData()
       {          
            var client = new HttpClient();

            HttpResponseMessage result = await client.GetAsync("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");

            var response = result.Content.ReadAsStringAsync();

            var parsedResponse = JsonConvert.DeserializeObject<List<PrivateBankCurrencyApi>>(response.Result);

            var entity = parsedResponse.Select(x => new PrivateBankCurrencyApi
            {
               Ccy = x.Ccy,
               Base_Ccy = x.Base_Ccy,
               Buy = Convert.ToDecimal(x.Buy),
               Sale = Convert.ToDecimal(x.Sale),
            });

            _context.AddRange(entity);
            _context.SaveChanges();
        }
    }
}
