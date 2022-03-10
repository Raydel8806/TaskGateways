using MusalaGatewaysSysAdmin.Models;
using Microsoft.EntityFrameworkCore;

IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Use local MSSQL Server "GatewaysDB" or in cloud "CloudGatewaysDB"
/// </summary>
builder.Services
    .AddDbContextFactory<GatewaysSysAdminDBContext>(optionsAction => optionsAction
    .UseSqlServer(configuration
    .GetConnectionString("GatewaysDB")));

/// <summary>
/// Use SQLGatewayRepository or other GatewayRepository technology. Ex. MockGatewayRepository
/// Comment using // to use MockGatewayRepository
/// </summary>
builder.Services.AddTransient<IGatewayRepository, SQLGatewayRepository>();

/// <summary>
/// Uncoment to use MockGatewayRepository
/// </summary>
//builder.Services.AddTransient<IGatewayRepository, MockGatewayRepository>();

// Add services to the container.

builder.Services.AddControllersWithViews();
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
