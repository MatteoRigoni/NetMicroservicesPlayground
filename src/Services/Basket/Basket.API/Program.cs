using Basket.API.GrpcServices;
using Basket.API.Repositories;
using DIscount.Grpc.Protos;
using MassTransit;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//IdentityModelEventSource.ShowPII = true;

//builder.Services.AddAuthentication("Bearer")
//.AddJwtBearer("Bearer", options =>
//{
//    options.Authority = "https://localhost:5006";
//     options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = false
//    };
});

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddStackExchangeRedisCache(opts =>
{ opts.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString"); });
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
