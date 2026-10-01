using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stadsdelsmammor.Models;

namespace Stadsdelsmammor.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mamma> Mammor => Set<Mamma>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Mamma>(entity =>
        {
            entity.Property(m => m.Fornamn)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Efternamn)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Telefonnummer)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(m => m.Omrade)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Sprak)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(m => m.Efternamn);
            entity.HasIndex(m => m.Omrade);
            entity.HasIndex(m => m.Sprak);
            entity.HasIndex(m => m.Status);
        });
    }
}
