using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RazorTest.Data;
using RazorTest.Models;
using System.Net;

namespace RazorTest.Services
{
    public class ManageProductService
    {
        private readonly ProductContext _context = default!;
        private readonly IConfiguration _configuration;

        public ManageProductService(ProductContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task AddProduct(Product product)
        {
            //if (_context.Products == null)
            //{
            //    throw new InvalidOperationException("Product context is not initialized.");
            //}
            //_context.Products.Add(product);
            //_context.SaveChanges();

            //List<dynamic> items = new List<dynamic>();

            try
            {
                CosmosClient cosmosClient = new CosmosClient(_configuration["CosmosDbConnectionString"]);
                var _container = cosmosClient.GetContainer("twincity", "products");

                await _container.CreateItemAsync(product, new PartitionKey(product.id));

                return;
            }
            catch (CosmosException e)
            {
                return;
            }
        }

        public void UpdateProduct(Product product)
        {
            if (_context.Products == null)
            {
                throw new InvalidOperationException("Product context is not initialized.");
            }
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            if (_context.Products == null)
            {
                throw new InvalidOperationException("Product context is not initialized.");
            }
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public async Task<List<dynamic>> GetAllProducts()
        {
            List<dynamic> items = new List<dynamic>();

            try
            {
                CosmosClient cosmosClient = new CosmosClient(_configuration["CosmosDbConnectionString"]);
                var _container = cosmosClient.GetContainer("twincity", "products");

                // Create a query to select all items
                QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

                // Get the FeedIterator for the query
                using FeedIterator<dynamic> feedIterator = _container.GetItemQueryIterator<dynamic>(queryDefinition);

                // Iterate through the results, page by page
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                    foreach (dynamic item in response)
                    {
                        items.Add(item);
                    }
                }

                return items;
            }
            catch (CosmosException e)
            {
                return items;
            }
        }

    }
}
