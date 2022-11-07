using MyQueue.DataTansferObject.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.AuthorizationServices
{
    interface IAuthorizationServices
    {
        public Task<string> Login(Login login);
        public Task<Registration> Registeration(Registration registration);
    }
}
