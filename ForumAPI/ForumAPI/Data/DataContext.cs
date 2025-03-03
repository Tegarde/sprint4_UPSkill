using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    
        public DbSet<Post> Posts { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
