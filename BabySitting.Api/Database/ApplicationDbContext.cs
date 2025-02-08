using BabySitting.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{

    public required DbSet<Offer> Offers { get; set; }
    public required DbSet<Schedule> Schedules { get; set; }
    public required DbSet<Verification> Verifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<IdentityRole>()
            .HasIndex(r => r.Name)
            .IsUnique();
        
        builder.Entity<Offer>().ToTable("Offers", "public");
        builder.Entity<Schedule>().ToTable("Schedules", "public");
        builder.Entity<Verification>().ToTable("Verifications", "public");
    }
}
