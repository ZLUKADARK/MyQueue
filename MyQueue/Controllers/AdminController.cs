using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.DataTansferObject.Admin;
using MyQueue.DataTansferObject.FoodManipulation;
using MyQueue.Services.AdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminServices _adminServices;
        public AdminController(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }
       

        [HttpPost("Role")]
        public async Task<ActionResult<RoleDTO>> AddRole(RoleDTO role)
        {
            var result = await _adminServices.AddRole(role);
            return result;
        }

        [HttpDelete("Role")]
        public async Task<ActionResult<RoleDTO>> DeleteRole(RoleDTO role)
        {
            var result = await _adminServices.DeleteRole(role);
            if (result == null)
                return BadRequest();
            return result;
        }

        // GET: api/<AdminController>
        [HttpGet("Roles")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRole()
        {
            var result = await _adminServices.GetRole();
            return result.ToList();
        }

        // GET: api/<AdminController>
        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetUsers()
        {
            var result = await _adminServices.GetUsers();
            return result.ToList();
        }

        [HttpDelete("User")]
        public async Task<ActionResult<UsersDTO>> DeleteUser(IDToDelete delete)
        {
            var result = await _adminServices.DeleteUser(delete);
            if (result == null)
                return BadRequest();
            return result;
        }

        [HttpDelete("UserRole")]
        public async Task<ActionResult<UserRoleDTO>> DeleteUserRole(UsersRole delete)
        {
            var result = await _adminServices.DeleteUserRole(delete);
            if (result == null)
                return BadRequest();
            return result;
        }

        [HttpPost("UserRole")]
        public async Task<ActionResult<UserRoleDTO>> AddUserRole(UsersRole add)
        {
            var result = await _adminServices.AddUserRole(add);
            if (result == null)
                return BadRequest();
            return result;
        }
    }
}
