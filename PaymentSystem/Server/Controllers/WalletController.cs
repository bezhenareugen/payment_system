using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using PaymentSystem.Shared;
using Transaction = PaymentSystem.Server.Models.Transaction;
using Wallet = PaymentSystem.Server.Models.Wallet;

namespace PaymentSystem.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public WalletController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public List<Wallet> GetWallets()
        {
            var userId = _userManager.GetUserId(User);
            var wallets = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId).Wallets;

            return wallets;
        }

        [HttpGet]
        [Route("{id}")]
        public Wallet GetWallet(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            var wallet = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId).Wallets.FirstOrDefault(w => w.Id == id);
           
            return wallet;

        }

        [HttpPost]
        public IActionResult CreateWallet([FromBody] string currency)
        {
            if(!CurrencyManager.Currencies.Contains(currency))
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var user = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId);

            if(user.Wallets.Any(w => w.Currency == currency))
            {
                return BadRequest();
            }

            var wallet = new Wallet
            {
                Amount = 0,
                Currency = currency,
            };

            if(user.Wallets == null)
            {
                user.Wallets = new List<Wallet>();
            }

            user.Wallets.Add(wallet);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("transfer")]
        public ActionResult MakeTranfer([FromBody] TransferDto data)
        {
            var userId = _userManager.GetUserId(User);

            var user = _context.Users.Include(u => u.Wallets).FirstOrDefault(u => u.Id == userId);

            if (!user.Wallets.Any(w => w.Currency == data.Currency))
            {
                return BadRequest();
            }

            var source = user.Wallets.FirstOrDefault(w => w.Currency == data.Currency);

            if (source.Amount < data.Amount)
            {
                return BadRequest();
            }

            var destinationUser = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.UserName == data.UserName);

            var destinationWallet = destinationUser.Wallets.FirstOrDefault(w => w.Currency == data.Currency);

            if(destinationWallet == null)
            {
                 destinationWallet  = new Wallet
                {
                    Amount = 0,
                    Currency = data.Currency,
                };

                destinationUser.Wallets.Add(destinationWallet);
            }

            source.Amount -= data.Amount;
            destinationWallet.Amount += data.Amount;
            

            var transaction = new Transaction
            {
                SourceUsername = user.UserName,
                DestinationUsername = destinationUser.UserName,
                Amount = data.Amount,
                Date = DateTime.Now,
            };
            _context.Add(transaction);
            _context.SaveChanges();

            return Ok();
        }
     
        [HttpDelete]
        [Route("{id}")]
        public void DeleteWallet(Guid id)
        {
             var userId = _userManager.GetUserId(User);

             var delWallet = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId).Wallets.FirstOrDefault(w => w.Id == id);

            _context.Remove(delWallet);
            _context.SaveChanges();  
        }
    }
}
