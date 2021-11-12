using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZanduIdentity.Data;
using ZanduIdentity.Models;

namespace ZanduIdentity.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsersAsync()
        {
            _logger.LogInformation("Get all users");
            
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUserAsync(string id)
        {
            _logger.LogInformation("Get all users");
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return Ok(user);
        }
    }
}