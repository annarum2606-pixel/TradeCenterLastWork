using DataLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeCenterApi.Controllers
{
    ///<summary>
    ///API контроллер для управления товарами
    ///</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        ///<summary>
        ///Инициализирует новый экземпляр контроллера товаров API
        ///</summary>
        ///<param name="context">Контекст базы данных</param>
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        ///Получает список всех товаров
        ///</summary>
        ///<returns>Список всех товаров</returns>
        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        ///<summary>
        ///Получает товар по артикулу
        ///</summary>
        ///<param name="article">Артикул товара</param>
        ///<returns>Товар</returns>
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