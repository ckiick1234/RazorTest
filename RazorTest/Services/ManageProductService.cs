using RazorTest.Models;
using RazorTest.Data;

namespace RazorTest.Services
{
    public class ManageProductService
    {
        private readonly ProductContext _context = default!;
        public ManageProductService(ProductContext context)
        {
            _context = context;
        }

        public IList<Product> GetAllProducts()
        {
            if (_context.Products == null)
            {
                return new List<Product>();
            }
            return _context.Products!.ToList();
        }

        public void AddProduct(Product product)
        {
            if (_context.Products == null)
            {
                throw new InvalidOperationException("Product context is not initialized.");
            }
            _context.Products.Add(product);
            _context.SaveChanges();
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

    }
}
