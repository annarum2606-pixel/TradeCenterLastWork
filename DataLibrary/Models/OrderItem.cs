using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    ///<summary>
    ///Модель позиции заказа, представляющая товар в заказе
    ///</summary>
    [Table("OrderItem")]
    public class OrderItem
    {
        ///<summary>
        ///Уникальный идентификатор позиции заказа
        ///</summary>
        [Key]
        public int Id { get; set; }

        ///<summary>
        ///Идентификатор заказа, к которому относится позиция
        ///</summary>
        public int OrderId { get; set; }

        ///<summary>
        ///Артикул товара в позиции заказа
        ///</summary>
        public string ProductArticle { get; set; }

        ///<summary>
        ///Количество товара
        ///</summary>
        public int Quantity { get; set; }
    }
}