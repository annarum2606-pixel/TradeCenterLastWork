using DataLibrary.Data;
using DataLibrary.Models;

namespace DataLibrary.Services
{
    ///<summary>
    ///Сервис для работы с заказами
    ///</summary>
    public class OrderService
    {
        readonly AppDbContext _context;

        ///<summary>
        ///Инициализирует новый экземпляр сервиса заказов
        ///</summary>
        public OrderService()
        {
            _context = new AppDbContext();
        }

        ///<summary>
        ///Получает список всех заказов
        ///</summary>
        ///<returns>Список заказов</returns>
        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        ///<summary>
        ///Получает заказ по ID
        ///</summary>
        ///<param name="id">ID заказа</param>
        ///<returns>Заказ или NULL</returns>
        public Order GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        ///<summary>
        ///Получает список позиций заказа
        ///</summary>
        ///<param name="orderId">ID заказа</param>
        ///<returns>Список позиций</returns>
        public List<OrderItem> GetOrderItems(int orderId)
        {
            return _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();
        }

        ///<summary>
        ///Получает товар по артикулу
        ///</summary>
        ///<param name="article">Артикул товара</param>
        ///<returns>Товар или NULL</returns>
        public Product GetProductByArticle(string article)
        {
            return _context.Products.FirstOrDefault(p => p.Article == article);
        }

        ///<summary>
        ///Обновляет статус заказа
        ///</summary>
        ///<param name="orderId">ID заказа</param>
        ///<param name="newStatus">Новый статус</param>
        ///<returns>true если успешно</returns>
        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order is null)
                return false;

            order.Status = newStatus;
            _context.SaveChanges();
            return true;
        }

        ///<summary>
        ///Обновляет дату доставки заказа
        ///</summary>
        ///<param name="orderId">ID заказа</param>
        ///<param name="deliveryDate">Новая дата доставки</param>
        ///<returns>true если успешно</returns>
        public bool UpdateDeliveryDate(int orderId, DateTime? deliveryDate)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order is null)
                return false;

            order.DeliveryDate = deliveryDate;
            _context.SaveChanges();
            return true;
        }

        ///<summary>
        ///Возвращает список возможных статусов заказа
        ///</summary>
        ///<returns>Список статусов</returns>
        public List<string> GetOrderStatuses()
        {
            return new List<string> { "Новый", "В обработке", "Готов к выдаче", "Доставлен", "Отменен" };
        }

        ///<summary>
        ///Получает имя клиента по ID пользователя
        ///</summary>
        ///<param name="userId">ID пользователя</param>
        ///<returns>Имя клиента или "Неавторизованный клиент"</returns>
        public string GetCustomerName(int? userId)
        {
            if (userId is null || userId == 0)
                return "Неавторизованный клиент";

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user?.FullName ?? "Неизвестный клиент";
        }
    }
}