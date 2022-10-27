using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyQueue.DataTansferObject.Authorization;
using MyQueue.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyQueue.DataTansferObject.FoodManipulation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly MQDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
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
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (passwordCheck.Succeeded)
                    {

                        var claims = new List<Claim>()
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: _options.Issuer,
                            audience: _options.Audience,
                            claims: claims,
                            expires: DateTime.UtcNow.AddHours(12),
                            notBefore: DateTime.UtcNow,
                            signingCredentials: credentials
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }

            return BadRequest();
        }

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
