using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public class MyTradesDbContext : DbContext
{
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Capital> Capital {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=mytrades;Username=user;Password=user@123");        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
    }
}