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
            var transfer = new GetTransferHistoriesQuery
            {
                UserId = _userManager.GetUserId(User),
                SortBy = sortBy,
                SortDir = sortDir,          
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
            };

            var transferHistory = await _mediator.Send(transfer);

            return transferHistory;
        }
    }
}
