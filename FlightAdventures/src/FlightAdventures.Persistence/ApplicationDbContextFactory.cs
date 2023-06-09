﻿using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FlightAdventures.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<FlightContext>
{
    /// <summary>
    /// Used for migrations
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public FlightContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FlightContext>();
        var storeOptions = Options.Create(new OperationalStoreOptions());
        // TODO read properly from configs
        optionsBuilder.UseSqlServer("data source=localhost, 1433;initial catalog=MyFlights;user id=sa;password=Strong@Passw0rd;Encrypt=False");

        return new FlightContext(optionsBuilder.Options, storeOptions);    
    }
}