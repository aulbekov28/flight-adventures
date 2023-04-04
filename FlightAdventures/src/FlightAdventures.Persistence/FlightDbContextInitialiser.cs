using FlightAdventures.Domain.Enums;
using FlightAdventures.Domain.Models;
using FlightAdventures.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightAdventures.Persistence;

public class FlightDbContextInitializer
{
    private readonly FlightContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<FlightDbContextInitializer> _logger;

    public FlightDbContextInitializer(
        FlightContext context,
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        ILogger<FlightDbContextInitializer> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default roles
        var moderatorRole = new ApplicationRole
        {
            Name = "Moderator"
        };
        
        if (_roleManager.Roles.All(r => r.Name != moderatorRole.Name))
        {
            await _roleManager.CreateAsync(moderatorRole);
        }
        
        var userRole = new ApplicationRole
        {
            Name = "User"
        };
        
        if (_roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
        }
        
        // Default users
        var administrator = new ApplicationUser { UserName = "moderator@localhost", Email = "moderator@localhost" };
        
        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Moderator1!");
            if (!string.IsNullOrWhiteSpace(userRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { userRole.Name });
            }
        }
        
        var user = new ApplicationUser { UserName = "user@localhost", Email = "user@localhost" };
        
        if (_userManager.Users.All(u => u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(user, "User1!");
            if (!string.IsNullOrWhiteSpace(userRole.Name))
            {
                await _userManager.AddToRolesAsync(user, new [] { userRole.Name });
            }
        }
        
        // Default data
        // Seed, if necessary
        if (!_context.Flights.Any())
        {
            _context.Flights.Add(new Flight { Origin = "NZQ", Destination = "UKK", Departure = DateTimeOffset.Now, Arrival = DateTimeOffset.Now.AddHours(2), Status = FlightStatus.InTime});
            _context.Flights.Add(new Flight { Origin = "UKK", Destination = "HND ", Departure = DateTimeOffset.Now.AddMonths(1), Arrival = DateTimeOffset.Now.AddMonths(1).AddHours(45), Status = FlightStatus.Cancelled});
            _context.Flights.Add(new Flight { Origin = "ALA", Destination = "LED", Departure = DateTimeOffset.Now.AddDays(1), Arrival = DateTimeOffset.Now.AddDays(1).AddHours(8), Status = FlightStatus.Delayed});
        
            await _context.SaveChangesAsync();
        }
    }
}