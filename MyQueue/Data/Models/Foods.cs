﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Data.Models
{
    public class Foods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Order> Orders { get; set; }
        public Category Category { get; set; }
    }
}