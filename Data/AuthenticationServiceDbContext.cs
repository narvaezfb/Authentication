using Authentication_Service.Models;
using Microsoft.EntityFrameworkCore;

public class AuthenticationServiceDbContext : DbContext
{
    public AuthenticationServiceDbContext(DbContextOptions<AuthenticationServiceDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}
