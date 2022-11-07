using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyQueue.Data;
using MyQueue.DataTansferObject.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MyQueue.Services.AuthorizationServices
{
    public class AuthorizationServices : IAuthorizationServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;
        public AuthorizationServices(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = config.Value;
        }
        public async Task<string> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null)
            {
                var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                if (passwordCheck.Succeeded)
                {
                    var newtoken = new Token();
                    var token = newtoken.GetToken(user, _options);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                    return null;
            }
            else
                return null;
        }

        public async Task<Registration> Registeration(Registration registration)
        {
            var existingUser = await _userManager.FindByEmailAsync(registration.Email);
            if (existingUser == null)
            {
                IdentityUser user = new IdentityUser() { UserName = registration.UserName, Email = registration.Email };
                IdentityResult result = _userManager.CreateAsync(user, registration.Password).Result;
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return registration;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
