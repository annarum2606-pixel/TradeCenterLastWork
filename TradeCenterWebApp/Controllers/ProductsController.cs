using DataLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeCenterWebApp.Controllers
{
    ///<summary>
    ///Контроллер для управления товарами в веб-приложении
    ///</summary>
    public class ProductsController : Controller
    {
        readonly AppDbContext _context;

        ///<summary>
        ///Инициализирует новый экземпляр контроллера товаров
        ///</summary>
        ///<param name="context">Контекст БД</param>
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        ///Отображает список всех товаров
        ///</summary>
        ///<returns>Представление со списком товаров</returns>
        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        ///<summary>
        ///Обрабатывает запрос на добавление товара в заказ
        ///</summary>
        ///<param name="article">Артикул товара для заказа</param>
        ///<returns>Представление с подтверждением заказа или ошибкой 404</returns>
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
