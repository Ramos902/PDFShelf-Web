using Microsoft.EntityFrameworkCore;
using PDFShelf.Api.Models;

namespace PDFShelf.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Plan> Plans => Set<Plan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Plan>().HasData(
            new Plan { Id = 1, Name = "Free", StorageLimitMB = 30, CanAnnotate = true, CanShare = false, MonthlyPrice = null, IsActive = true },
            new Plan { Id = 2, Name = "Basic", StorageLimitMB = 200, CanAnnotate = true, CanShare = true, MonthlyPrice = 19.90, IsActive = true },
            new Plan { Id = 3, Name = "Premium", StorageLimitMB = 1000, CanAnnotate = true, CanShare = true, MonthlyPrice = 39.90, IsActive = true }
        );

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }

}