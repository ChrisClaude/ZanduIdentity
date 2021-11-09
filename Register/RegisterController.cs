using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZanduIdentity.Models;

namespace ZanduIdentity.Register
{
    [Route("account/register")]
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View("Register");
        }
        
        /// <summary>
        /// Handle form submission for user registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(RegisterInputModel inputModel)
        {
            _logger.LogInformation($"email: {inputModel.Email}");
            return Redirect("/");
        }

    }
}