using Ar_Card_Connect_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ar_Card_Connect_Api.DataBase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) 
    { 
        
    }
    public DbSet<UserCard>  UserCards { get; set; }
    public DbSet<UserLogin>  UserLogins { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
}