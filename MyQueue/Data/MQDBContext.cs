using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data.Models;

namespace MyQueue.Data
{
    public class MQDBContext : IdentityDbContext
    {
        public MQDBContext(DbContextOptions<MQDBContext> options)
            : base(options)
        {
        }
        public DbSet<Foods> Foods { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }

    }
}