using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlightAdventures.IntegrationTests.Common;

public class StartupFixture : WebApplicationFactory<Program>
{
    private static readonly object s_migrationLocker = new();
    
    protected override IHostBuilder CreateHostBuilder()
    {
        var hostBuilder = base.CreateHostBuilder();

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            hostBuilder.UseEnvironment("Development");

        return hostBuilder;
    }
    
    public StartupFixture()
    {
        if (Server is null)
            throw new InvalidOperationException("Unable to start TestServer");

        TryMigrate();
    }
    
    private void TryMigrate()
    {
        using var scope = Services.CreateScope();
        if (scope.ServiceProvider.GetService<IMigrationRunner>() is not { } migrationRunner)
            return;

        lock (s_migrationLocker)
            migrationRunner.MigrateUp();
    }
}