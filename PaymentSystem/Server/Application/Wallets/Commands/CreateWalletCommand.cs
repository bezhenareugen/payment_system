using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Application.Wallets.Commands
{
    public class CreateWalletCommand : IRequest<CreateWalletResult>
    {
        public string UserId { get; set; }
        public string Currency { get; set; }
    }

    public class CreateWalletResult
    {
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; }

        public static CreateWalletResult ReturnSuccess()
        {
            return new CreateWalletResult { IsSuccessful = true };
        }
        
        public static CreateWalletResult ReturnFailure(string failureReasone)
        {
            return new CreateWalletResult
            {
                IsSuccessful = false,
                FailureReason = failureReasone,
            };
        }
    }


    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletResult>
    {
        private readonly ApplicationDbContext _context;

        public CreateWalletCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateWalletResult> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
        {
            if (!CurrencyManager.Currencies.Contains(command.Currency))
            {
                return CreateWalletResult.ReturnFailure("INVALID_CURRENCY");
            }

            var user = await _context.Users.Include(w => w.Wallets).FirstOrDefaultAsync(u => u.Id == command.UserId);

            if (user.Wallets.Any(w => w.Currency == command.Currency))
            {
                return CreateWalletResult.ReturnFailure("WALLET_EXISTS");
            }

            var wallet = new Wallet
            {
                Amount = 0,
                Currency = command.Currency,
            };

            if (user.Wallets == null)
            {
                user.Wallets = new List<Wallet>();
            }

            user.Wallets.Add(wallet);
            _context.SaveChanges();

            return CreateWalletResult.ReturnSuccess();
        }
    }
}
