using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.Data.Models;
using MyQueue.DataTansferObject.Order;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MQDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public OrdersController(MQDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(AddOrderDTO orderDTO)
        {
            ResultOrderDTO result = new ResultOrderDTO();
            List<Foods> foods = new List<Foods>();
            List<Food> food = new List<Food>();
            foreach (var f in orderDTO.FoodsId)
                foods.Add(await _context.Foods.FindAsync(f));
            foreach (var f in foods)
                food.Add(new Food { Name = f.Name, Price = f.Price });


            Order order = new Order() { Date = DateTime.Now, User = await _userManager.FindByIdAsync(orderDTO.UserId), Foods = foods };
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            result.Id = order.Id;
            result.User = order.User.UserName;
            result.Date = order.Date;
            result.Foods = food;
            result.TotalPrice = food.Sum(f => f.Price);

            return CreatedAtAction("GetOrder", new { id = order.Id }, result);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
