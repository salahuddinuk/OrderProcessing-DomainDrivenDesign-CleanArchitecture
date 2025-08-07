using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Middleware;
using OrderProcessing.Application.Services;
using OrderProcessing.Application.Validators;
using OrderProcessing.Domain.Repositories;
using OrderProcessing.Infrastructure.Data;
using OrderProcessing.Infrastructure.Repositories;
using Serilog;
using Serilog.Exceptions;

// Ensure the required package is installed:
// Install-Package Microsoft.EntityFrameworkCore.SqlServer

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
        options.UseInMemoryDatabase("TestDb");
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var environment = builder.Environment.EnvironmentName;

//if (environment == "Testing")
//{
//    // Add DbContext (InMemory for testing)
//    builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
//        options.UseInMemoryDatabase("TestDb"));
//}
//else
//{
//    // Add DbContext (SQL Server)
//    builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(); // auto handle validations

// setup DI
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.UseSerilogRequestLogging(); // Enable Serilog request logging

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
//              .AllowAnyHeader()
//              .AllowAnyMethod());
//});

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }