using System.Collections.Generic;

namespace MyQueue.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Foods> Foods { get; set; }
    }
}
