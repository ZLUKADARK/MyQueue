using System.Collections.Generic;

namespace MyQueue.Data.Models
{
    public class Foods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Order> Orders { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool? Active { get; set; }        
    }
}
