using MyQueue.Data.Models;
using MyQueue.DataTansferObject.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.OrdersServices
{
    interface IOrdersServices
    {
        public Task<IEnumerable<Order>> GetOrder();
        public Task<ResultOrderDTO> GetOrder(int id);
        public Task<bool> PutOrder(int id, Order order);
        public Task<Order> PostOrder(AddOrderDTO orderDTO);
        public Task<Order> DeleteOrder(int id);

    }
}
