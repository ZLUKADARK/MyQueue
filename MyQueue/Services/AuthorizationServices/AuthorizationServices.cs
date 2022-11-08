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

        public AuthorizationServices(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value;
            _options = options.Value;
        }

        public async Task<bool> ConfirmEmail(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return false;
            }
            var user = await _userManager.FindByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return true;
            else
                return false;
        }

        public async Task<TokenOrMailConfirme> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null)
            {
                if (user.EmailConfirmed == true)
                {
                    var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                    if (passwordCheck.Succeeded)
                    {
                        var newtoken = new Token();
                        var token = newtoken.GetToken(user, _options);
                        return new TokenOrMailConfirme { Token = new JwtSecurityTokenHandler().WriteToken(token) };
                    }
                    return null;
                }
                else
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return new TokenOrMailConfirme { Code = code };
                }
            }
            return null;
        }

        public async Task<RegistrationSuccsess> Registeration(Registration registration)
        {
            var existingUser = await _userManager.FindByEmailAsync(registration.Email);
            if (existingUser == null)
            {
                IdentityUser user = new IdentityUser() { UserName = registration.UserName, Email = registration.Email };
                IdentityResult result = _userManager.CreateAsync(user, registration.Password).Result;
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return new RegistrationSuccsess { Email = registration.Email, Password = registration.Password, UserName = registration.UserName, Code = code };
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
