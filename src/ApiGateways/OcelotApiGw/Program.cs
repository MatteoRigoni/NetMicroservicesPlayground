using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var authenticationProviderKey = "IdentityApiKey";
builder.Services.AddAuthentication()
 .AddJwtBearer(authenticationProviderKey, x =>
 {
     x.Authority = "https://localhost:5006"; // IDENTITY SERVER URL
                          //x.RequireHttpsMetadata = false;
     x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false
     };
 });

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) => { logger.LogInformation("Hello World!"); });

await app.UseOcelot();

app.Run();
