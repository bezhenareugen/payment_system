using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Application.Wallets.Queres
{
    public class GetWalletsQuery : IRequest<List<Wallet>>
    {
        public string UserId { get; set; }
    }

    public class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, List<Wallet>>
    {
        private readonly ApplicationDbContext _context;

        public GetWalletsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<List<Wallet>> Handle(GetWalletsQuery query, CancellationToken cancellationToken)
        {
            var userWithWallets = await _context.Users
                .Include(w => w.Wallets)
                .FirstOrDefaultAsync(u => u.Id == query.UserId);

            return userWithWallets.Wallets;
        }

       
    }
}
