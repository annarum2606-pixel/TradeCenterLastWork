using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public string Article { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public int Discount { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
    }
}