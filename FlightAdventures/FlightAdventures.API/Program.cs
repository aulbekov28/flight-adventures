using FlightAdventures.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<FlightContext>(options =>
        {
            options.UseSqlServer(builder.Configuration["ConnectionString"]);
        },
        ServiceLifetime.Scoped
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces