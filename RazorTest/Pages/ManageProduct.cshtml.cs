using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RazorTest.Models;
using RazorTest.Services;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace RazorTest.Pages
{
    public class ManageProductModel : PageModel
    {
        private readonly ManageProductService _manageProductService;

        public List<Product> Products = new List<Product>();

        [BindProperty]
        public Product FormData { get; set; }

        public ManageProductModel(ManageProductService manageProductService)
        {
            _manageProductService = manageProductService;
        }

        public void OnGet()
        {
            initialize();
        }

        public async Task OnPostAsync()
        {
            Product product = new Product {
                id = new Random().Next().ToString(),//Guid.NewGuid().ToString(),
                name = FormData.name,
                price = FormData.price,
                quantity = FormData.quantity };
            

            var result = _manageProductService.AddProduct(product);
            result.Wait();
            initialize();
        }

        public async Task OnPostDeleteAsync(string id)
        {
            var result = _manageProductService.DeleteProduct(id);
            result.Wait();
            initialize();
        }

        public void initialize()
        {
            var result = _manageProductService.GetAllProducts();
            result.Wait();
            Products = result.Result.ConvertAll(item => new Product
            {
                id = item.id,
                name = item.name,
                price = item.price,
                quantity = item.quantity
            });
        }


    }
}
