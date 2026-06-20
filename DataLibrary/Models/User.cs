using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    ///<summary>
    ///Модель, представляющая пользователя в системе
    ///</summary>
    [Table("Users")]
    public class User
    {
        ///<summary>
        ///Уникальный идентификатор пользователя
        ///</summary>
        [Key]
        public int Id { get; set; }

        ///<summary>
        ///Роль пользователя(Администратор, Менеджер, Авторизированный клиент)
        ///</summary>
        public string Role { get; set; }

        ///<summary>
        ///Полное имя пользователя
        ///</summary>
        public string FullName { get; set; }

        ///<summary>
        ///Логин пользователя
        ///</summary>
        public string Login { get; set; }

        ///<summary>
        ///Пароль пользователя
        ///</summary>
        public string Password { get; set; }
    }
}