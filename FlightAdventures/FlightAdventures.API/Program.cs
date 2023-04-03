using FlightAdventures.Application.Queries;
using FlightAdventures.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration.WriteTo.Console();
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetFlightsQuery).Assembly));

builder.Services
    .AddDbContext<FlightContext>(options =>
        {
            options.UseSqlServer(builder.Configuration["ConnectionString"]);
        }
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces