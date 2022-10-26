using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.DataTansferObject.FoodManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [HttpGet("Role")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> Get()
        {
            var result = from r in _roleManager.Roles select new RoleDTO { Id = r.Id, Name = r.Name };
            return await result.ToListAsync();
        }

        // GET api/<AdminController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AdminController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<AdminController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
