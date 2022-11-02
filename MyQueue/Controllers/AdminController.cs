using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.DataTansferObject.Admin;
using MyQueue.DataTansferObject.FoodManipulation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MQDBContext _context;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, MQDBContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
       

        [HttpPost("Role")]
        public async Task<ActionResult<RoleDTO>> AddRole(RoleDTO role)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(role.Name));
            }
            var result = _roleManager.FindByNameAsync(role.Name);
            return new RoleDTO { Name = result.Result.Name, Id = result.Result.Id };
        }

        [HttpDelete("Role")]
        public async Task<ActionResult<RoleDTO>> DeleteRole(RoleDTO role)
        {

            if (await _roleManager.RoleExistsAsync(_roleManager.FindByIdAsync(role.Id).Result.Name))
            {
                var result = _roleManager.FindByIdAsync(role.Id);
                await _roleManager.DeleteAsync(_roleManager.FindByIdAsync(role.Id).Result);
                return new RoleDTO { Name = result.Result.Name, Id = result.Result.Id };
            }
            return BadRequest();
        }

        // GET: api/<AdminController>
        [HttpGet("Roles")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRole()
        {
            var result = from r in _roleManager.Roles select new RoleDTO { Id = r.Id, Name = r.Name };
            return await result.ToListAsync();
        }

        // GET: api/<AdminController>
        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetUsers()
        {
            var user = await _userManager.Users.AsNoTracking().ToListAsync();
                         
            var result = from u in user
                         select new UsersDTO
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             Phone = u.PhoneNumber,
                             UserRole =  _userManager.GetRolesAsync(u).Result.ToList()
                         };
            
            return result.ToList();
        }

        [HttpDelete("User")]
        public async Task<ActionResult<UsersDTO>> DeleteUser(IDToDelete delete)
        {
            var user = await _userManager.FindByIdAsync(delete.Id);
            if (user != null)
            {
                var result = new UsersDTO(){ Id = user.Id,UserName = user.UserName, Email = user.Email, Phone = user.PhoneNumber, UserRole = _userManager.GetRolesAsync(user).Result.ToList() };
                await _userManager.DeleteAsync(user);
                return result;
            }
            return BadRequest();
        }

        [HttpDelete("UserRole")]
        public async Task<ActionResult<UserRoleDTO>> DeleteUserRole(UsersRole delete)
        {
            var user = await _userManager.FindByIdAsync(delete.UserId);
            if (user != null)
            {
                var role = await _roleManager.FindByIdAsync(delete.RoleId);
                if(role != null)
                {
                    var result = new UserRoleDTO { Id = role.Id, Name = user.UserName, RoleName = role.Name };
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                    return result;
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("UserRole")]
        public async Task<ActionResult<UserRoleDTO>> AddUserRole(UsersRole add)
        {
            var user = await _userManager.FindByIdAsync(add.UserId);
            if (user != null)
            {
                var role = await _roleManager.FindByIdAsync(add.RoleId);
                if (role != null)
                {
                    var result = new UserRoleDTO { Id = role.Id, Name = user.UserName, RoleName = role.Name };
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return result;
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
