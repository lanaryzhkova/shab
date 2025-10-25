using Microsoft.EntityFrameworkCore;

namespace RESTful.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
        }
    }
}
