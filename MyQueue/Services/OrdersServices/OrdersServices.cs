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
        public Task<Order> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrder()
        {
            throw new NotImplementedException();
        }

        public Task<ResultOrderDTO> GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> PostOrder(AddOrderDTO orderDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutOrder(int id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
