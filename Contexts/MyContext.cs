#nullable disable
using Microsoft.EntityFrameworkCore;
using simple_api.Models;

namespace simple_api.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }    
    }
}
