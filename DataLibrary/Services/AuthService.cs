using DataLibrary.Data;
using DataLibrary.Models;

namespace DataLibrary.Services
{
    ///<summary>
    ///Сервис для аутентификации и авторизации пользователей
    ///</summary>
    public class AuthService
    {
        readonly AppDbContext context;

        ///<summary>
        ///Инициализирует новый экземпляр сервиса аутентификации
        ///</summary>
        public AuthService()
        {
            context = new AppDbContext();
        }

        ///<summary>
        ///Выполняет вход пользователя в систему
        ///</summary>
        ///<param name="login">Логин пользователя</param>
        ///<param name="password">Пароль пользователя</param>
        ///<returns>Пользователя при успешном входе, иначе NULL</returns>
        public User Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            login = login.Trim();
            password = password.Trim();

            var user = context.Users
                .AsEnumerable()
                .FirstOrDefault(u =>
                    u.Login.Trim() == login &&
                    u.Password.Trim() == password
                );

            return user;
        }

        /// <summary>
        /// Получает пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Объект пользователя или null</returns>
        public User GetUserById(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        ///<summary>
        ///Проверяет, является ли роль администратором
        ///</summary>
        ///<param name="role">Название роли</param>
        ///<returns>true, если роль администратора</returns>
        public bool IsAdmin(string role)
        {
            return role == "admin" || role == "Администратор";
        }

        ///<summary>
        ///Проверяет, является ли роль менеджером
        ///</summary>
        ///<param name="role">Название роли</param>
        ///<returns>true, если роль менеджера</returns>
        public bool IsManager(string role)
        {
            return role == "manager" || role == "Менеджер";
        }

        ///<summary>
        ///Проверяет, имеет ли роль права на управление заказами
        ///</summary>
        ///<param name="role">Название роли</param>
        ///<returns>true, если роль имеет права на управление заказами</returns>
        public bool CanManageOrders(string role)
        {
            return IsAdmin(role) || IsManager(role);
        }
    }
}