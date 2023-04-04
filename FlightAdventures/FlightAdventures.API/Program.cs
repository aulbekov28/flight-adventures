using System.Collections.Generic;
using System.Reflection;
using FlightAdventures.Application.Abstractions;
using FlightAdventures.Application.Commands.AddFlight;
using FlightAdventures.Application.Queries.GetFlight;
using FlightAdventures.Domain.Models;
using FlightAdventures.Infrastructure.Identity;
using FlightAdventures.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration.WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<GetFlightsQuery, ICollection<Flight>>), typeof(GetFlightsCacheBehavior));
        cfg.AddBehavior(typeof(IPipelineBehavior<AddFlightCommand, Flight>), typeof(AddFlightCacheBehavior));
    }
);

// Context configuration, move to separate file
builder.Services.AddDbContext<FlightContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString"],
        optionActions => optionActions.MigrationsAssembly(typeof(FlightContext).Assembly.FullName));
});
builder.Services.AddScoped<IFlightDbContext>(provider => provider.GetRequiredService<FlightContext>());
builder.Services.AddScoped<FlightDbContextInitializer>();
builder.Services
    .AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<FlightContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, FlightContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<FlightDbContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();

// TODO needed for web application factory in the integration tests
#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces