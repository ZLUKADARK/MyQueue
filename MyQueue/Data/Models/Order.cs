using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Foods> Foods { get; set; }
    }
}
