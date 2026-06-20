using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    ///<summary>
    ///Модель товара, представляющая продукт в системе терминала заказа
    ///</summary>
    [Table("Product")]
    public class Product
    {
        ///<summary>
        ///Артикул товара (уникальный идентификатор)
        ///</summary>ы
        [Key]
        public string Article { get; set; }

        ///<summary>
        ///Наименование товара
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///Единица измерения товара(шт, кг, л и т.д.)
        ///</summary>
        public string Unit { get; set; }

        ///<summary>
        ///Цена товара
        ///</summary>
        public int Price { get; set; }

        ///<summary>
        ///Cоздатель товара
        ///</summary>
        public string Author { get; set; }

        ///<summary>
        ///Производитель товара
        ///</summary>
        public string Manufacturer { get; set; }

        ///<summary>
        ///Категория товара
        ///</summary>
        public string Category { get; set; }

        ///<summary>
        ///Скидка на товар
        ///</summary>
        public int Discount { get; set; }

        ///<summary>
        ///Количество товара
        ///</summary>
        public int Amount { get; set; }

        ///<summary>
        ///Описание товара(может быть NULL)
        ///</summary>
        public string? Description { get; set; }

        ///<summary>
        ///Путь к фотографии товара(может быть NULL)
        ///</summary>
        public string? Photo { get; set; }

        /// <summary>
        /// Полный путь к изображению
        /// </summary>
        [NotMapped]
        public string ImagePath { get; set; }
    }
}