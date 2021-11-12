using System;
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

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> UpdateUserAsync(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var storedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (storedUser != null)
            {
                return NotFound();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if ((await _context.Users.FindAsync(id)) == null)
                {
                    return NotFound();
                }
            }
            
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ApplicationUser>> CreateUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetUserAsync), new {Id = user.Id}, user);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApplicationUser>> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}