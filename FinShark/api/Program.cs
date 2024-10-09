using Domain.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using api.Extensions;
using api.Middlewares;
using Application.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.CustomAddSwagger();


// Using a Custom Middleware Type 3 (Adding Service Necessary)
builder.Services.AddTransient<ExceptionHandlingMiddlewareT3>();


// Adding DbContext Through Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Adding Identity to the WebAPI
builder.Services.AddCustomIdentity();


// Things we are gonna use. Like JWT, Cookies and etc.
builder.Services.RegisterAutheticationWithJWT(builder.Configuration);


// Dependency Injection of Our Services
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IStockService, StockService>();


// Building the Application (Initialization)
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// Used for JWT Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();


// Using a Custom Middleware Type 1
//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next(context);
//    }
//    catch(Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//        context.Response.StatusCode = 500;
//    }
//});


// Using a Custom Middleware Type 2
//app.UseMiddleware<ExceptionHandlingMiddlewareT2>();


// Using a Custom Middleware Type 3 (Service Added Above)
app.UseMiddleware<ExceptionHandlingMiddlewareT3>();


app.MapControllers();


app.Run();