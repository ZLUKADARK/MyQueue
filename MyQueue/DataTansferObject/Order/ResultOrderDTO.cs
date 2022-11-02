using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.DataTansferObject.Order
{
    public class ResultOrderDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public decimal TotalPrice { get; set; }
        public List<FoodOrder> Foods { get; set; }
    }
}
