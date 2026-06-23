namespace TradeCenterApp
{
public partial class OrdersWindow
    {
        ///<summary>
        ///Вспомогательный класс для отображения товаров в заказе
        ///</summary>
        private class OrderItemDisplay
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
        }
    }
}