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
using PaymentSystem.Server.Application.TransferHistories.Queres;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using PaymentSystem.Shared;
using Transaction = PaymentSystem.Server.Models.Transaction;

namespace PaymentSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TransferHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        
        public TransferHistoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PagenetedTransferHistory> GetPageneted(string sortDir, string sortBy, int pageNumber, int itemsPerPage)
        {
            var transactionsCount = _context.Transactions.Count();

            decimal pages = transactionsCount / itemsPerPage;

            pages = Math.Ceiling(pages);


            var transfer = new GetTransferHistoriesQuery
            {
                UserId = _userManager.GetUserId(User),
                SortBy = sortBy,
                SortDir = sortDir,
                MaxPageNumbers = pages,
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
            };

            var transferHistory = _mediator.Send(transfer);

            return await transferHistory;
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

            if (destinationWallet == null)
            {
                destinationWallet = new Models.Wallet
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
    }
}
