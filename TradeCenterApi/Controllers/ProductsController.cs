using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET: api/products/{article}
        [HttpGet("{article}")]
        public async Task<IActionResult> GetByArticle(string article)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Article == article);
            if (product is null)
                return NotFound();
            return Ok(product);
        }
    }
}
