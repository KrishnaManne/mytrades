using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public class MyTradesDbContext : DbContext
{
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Capital> Capital {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<OtpVerification> OtpVerifications { get; set; }
    public MyTradesDbContext(DbContextOptions options): base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql("Host=localhost;Database=mytrades;Username=krishna.manne9;Password=mytrades@123");        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
    }
}