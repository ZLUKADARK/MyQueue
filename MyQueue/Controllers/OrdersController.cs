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
            var result = await _context.Order.Include(x => x.Foods).Include(x => x.User).ToListAsync();
            return Ok(result);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultOrderDTO>> GetOrder(int id)
        {
            var order = await _context.Order.Where(o => o.Id == id).Include(x => x.Foods).Include(u =>  u.User).FirstOrDefaultAsync();
            List<FoodOrder> foodOrder = new List<FoodOrder>();
            foreach (var o in order.Foods) 
                foodOrder.Add(new FoodOrder { Name = o.Name, Price = o.Price });
                
            if (order == null)
                return NotFound();

            return new ResultOrderDTO { Id = order.Id, User = order.User.UserName, Date = order.Date, Foods = foodOrder, TotalPrice = foodOrder.Sum(f => f.Price) };
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
            List<Foods> foods = new List<Foods>();
            List<FoodOrder> foodOrder = new List<FoodOrder>();
            foreach (var f in orderDTO.FoodsId)
                foods.Add(await _context.Foods.FindAsync(f));
            foreach (var f in foods)
                foodOrder.Add(new FoodOrder { Name = f.Name, Price = f.Price });

            Order order = new Order() { Date = DateTime.Now, User = await _userManager.FindByIdAsync(orderDTO.UserId), Foods = foods };
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", 
                new { id = order.Id }, 
                new ResultOrderDTO { Id = order.Id, User = order.User.UserName, Date = order.Date, Foods = foodOrder, TotalPrice = foodOrder.Sum(f => f.Price)});
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
