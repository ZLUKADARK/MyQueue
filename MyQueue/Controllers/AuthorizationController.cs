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
        private readonly MQDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;
        public AuthorizationController(MQDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> config)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _options = config.Value;
        }

        // POST api/<AuthorizationController>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                    if (passwordCheck.Succeeded)
                    {
                        var newtoken = new Token();
                        var token = newtoken.GetToken(user, _options);
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
                else
                    return Unauthorized();
            }
            return BadRequest();
        }

        // POST api/<AuthorizationController>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // POST api/AuthorizationController
        [HttpPost("register")]
        public async Task<IActionResult> Registeration(Registration registration)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(registration.Email);
                if (existingUser == null)
                {
                    IdentityUser user = new IdentityUser() { UserName = registration.UserName, Email = registration.Email };
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
