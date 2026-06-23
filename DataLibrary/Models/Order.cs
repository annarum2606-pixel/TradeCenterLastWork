using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    /// <summary>
    /// Модель заказа, содержащая информацию о заказе клиента
    /// </summary>s
    [Table("Order")]
    public class Order
    {
        ///<summary>
        ///Уникальный идентификатор заказа
        ///</summary>
        [Key]
        public int Id { get; set; }

        ///<summary>
        ///Номер заказа
        ///</summary>
        public int OrderNumber { get; set; }

        ///<summary>
        ///Дата оформления заказа
        ///</summary>
        public DateTime OrderDate { get; set; }

        ///<summary>
        ///Дата доставки заказа(может быть NULL)
        ///</summary>
        public DateTime? DeliveryDate { get; set; }

        ///<summary>
        ///Идентификатор пользователя, оформившего заказ (может быть NULL)
        ///</summary>
        public int? UserId { get; set; }

        ///<summary>
        ///Код для получения заказа
        ///</summary>
        public string Code { get; set; }

        ///<summary>
        ///Статус заказа(Новый, В обработке, Доставлен и т.д.)
        ///</summary>
        public string Status { get; set; }
    }
}