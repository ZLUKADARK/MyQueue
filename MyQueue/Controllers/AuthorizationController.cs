using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyQueue.DataTansferObject.Authorization;
using MyQueue.Data;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using MyQueue.Services.AuthorizationServices;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthorizationServices _authorizationServices;
        public AuthorizationController(SignInManager<IdentityUser> signInManager, AuthorizationServices authorizationServices)
        {
            _signInManager = signInManager;
            _authorizationServices = authorizationServices;
        }

        // POST api/Authorization/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationServices.Login(login);
                if (result != null)
                {
                    return Ok( new { Token = result });
                }
                else
                    return Unauthorized();
            }
            return BadRequest();
        }

        // POST api/Authorization/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // POST api/Authorization/Register
        [HttpPost("register")]
        public async Task<IActionResult> Registeration(Registration registration)
        {
            if (ModelState.IsValid)
            {
                return Created("", await _authorizationServices.Registeration(registration));
            }
            return BadRequest();
        }
    }
}
