using RazorTest.Models;
using Microsoft.Azure.Cosmos;
using RazorTest.Services;
using RazorTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddDbContext<MyDbContext>();

var cosmosConnectionString = builder.Configuration["CosmosDbConnectionString"];
var cosmosDatabaseName = builder.Configuration["CosmosDbContainerName"];

if (string.IsNullOrEmpty(cosmosConnectionString))
    throw new InvalidOperationException("Cosmos DB connection string is not configured.");
if (string.IsNullOrEmpty(cosmosDatabaseName))
    throw new InvalidOperationException("Cosmos DB database name is not configured.");

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseCosmos(
        cosmosConnectionString,
        cosmosDatabaseName
    )
);

builder.Services.AddRazorPages();

builder.Services.AddScoped<ManageProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
