using Microsoft.EntityFrameworkCore;
using MyQueue.Data.Models;

namespace MyQueue.Data
{
    public class MQDBContext : DbContext
    {
        public MQDBContext(DbContextOptions<MQDBContext> options)
            : base(options)
        {
        }

        public DbSet<Foods> Foods { get; set; }

    }
}