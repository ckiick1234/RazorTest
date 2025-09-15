using RazorTest.Models;
using Microsoft.Azure.Cosmos;
using RazorTest.Services;
using RazorTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);



if (builder.Environment.IsProduction())
{
    var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
    var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
    var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
    var keyVaultDirectoryId = builder.Configuration.GetSection("KeyVault:DirectoryId");

    var credential = new ClientSecretCredential(
        keyVaultDirectoryId.Value!.ToString() ?? throw new InvalidOperationException("KeyVaultDirectoryId is not configured."),
        keyVaultClientId.Value!.ToString() ?? throw new InvalidOperationException("KeyVaultClientId is not configured."),
        keyVaultClientSecret.Value!.ToString() ?? throw new InvalidOperationException("KeyVaultClientSecret is not configured.")
    );

    var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()),credential);
    
    builder.Services.AddDbContext<ProductContext>(options =>
        options.UseCosmos(
            client.GetSecret("CosmosDbConnectionString").Value.Value.ToString(),
            client.GetSecret("CosmosDbContainerName").Value.Value.ToString()
        )
    );
}
else
{
    builder.Configuration.AddUserSecrets<Program>();
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
}

builder.Services.AddDbContext<MyDbContext>();

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
