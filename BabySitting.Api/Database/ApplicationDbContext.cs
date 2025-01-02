using BabySitting.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{

    public required DbSet<ParentOffer> ParentOffers { get; set; }
    public required DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>()
            .HasIndex(r => r.Name)
            .IsUnique();

        builder.HasDefaultSchema("identity");

        builder.Entity<ParentOffer>().ToTable("ParentOffers", "public");
        builder.Entity<Schedule>().ToTable("Schedules", "public");
    }
}
