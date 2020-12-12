using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Server.Data;
using PaymentSystem.Server.Models;
using PaymentSystem.Shared;

namespace PaymentSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

       public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        

        [HttpGet]
        [Route("{username}/validate")]
        public UserValidationResult ValidateUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            return new UserValidationResult
            {
                Exists = user != null
            }; 
        }       
    }
}
