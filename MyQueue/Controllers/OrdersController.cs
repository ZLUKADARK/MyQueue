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
using MyQueue.Services.OrdersServices;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersServices _orderServices;
        public OrdersController(OrdersServices orderServices)
        {
            _orderServices = orderServices;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultOrderDTO>>> GetOrder()
        {
            var result = await _orderServices.GetOrder();   
            return result.ToList();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultOrderDTO>> GetOrder(int id)
        {
            var result = await _orderServices.GetOrder(id);
            return result;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<ResultOrderDTO>> PostOrder(AddOrderDTO orderDTO)
        {
            var result = await _orderServices.PostOrder(orderDTO); 
            return CreatedAtAction("GetOrder", new { id = result.Id }, result);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShortResult>> DeleteOrder(int id)
        {
            var result = await _orderServices.DeleteOrder(id);
            if (result == null)
                return NotFound();
            return result;
        }
    }
}
