using FlightAdventures.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightAdventures.Persistence;

public class FlightContext : DbContext
{
    public FlightContext(DbContextOptions<FlightContext> options)
        : base(options)
    {
    }
    
    public DbSet<Flight> Flights { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(256);
            
            entity.Property(e => e.Destination)
                .IsRequired()
                .HasMaxLength(256);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Username, "Username_Unique")
                .IsUnique();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(256);
            
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Code, "RoleCode_Unique")
                .IsUnique();

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(256);
        });
    }
}