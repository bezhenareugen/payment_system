using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using PaymentSystem.Server.Models;
using PaymentSystem.Server.Data;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace PaymentSystem.Server.Application.TransferHistories.Queres
{
    public class GetTransferHistoriesQuery : IRequest<PagenetedTransferHistory>
    {
        public string UserId { get; set; }
        public string SortDir { get; set; }
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }

    public class GetTransferHistoryHandler : IRequestHandler<GetTransferHistoriesQuery, PagenetedTransferHistory>
    {
        private readonly ApplicationDbContext _context;

        public GetTransferHistoryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagenetedTransferHistory> Handle(GetTransferHistoriesQuery query, CancellationToken cancelletionToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == query.UserId);

            var transactions = _context.Transactions
                .Where(x => x.SourceUsername == user.UserName || x.DestinationUsername == user.UserName);

            var transactionsCount = transactions.Count();
            decimal pages = transactionsCount / query.ItemsPerPage;

            var history = transactions.AsQueryable();
                

            if (history == null)
            {
                throw new Exception();
            }

            switch (query.SortDir)
            {
                case "asc":
                    switch (query.SortBy)
                    {
                        case "Amount":
                            history = history.OrderBy(x => x.Amount);
                            break;
                        case "Date":
                            history = history.OrderBy(x => x.Date);
                            break;
                    }
                    break;

                case "desc":
                    switch (query.SortBy)
                    {
                        case "Amount":
                            history = history.OrderByDescending(x => x.Amount);
                            break;
                        case "Date":
                            history = history.OrderByDescending(x => x.Date);
                            break;
                    }
                    break;
            }

            var result = history.Skip((query.PageNumber - 1) * query.ItemsPerPage).Take(query.ItemsPerPage).ToList();

            return new PagenetedTransferHistory
            {
                Transactions = result,
                MaxPageNumber = pages,
            };
        }
    }
}
