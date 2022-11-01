using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.DataTansferObject.Order
{
    public class AddOrderDTO
    {
        public List<int> FoodsId { get; set; }
        public string UserId { get; set; }
    }
}
