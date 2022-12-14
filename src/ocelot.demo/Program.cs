using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.Configuration;
using Ocelot.demo;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.ServiceDiscovery.Providers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Host.ConfigureAppConfiguration(config =>
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    config.AddJsonFile($"ocelot.{env}.json");
});

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secret = "Thisismyhtestprivatekey";
    var key = Encoding.ASCII.GetBytes(secret);
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

Func<IServiceProvider, DownstreamRoute, IServiceDiscoveryProvider, CustomLoadBalancer> loadBalancerFactoryFunc = (serviceProvider, Route, serviceDiscoveryProvider) => new CustomLoadBalancer(serviceDiscoveryProvider.Get);

builder.Services
    .AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle())
    .AddCustomLoadBalancer(loadBalancerFactoryFunc);

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.UseOcelot().Wait();

app.Run();
