using DataLibrary.Data;
using DataLibrary.Models;

namespace DataLibrary.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService()
        {
            _context = new AppDbContext();
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductByArticle(string article)
        {
            return _context.Products.FirstOrDefault(p => p.Article == article);
        }
    }
}