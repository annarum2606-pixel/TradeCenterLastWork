using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.Data
{
    ///<summary>
    ///Контекст БД для работы с сущностями системы
    ///</summary>
    public class AppDbContext : DbContext
    {
        ///<summary>
        ///Инициализирует новый экземпляр контекста с заданными параметрами
        ///</summary>
        ///<param name="options">Параметры конфигурации контекста</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        ///<summary>
        ///Инициализирует новый экземпляр контекста с настройками по умолчанию
        ///</summary>
        public AppDbContext()
        {
        }

        ///<summary>
        ///Коллекция товаров в БД
        ///</summary>
        public DbSet<Product> Products { get; set; }

        ///<summary>
        ///Коллекция пользователей в БД
        ///</summary>
        public DbSet<User> Users { get; set; }

        ///<summary>
        ///Коллекция заказов в БД
        ///</summary>
        public DbSet<Order> Orders { get; set; }

        ///<summary>
        ///Коллекция позиций заказов в БД
        ///</summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        ///<summary>
        ///Настройка модели БД
        ///</summary>
        /// <param name="modelBuilder">Построитель модели</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //явно указываем имя таблицы для пользователей 
            modelBuilder.Entity<User>().ToTable("User");
        }

        ///<summary>
        ///Настройка подключения к БД
        ///</summary>
        ///<param name="optionsBuilder">Построитель строки подключения</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TradeCenterDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}