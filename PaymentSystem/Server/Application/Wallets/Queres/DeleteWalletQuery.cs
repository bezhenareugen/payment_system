using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Application.Wallets.Queres
{
    public class DeleteWalletQuery :IRequest<DeleteWalletResult>
    {
        public string UserId { get; set; }
        public Guid WalletId { get; set; }
    }


    public class DeleteWalletResult
    {
        public bool IsSuccessful { get; set; }

        public static DeleteWalletResult ReturnSuccess()
        {
            return new DeleteWalletResult { IsSuccessful = true };
        }

        public static DeleteWalletResult ReturnFailure()
        {
            return new DeleteWalletResult { IsSuccessful = false };
        }
    }


    public class DeleteWalletQueryHandler : IRequestHandler<DeleteWalletQuery, DeleteWalletResult>
    {
        private readonly ApplicationDbContext _context;

        public DeleteWalletQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DeleteWalletResult> Handle(DeleteWalletQuery query, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(w => w.Wallets)
                .FirstOrDefaultAsync(w => w.Id == query.UserId);

            var deleteWallet = user.Wallets.FirstOrDefault(w => w.Id == query.WalletId);
         

            if(deleteWallet == null)
            {
                return DeleteWalletResult.ReturnFailure();
            }

            _context.Remove(deleteWallet);
            _context.SaveChanges();

            return DeleteWalletResult.ReturnSuccess();
        }
    }
}
