using DataLibrary.Data;
using DataLibrary.Models;

namespace DataLibrary.Services
{
    ///<summary>
    ///Сервис для работы с товарами в системе
    ///</summary>
    public class ProductService
    {
        readonly AppDbContext _context;

        ///<summary>
        ///Инициализирует новый экземпляр сервиса товаров
        ///</summary>
        public ProductService()
        {
            _context = new AppDbContext();
        }

        ///<summary>
        ///Получает все товары из БД
        ///</summary>
        ///<returns>Список всех товаров</returns>
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductByArticle(string article)
        {
            return _context.Products.FirstOrDefault(p => p.Article == article);
        }

        ///<summary>
        ///Получает список всех производителей товаров
        ///</summary>
        ///<returns>Список производителей</returns>
        public List<string> GetAllManufacturers()
        {
            return _context.Products
                .Select(p => p.Manufacturer)
                .Where(m => !string.IsNullOrEmpty(m))
                .Distinct()
                .OrderBy(m => m)
                .ToList();
        }

        ///<summary>
        ///Получает отфильтрованный список товаров с учетом поиска, фильтрации и сортировки
        ///</summary>
        ///<param name="searchText">Текст для поиска по имени</param>
        ///<param name="manufacturer">Фильтр по производителю</param>
        ///<param name="minPrice">Минимальная цена</param>
        ///<param name="maxPrice">Максимальная цена</param>
        ///<param name="sortBy">Поле для типа сортировки(name, price, manufacturer)</param>
        ///<param name="descending">Направление сортировки(true - по убыванию, false - по возрастанию)</param>
        ///<returns>Лист с отфильтрованными товарами, общим количеством и количеством после фильтрации</returns>
        public (List<Product> Products, int TotalCount, int FilteredCount) GetFilteredProducts(
            string searchText = null,
            string manufacturer = null,
            int? minPrice = null,
            int? maxPrice = null,
            string sortBy = null,
            bool descending = false)
        {
            var query = _context.Products.AsQueryable();

            // Поиск по наименованию (без учета регистра)
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p => p.Name != null &&
                    p.Name.ToLower().Contains(searchText.ToLower()));
            }

            // Фильтрация по производителю
            if (!string.IsNullOrWhiteSpace(manufacturer) && manufacturer != "Все производители")
            {
                query = query.Where(p => p.Manufacturer == manufacturer);
            }

            // Фильтрация по цене (минимальная)
            if (minPrice.HasValue && minPrice.Value > 0)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            // Фильтрация по цене (максимальная)
            if (maxPrice.HasValue && maxPrice.Value < int.MaxValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var totalCount = _context.Products.Count();
            var filteredCount = query.Count();

            // Сортировка
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = descending
                            ? query.OrderByDescending(p => p.Name)
                            : query.OrderBy(p => p.Name);
                        break;
                    case "price":
                        query = descending
                            ? query.OrderByDescending(p => p.Price)
                            : query.OrderBy(p => p.Price);
                        break;
                    case "manufacturer":
                        query = descending
                            ? query.OrderByDescending(p => p.Manufacturer)
                            : query.OrderBy(p => p.Manufacturer);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }

            return (query.ToList(), totalCount, filteredCount);
        }
    }
}