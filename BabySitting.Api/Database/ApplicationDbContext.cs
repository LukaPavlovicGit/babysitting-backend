using BabySitting.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BabySitting.Api.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{

    public DbSet<ParentOffer> ParentOffers { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

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
