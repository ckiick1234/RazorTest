using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorTest.Models
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }

        [Range(0,100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Range(1, 10000)]
        public int quantity { get; set; }
        //public string ImageUrl { get; set; }
    }
}
