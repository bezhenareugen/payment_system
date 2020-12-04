using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Server.Application.Transfers.Commands;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using PaymentSystem.Shared;

namespace PaymentSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public TransferController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

       
        [HttpPost]
        public async Task<IActionResult> MakeTranfer([FromBody] TransferDto data)
        {
            var createTransferCommand = new CreateTransferCommand
            {
                UserId = _userManager.GetUserId(User),
                UserName = data.UserName,
                WalletId = data.Id,
                Currency = data.Currency,
                DestinationCurrency = data.DestinationCurrency,
                Amount = data.Amount,
            };

            var createTransferResult = await _mediator.Send(createTransferCommand);

            if (!createTransferResult.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
