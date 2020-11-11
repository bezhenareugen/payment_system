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
        public void CreateWallet([FromBody] string currency)
        {
            var userId = _userManager.GetUserId(User);

            var wallet = new Wallet
            {
                Amount = 0,
                Currency = currency,
            };

            _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId).Wallets.Add(wallet);
            _context.SaveChanges();
        }

     /*TODO: Post request*/
     

        [HttpDelete]
        [Route("{id}")]
        public void DeleteWallet(Guid id)
        {
             var userId = _userManager.GetUserId(User);

              var delWallet = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId).Wallets.FirstOrDefault(w => w.Id == id);

          //  var delWallet = _context.Wallets.FirstOrDefault(w => w.Id == id);

            _context.Remove(delWallet);
            _context.SaveChanges();
        }

    }
}
