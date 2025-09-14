using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorTest.Models;
using RazorTest.Services;   

namespace RazorTest.Pages
{
    public class ManageProductModel : PageModel
    {
        private readonly ManageProductService _manageProductService;

        public List<Product> Products = new List<Product>();

        public ManageProductModel(ManageProductService manageProductService)
        {
            _manageProductService = manageProductService;
        }

        public void OnGet()
        {
            initialize();
        }

        public void initialize()
        {
            var result = _manageProductService.GetAllProducts();
            result.Wait();
            Console.WriteLine("GetAllProducts called " + result.Result.ToString());
            Products = result.Result.ConvertAll(item => new Product
            {
                Id = item.id,
                Name = item.name,
                Price = item.price,
                Quantity = item.quantity
            });
        }
    }
}
