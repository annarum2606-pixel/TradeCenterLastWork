using DataLibrary.Models;
using DataLibrary.Services;
using System.Windows;
using System.Windows.Controls;

namespace TradeCenterApp
{
    ///<summary>
    ///Окно управления заказами
    ///</summary>
    public partial class OrdersWindow : Window
    {
        readonly OrderService orderService;
        Order currentOrder;
        bool isUpdating = false;

        ///<summary>
        ///Инициализирует новый экземпляр окна заказов
        ///</summary>
        public OrdersWindow()
        {
            InitializeComponent();
            orderService = new();
            Loaded += OrdersWindow_Loaded;
        }

        ///<summary>
        ///Обработчик загрузки окна
        ///</summary>
        private void OrdersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatuses();
            ClearOrderDisplay();
        }

        ///<summary>
        ///Загружает статусы в выпадающий список
        ///</summary>
        private void LoadStatuses()
        {
            StatusComboBox.Items.Clear();
            foreach (var status in orderService.GetOrderStatuses())
                StatusComboBox.Items.Add(status);
        }

        ///<summary>
        ///Поиск заказа по ID
        ///</summary>
        private void SearchOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderIdTextBox.Text, out int orderId))
            {
                MessageBox.Show("Введите корректный ID заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = orderService.GetOrderById(orderId);
            if (order is null)
            {
                MessageBox.Show($"Заказ с ID {orderId} не найден.", "Не найден", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearOrderDisplay();
                return;
            }

            DisplayOrder(order);
        }

        ///<summary>
        ///Показывает список всех заказов
        ///</summary>
        private void ShowAllOrders_Click(object sender, RoutedEventArgs e)
        {
            var orders = orderService.GetAllOrders();
            if (orders.Count == 0)
            {
                MessageBox.Show("Заказов нет.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string message = "Список заказов:\n";
            foreach (var order in orders)
                message += $"ID: {order.Id} | №{order.OrderNumber} | {order.OrderDate:dd.MM.yyyy} | {order.Status}\n";

            MessageBox.Show(message, "Все заказы", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        ///<summary>
        ///Очищает поля ввода и отображения
        ///</summary>
        private void ClearOrder_Click(object sender, RoutedEventArgs e)
        {
            ClearOrderDisplay();
            OrderIdTextBox.Clear();
        }

        ///<summary>
        ///Отображает информацию о заказе
        ///</summary>
        ///<param name="order">Заказ для отображения</param>
        private void DisplayOrder(Order order)
        {
            currentOrder = order;
            isUpdating = true;

            OrderIdText.Text = order.Id.ToString();
            OrderNumberText.Text = order.OrderNumber.ToString();
            OrderDateText.Text = order.OrderDate.ToString("dd.MM.yyyy HH:mm");
            CustomerNameText.Text = orderService.GetCustomerName(order.UserId).Trim();
            CodeText.Text = order.Code ?? "-";
            StatusComboBox.SelectedItem = order.Status;
            DeliveryDatePicker.SelectedDate = order.DeliveryDate;

            LoadOrderItems(order.Id);
            isUpdating = false;
        }

        ///<summary>
        ///Загружает товары в заказе
        ///</summary>
        ///<param name="orderId">ID заказа</param>
        private void LoadOrderItems(int orderId)
        {
            var items = orderService.GetOrderItems(orderId);
            var itemsWithNames = new List<OrderItemDisplay>();

            foreach (var item in items)
            {
                var product = orderService.GetProductByArticle(item.ProductArticle);
                itemsWithNames.Add(new OrderItemDisplay
                {
                    ProductName = product?.Name ?? item.ProductArticle,
                    Quantity = item.Quantity
                });
            }

            OrderItemsList.ItemsSource = itemsWithNames;
        }

        ///<summary>
        ///Очищает отображение заказа
        ///</summary>
        private void ClearOrderDisplay()
        {
            currentOrder = null;
            isUpdating = true;

            OrderIdText.Text = "-";
            OrderNumberText.Text = "-";
            OrderDateText.Text = "-";
            CustomerNameText.Text = "-";
            CodeText.Text = "-";
            StatusComboBox.SelectedIndex = -1;
            DeliveryDatePicker.SelectedDate = null;
            OrderItemsList.ItemsSource = null;

            isUpdating = false;
        }

        ///<summary>
        ///Обработчик изменения статуса заказа
        ///</summary>
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating || currentOrder == null || StatusComboBox.SelectedItem == null)
                return;

            string newStatus = StatusComboBox.SelectedItem.ToString();
            if (newStatus == currentOrder.Status)
                return;

            var result = MessageBox.Show($"Изменить статус заказа №{currentOrder.OrderNumber} на '{newStatus}'?",
                                        "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (orderService.UpdateOrderStatus(currentOrder.Id, newStatus))
                {
                    currentOrder.Status = newStatus;
                    MessageBox.Show("Статус заказа обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении статуса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    isUpdating = true;
                    StatusComboBox.SelectedItem = currentOrder.Status;
                    isUpdating = false;
                }
            }
            else
            {
                isUpdating = true;
                StatusComboBox.SelectedItem = currentOrder.Status;
                isUpdating = false;
            }
        }

        ///<summary>
        ///Обработчик изменения даты доставки
        ///</summary>
        private void DeliveryDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating || currentOrder is null)
                return;

            DateTime? newDate = DeliveryDatePicker.SelectedDate;
            if (newDate == currentOrder.DeliveryDate)
                return;

            string dateStr = newDate?.ToString("dd.MM.yyyy") ?? "не указана";
            var result = MessageBox.Show($"Изменить дату доставки заказа №{currentOrder.OrderNumber} на {dateStr}?",
                                        "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (orderService.UpdateDeliveryDate(currentOrder.Id, newDate))
                {
                    currentOrder.DeliveryDate = newDate;
                    MessageBox.Show("Дата доставки обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении даты доставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    isUpdating = true;
                    DeliveryDatePicker.SelectedDate = currentOrder.DeliveryDate;
                    isUpdating = false;
                }
            }
            else
            {
                isUpdating = true;
                DeliveryDatePicker.SelectedDate = currentOrder.DeliveryDate;
                isUpdating = false;
            }
        }
    }
}