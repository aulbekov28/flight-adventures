using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using FlightAdventures.Application.Abstractions;
using FlightAdventures.Domain.Models;
using FlightAdventures.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FlightAdventures.Persistence;

public class FlightContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IFlightDbContext, IPersistedGrantDbContext
{
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

    public FlightContext(
        DbContextOptions<FlightContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;
    }
    
    public DbSet<Flight> Flights { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
        
        modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
    }
    
    public DbSet<PersistedGrant> PersistedGrants { get; set; }
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    public DbSet<Key> Keys { get; set; }
    
    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}