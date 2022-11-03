using MyQueue.DataTansferObject.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.AuthorizationServices
{
    public class AuthorizationServices : IAuthorizationServices
    {
        public Task<JwtSecurityTokenHandler> Login(Login login)
        {
            throw new NotImplementedException();
        }

        public Task<Registration> Registeration(Registration registration)
        {
            throw new NotImplementedException();
        }
    }
}
