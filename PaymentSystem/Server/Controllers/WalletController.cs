using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Application.Wallets.Commands;
using PaymentSystem.Server.Application.Wallets.Queres;
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
        private readonly IMediator _mediator;
        public WalletController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("walletcurrency")]
        public List<string> GetWalletCurrencies()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.Id == userId);
            var walletcurrency = user.Wallets.Select(w => w.Currency).ToList();

            return walletcurrency;        
        }


        [HttpGet]
        public async Task<List<Wallet>> GetWallets()
        {
            var query = new GetWalletsQuery
            {
                UserId = _userManager.GetUserId(User)
            };

           var wallets =  await _mediator.Send(query);

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
        public async Task<IActionResult> CreateWallet([FromBody] string currency)
        {
            var createWalletCommand = new CreateWalletCommand
            {
                UserId = _userManager.GetUserId(User),
                Currency = currency,
            };

            var createWalletResult = await _mediator.Send(createWalletCommand);

            if (!createWalletResult.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }

      
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteWallet(Guid id)
        {
            var query = new DeleteWalletQuery
            {
                UserId = _userManager.GetUserId(User),
                WalletId = id,   
            };

            var createDeleteResult = await _mediator.Send(query);

            if(!createDeleteResult.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
