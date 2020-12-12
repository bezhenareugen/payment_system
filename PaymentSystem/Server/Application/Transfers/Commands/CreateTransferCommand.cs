using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using PaymentSystem.Server.Services.ConverterOfCurrencyService;
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
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
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
        private readonly IConverterOfCurrency _converterOfCurrency;

        public CreateTransferCommandHandler(ApplicationDbContext context, IConverterOfCurrency converterOfCurrency)
        {
            _context = context;
            _converterOfCurrency = converterOfCurrency;
        }

        public async Task<CreateTransferResult> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
        {
            if(command.UserName == null)
            {
                throw new ResultFailedException();
            }

            var user = await _context.Users.Include(u => u.Wallets).FirstOrDefaultAsync(u => u.Id == command.UserId);

            if (!user.Wallets.Any(w => w.Currency == command.SourceCurrency))
            {
                return CreateTransferResult.ReturnFailure();
            }

            var source =  user.Wallets.FirstOrDefault(w => w.Currency == command.SourceCurrency);

            if (source.Amount < command.Amount)
            {
                return CreateTransferResult.ReturnFailure();
            }

            if (command.DestinationCurrency != null)
            {

                var userDestinationOWnWallet = user.Wallets.FirstOrDefault(w => w.Currency == command.DestinationCurrency);

                if (userDestinationOWnWallet == null)
                {
                    throw new Exception();
                }


                var convertedAmount = _converterOfCurrency.ConvertCurrency(command.SourceCurrency, command.DestinationCurrency, command.Amount);

                source.Amount -= command.Amount;
                userDestinationOWnWallet.Amount += convertedAmount;

                var transaction = new Transaction
                {
                    SourceUsername = user.UserName,
                    DestinationUsername = command.UserName,
                    Currency = command.SourceCurrency,
                    Amount = command.Amount,
                    Date = DateTime.Now,
                };
                _context.Add(transaction);

            }
            else
            {
                var destinationUser = _context.Users.Include(w => w.Wallets).FirstOrDefault(u => u.UserName == command.UserName);

                if (destinationUser == null)
                {
                    throw new ResultFailedException();
                }



                var destinationWallet = destinationUser.Wallets.FirstOrDefault(w => w.Currency == command.SourceCurrency);


                if (destinationWallet == null)
                {
                    destinationWallet = new Models.Wallet
                    {
                        Amount = 0,
                        Currency = command.SourceCurrency,
                    };

                    destinationUser.Wallets.Add(destinationWallet);
                }

                source.Amount -= command.Amount;
                destinationWallet.Amount += command.Amount;

                var transaction = new Transaction
                {
                    SourceUsername = user.UserName,
                    DestinationUsername = destinationUser.UserName,
                    Currency = command.SourceCurrency,
                    Amount = command.Amount,
                    Date = DateTime.Now,
                };

                _context.Add(transaction);
            }
            
            _context.SaveChanges();

            return CreateTransferResult.ReturnSuccess();
        }   
    }
}
