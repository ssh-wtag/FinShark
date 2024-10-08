using Domain.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Written to Enable Usage of Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.CustomAddSwagger();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});


// Adding DbContext Through Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Adding Identity to the WebAPI
builder.Services.AddCustomIdentity();


///////TO DO -> builder.Services.RegisterAutheticationWithJWT();
// Things we are gonna use. Like JWT, Cookies and etc.
builder.Services.RegisterAutheticationWithJWT(builder.Configuration);


// Dependency Injection of Our Services
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();


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

app.MapControllers();

app.Run();
