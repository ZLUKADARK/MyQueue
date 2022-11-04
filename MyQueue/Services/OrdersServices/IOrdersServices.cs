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
        public Task<IEnumerable<ResultOrderDTO>> GetOrder();
        public Task<ResultOrderDTO> GetOrder(int id);
        public Task<ResultOrderDTO> PostOrder(AddOrderDTO orderDTO);
        public Task<ShortResult> DeleteOrder(int id);

    }
}
