using DataLibrary.Data;
using DataLibrary.Models;

namespace DataLibrary.Services
{
    ///<summary>
    ///Сервис для работы с товарами в системе
    ///</summary>
    public class ProductService
    {
        readonly AppDbContext context;
        readonly string path;

        ///<summary>
        ///Инициализирует новый экземпляр сервиса товаров, а так же пишет путь для папки images
        ///</summary>
        public ProductService()
        {
            context = new AppDbContext();
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        }

        ///<summary>
        ///Получает список всех товаров с путями к картинкам
        ///</summary>
        ///<returns>Список всех товаров</returns>
        public List<Product> GetAllProducts()
        {
            var products = context.Products.ToList();
            SetImagePaths(products);
            return products;
        }

        ///<summary>
        ///Получает товар по артикулу с путем к картинке
        ///</summary>
        ///<param name="article">Артикул товара</param>
        ///<returns>Товар или NULL, если не найден</returns>
        public Product GetProductByArticle(string article)
        {
            var product = context.Products.FirstOrDefault(p => p.Article == article);
            if (product is not null)
                SetImagePath(product);
            return product;
        }

        ///<summary>
        ///Получает список всех производителей товаров
        ///</summary>
        ///<returns>Отсортированный список производителей</returns>
        public List<string> GetAllManufacturers()
        {
            return context.Products
                .Select(p => p.Manufacturer)
                .Where(m => !string.IsNullOrEmpty(m))
                .Distinct()
                .OrderBy(m => m)
                .ToList();
        }

        ///<summary>
        ///Получает отфильтрованный и отсортированный список товаров
        ///</summary>
        ///<param name="searchText">Текст поиска по названию</param>
        ///<param name="manufacturer">Фильтр по производителю</param>
        ///<param name="minPrice">Минимальная цена</param>
        ///<param name="maxPrice">Максимальная цена</param>
        ///<param name="sortBy">Поле текста типа сортировки(name, price, manufacturer)</param>
        ///<param name="descending">Направление сортировки(true - по убыванию, false - по возрастанию)</param>
        ///<returns>Список товаров, общее количество, количество с фильтром</returns>
        public (List<Product> Products, int TotalCount, int FilteredCount) GetFilteredProducts(
                string searchText = null,
                string manufacturer = null,
                int? minPrice = null,
                int? maxPrice = null,
                string sortBy = null,
                bool descending = false
            )
        {
            var query = context.Products.AsQueryable();

            //Поиск по названию
            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(searchText.ToLower()));

            //Фильтрация по производителю
            if (!string.IsNullOrWhiteSpace(manufacturer) && manufacturer != "Все производители")
                query = query.Where(p => p.Manufacturer == manufacturer);

            //Минимальная цена
            if (minPrice.HasValue && minPrice.Value > 0)
                query = query.Where(p => p.Price >= minPrice.Value);

            //Максимальная цена
            if (maxPrice.HasValue && maxPrice.Value < int.MaxValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var totalCount = context.Products.Count();
            var filteredCount = query.Count();

            //Сортировка
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                    case "price":
                        query = descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                        break;
                    case "manufacturer":
                        query = descending ? query.OrderByDescending(p => p.Manufacturer) : query.OrderBy(p => p.Manufacturer);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name);
                        break;
                }
            }
            else
                query = query.OrderBy(p => p.Name);

            var products = query.ToList();
            SetImagePaths(products);

            return (products, totalCount, filteredCount);
        }

        ///<summary>
        ///Путь к картинкам для списка товаров
        ///</summary>
        ///<param name="products">Список товаров</param>
        private void SetImagePaths(List<Product> products)
        {
            foreach (var product in products)
                SetImagePath(product);
        }

        ///<summary>
        ///Путь к картинкам для одного товара
        ///</summary>
        ///<param name="product">Товар</param>
        private void SetImagePath(Product product)
        {
            if (string.IsNullOrEmpty(product.Photo))
                return;

            string fileName = product.Photo.TrimStart('/');

            string fullPath = Path.Combine(path, fileName);

            if (!File.Exists(fullPath))
                return;

            product.ImagePath = fullPath;

        }
    }
}