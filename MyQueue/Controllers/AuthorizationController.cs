using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyQueue.DataTansferObject.Authorization;
using MyQueue.Data;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using MyQueue.Services.AuthorizationServices;
using Microsoft.AspNetCore.Authorization;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthorizationServices _authorizationServices;
        private readonly MailConfig _mailOptions;
        public AuthorizationController(SignInManager<IdentityUser> signInManager, AuthorizationServices authorizationServices, IOptions<MailConfig> mailOptions)
        {
            _signInManager = signInManager;
            _authorizationServices = authorizationServices;
            _mailOptions = mailOptions.Value;
        }

        // POST api/Authorization/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationServices.Login(login);
                if (result.Token != null)
                {
                    return Ok( new { Token = result.Token });
                }
                else if (result.Code != null)
                {
                    var callbackUrl = Url.Action(
                                            "ConfirmEmail",
                                            "Authorization",
                                            new { userName = login.Email, code = result.Code },
                                            protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(login.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>", _mailOptions);

                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
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
                var user = await _authorizationServices.Registeration(registration);
                if (user != null)
                {
                    var callbackUrl = Url.Action(
                                            "ConfirmEmail",
                                            "Authorization",
                                            new { userName = user.Email, code = user.Code },
                                            protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(registration.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>", _mailOptions);

                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userName, string code)
        {
            var result = await _authorizationServices.ConfirmEmail(userName, code);
            if (result == true)
                return Ok("Успешно");
            else
                return BadRequest();
        }
    }
}
