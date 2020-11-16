﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;

namespace PaymentSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TransferHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public TransferHistoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public PagenetedTransferHistory GetPageneted(string sortDir, string sortBy, int pageNumber, int itemsPerPage)
        {
            var transactionsCount = _context.Transactions.Count();

            decimal pages = transactionsCount / itemsPerPage;
            
            pages = Math.Ceiling(pages);
            

            var userId = _userManager.GetUserId(User);
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            var query = _context.Transactions.Where(x => x.SourceUsername == user.UserName || x.DestinationUsername == user.UserName).AsQueryable();

            switch (sortDir)
            {
                case "asc":
                    switch (sortBy)
                    {
                        case "Amount":
                            query = query.OrderBy(x => x.Amount);
                            break;
                        case "Date":
                            query = query.OrderBy(x => x.Date);
                            break;
                    }
                    break;

                case "desc":
                    switch (sortBy)
                    {
                        case "Amount":
                            query = query.OrderByDescending(x => x.Amount);   
                            break;
                        case "Date":
                            query = query.OrderByDescending(x => x.Date);
                            break;
                    }
                    break;
            }

            var result = query.Skip((pageNumber -1)* itemsPerPage).Take(itemsPerPage).ToList();

            return new PagenetedTransferHistory
            {
                Transactions = result,
                MaxPageNumber = pages,
            };
        }
    }
}
