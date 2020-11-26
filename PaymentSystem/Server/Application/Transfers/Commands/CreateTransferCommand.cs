using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Application.Transfers.Commands
{
    public class CreateTransferCommand : IRequest<CreateTransferResult>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateTransferResult
    {
        public bool IsSuccessful { get; set; }

        public static CreateTransferResult ReturnSuccess()
        {
            return new CreateTransferResult { IsSuccessful = true };
        }

        public static CreateTransferResult ReturnFailure()
        {
            return new CreateTransferResult { IsSuccessful = false };
        }
    }

    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, CreateTransferResult>
    {
        private readonly ApplicationDbContext _context;

        public CreateTransferCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateTransferResult> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Wallets).FirstOrDefaultAsync(u => u.Id == command.UserId);

            if (!user.Wallets.Any(w => w.Currency == command.Currency))
            {
                return CreateTransferResult.ReturnFailure();
            }

            var source =  user.Wallets.FirstOrDefault(w => w.Currency == command.Currency);

            if (source.Amount < command.Amount)
            {
                return CreateTransferResult.ReturnFailure();
            }

            var destinationUser = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.UserName == command.UserName);

            var destinationWallet = destinationUser.Wallets.FirstOrDefault(w => w.Currency == command.Currency);

            if (destinationWallet == null)
            {
                destinationWallet = new Models.Wallet
                {
                    Amount = 0,
                    Currency = command.Currency,
                };

                destinationUser.Wallets.Add(destinationWallet);
            }

            source.Amount -= command.Amount;
            destinationWallet.Amount += command.Amount;


            var transaction = new Transaction
            {
                SourceUsername = user.UserName,
                DestinationUsername = destinationUser.UserName,    
                Currency = command.Currency,
                Amount = command.Amount,
                Date = DateTime.Now,
            };
            _context.Add(transaction);
            _context.SaveChanges();

            return CreateTransferResult.ReturnSuccess();
        }   
    }
}
