var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration.WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flights API", Version = "v1" });
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<GetFlightsQuery, ICollection<Flight>>), typeof(GetFlightsCacheBehavior));
        cfg.AddBehavior(typeof(IPipelineBehavior<AddFlightCommand, Flight>), typeof(AddFlightCacheBehavior));
    }
);

// redis context configuration, move to separate file
var redisConfiguration = builder.Configuration.GetSection("Redis");
var redisOptions = ConfigurationOptions.Parse(redisConfiguration["Configuration"]!);
redisOptions.Password = redisConfiguration["Password"];

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = redisOptions;
});

// db context configuration, move to separate file
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
    pattern: "{controller}/{action}/{id?}");

app.Run();