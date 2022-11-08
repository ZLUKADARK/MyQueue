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
        public Task<TokenOrMailConfirme> Login(Login login);
        public Task<RegistrationSuccsess> Registeration(Registration registration);
        public Task<bool> ConfirmEmail(string userName, string code);
    }
}
