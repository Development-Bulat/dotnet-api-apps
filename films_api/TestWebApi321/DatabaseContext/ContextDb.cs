using Microsoft.EntityFrameworkCore;
using TestWebApi321.Models;

namespace TestWebApi321.DatabaseContext
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options) 
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
