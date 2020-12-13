using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;

namespace PaymentSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExchangeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExchangeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<PrivateBankCurrencyApi> GetCurrenciesApi()
        {
            var currencyList = _context.PrivateBankCurrencies.ToList();

            var skipedCurrencylist = currencyList.Skip(currencyList.Count - 4).ToList();

            return skipedCurrencylist;


        }
    }
}
