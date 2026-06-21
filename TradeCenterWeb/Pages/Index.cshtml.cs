using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataLibrary.Data;
using DataLibrary.Models;

namespace TradeCenterWeb.Pages
{
    /// <summary>
    /// Страница для отображения списка товаров и добавления их в заказ.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Список товаров, отображаемый на странице.
        /// </summary>
        public List<Product> Products { get; set; } = new();

        /// <summary>
        /// Сообщение для пользователя после добавления товара в заказ.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Загружает список товаров из БД при GET-запросе.
        /// </summary>
        public void OnGet()
        {
            Products = _context.Products.ToList();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Заказать".
        /// Находит товар по артикулу и выводит сообщение.
        /// </summary>
        /// <param name="article">Артикул товара</param>
        public IActionResult OnPostOrder(string article)
        {
            var product = _context.Products.FirstOrDefault(p => p.Article == article);

            if (product is not null)
                Message = $"Товар '{product.Name}' (цена: {product.Price} р) добавлен в заказ!";
            else
                Message = "Товар не найден.";

            Products = _context.Products.ToList();
            return Page();
        }
    }
}