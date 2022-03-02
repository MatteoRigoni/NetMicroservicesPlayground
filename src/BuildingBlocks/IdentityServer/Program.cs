using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Test;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients(builder.Configuration))
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddTestUsers(Config.TestUsers)
                .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseRouting();
app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
