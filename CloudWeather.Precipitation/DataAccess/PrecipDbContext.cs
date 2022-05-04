using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Precipitation.DataAccess;

public class PrecipDbContext: DbContext
{
    public PrecipDbContext(DbContextOptions opt): base(opt)
    {
        
    }
    
    public DbSet<Precipitation> Precipitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentityTablesNames(modelBuilder);
        
    }

    private static void SnakeCaseIdentityTablesNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Precipitation>(b => { b.ToTable("precipitation"); });
    }
}