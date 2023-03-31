using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace FlightAdventures.IntegrationTests.Common;

public class StartupFixture : WebApplicationFactory<Program>
{
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
    }
}