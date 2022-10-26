using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyQueue.DataTansferObject.Authorization;
using MyQueue.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyQueue.DataTansferObject.FoodManipulation;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly MQDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorizationController(MQDBContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // POST api/<AuthorizationController>


        // POST api/AuthorizationController
        [HttpPost("register")]
        public async Task<IActionResult> Registeration( Registration registration)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(registration.Email);
                if (existingUser == null)
                {
                    IdentityUser user = new IdentityUser();
                    user.UserName = registration.UserName;
                    user.Email = registration.Email;

                    IdentityResult result = _userManager.CreateAsync(user, registration.Password).Result;

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        return Created("", registration);
                    }
                }

            }

            return BadRequest();
        }
        
       
    }
}
