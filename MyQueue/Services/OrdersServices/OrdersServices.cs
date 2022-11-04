using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.Data.Models;
using MyQueue.DataTansferObject.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.OrdersServices
{
    public class OrdersServices : IOrdersServices
    {
        private readonly MQDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public OrdersServices(MQDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ShortResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
                return null;

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return new ShortResult { Date = order.Date, Id = order.Id };
        }

        public async Task<IEnumerable<ResultOrderDTO>> GetOrder()
        {
            var result = from order in _context.Order.Include(x => x.Foods).Include(x => x.User)
                         select new ResultOrderDTO
                         {
                             Id = order.Id,
                             User = order.User.UserName,
                             Date = order.Date,
                             Foods = order.Foods.Select(x => new FoodOrder { Name = x.Name, Price = x.Price }).ToList(),
                             TotalPrice = order.Foods.Select(x => new FoodOrder { Name = x.Name, Price = x.Price }).Sum(t => t.Price)
                         };

            return await result.ToListAsync();
        }

        public async Task<ResultOrderDTO> GetOrder(int id)
        {
            var result = from order in _context.Order.Where(o => o.Id == id).Include(x => x.Foods).Include(x => x.User)
                         select new ResultOrderDTO
                         {
                             Id = order.Id,
                             User = order.User.UserName,
                             Date = order.Date,
                             Foods = order.Foods.Select(x => new FoodOrder { Name = x.Name, Price = x.Price }).ToList(),
                             TotalPrice = order.Foods.Select(x => new FoodOrder { Name = x.Name, Price = x.Price }).Sum(t => t.Price)
                         };

            if (result == null)
                return null;

            return await result.FirstOrDefaultAsync();
        }

        public async Task<ResultOrderDTO> PostOrder(AddOrderDTO orderDTO)
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

            return new ResultOrderDTO { Id = order.Id, User = order.User.UserName, Date = order.Date, Foods = foodOrder, TotalPrice = foodOrder.Sum(f => f.Price)};
        }
    }
}
