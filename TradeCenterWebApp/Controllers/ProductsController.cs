using DataLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeCenterWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // POST: /Products/Order
        [HttpPost]
        public IActionResult Order(string article)
        {
            var product = _context.Products.FirstOrDefault(p => p.Article == article);
            if (product == null)
                return NotFound();

            ViewBag.Message = $"Товар '{product.Name}' (Цена: {product.Price} ₽) добавлен в заказ!";
            return View("OrderConfirmation");
        }
    }
}
