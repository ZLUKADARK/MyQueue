using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyQueue.DataTansferObject.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MyQueue.Services.AuthorizationServices
{
    public class Token
    {
        public JwtSecurityToken GetToken(IdentityUser user, JWTSettings _options)
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
            return token;
        }
    }
}
