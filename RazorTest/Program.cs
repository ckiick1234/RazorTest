using RazorTest.Models;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MyDbContext>();

builder.Services.AddSingleton<CosmosClient>(ServiceProvider =>
{
    var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
    string accountEndpoint = configuration["CosmosDb:AccountEndpoint"] ?? throw new InvalidOperationException("Cosmos DB account endpoint is not configured.");
    string accountKey = configuration["CosmosDb:AccountKey"] ?? throw new InvalidOperationException("Cosmos DB account key is not configured.");
    CosmosClientOptions options = new CosmosClientOptions
    {
        ApplicationName = "RazorTestApp"
    };
    return new CosmosClient(accountEndpoint, accountKey, options);
});

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
