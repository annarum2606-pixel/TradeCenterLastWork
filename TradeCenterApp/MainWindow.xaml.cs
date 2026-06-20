using DataLibrary.Models;
using DataLibrary.Services;
using System.Windows;
using System.Windows.Controls;

namespace TradeCenterApp
{
    ///<summary>
    ///Главное окно приложения заказа товаров
    ///</summary>
    public partial class MainWindow : Window
    {
        readonly ProductService productService;
        readonly AuthService authService;
        string currentSortBy = "name";
        bool isDescending = false;
        bool isLoaded = false;
        User currentUser;
        bool isAuthenticated = false;

        ///<summary>
        ///Инициализирует новый экземпляр главного окна
        ///</summary>
        public MainWindow()
        {
            InitializeComponent();
            productService = new ProductService();
            authService = new AuthService();
            Loaded += MainWindow_Loaded;
        }

        ///<summary>
        ///Обработчик события загрузки окна
        ///</summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            LoadManufacturers();
            LoadProducts();
        }

        ///<summary>
        ///Устанавливает текущего авторизованного пользователя
        ///</summary>
        ///<param name="user">Авторизованный пользователь</param>
        private void SetAuthenticatedUser(User user)
        {
            currentUser = user;
            isAuthenticated = true;

            UserFullNameLabel.Text = user.FullName;
            UserFullNameLabel.Visibility = Visibility.Visible;
            AuthButton.Content = "Выйти";

            if (authService.CanManageOrders(user.Role))
                OrdersButton.Visibility = Visibility.Visible;
            else
                OrdersButton.Visibility = Visibility.Collapsed;
        }

        ///<summary>
        ///Очищает данные об авторизации пользователя
        ///</summary>
        private void ClearAuthentication()
        {
            currentUser = null;
            isAuthenticated = false;

            UserFullNameLabel.Text = "";
            UserFullNameLabel.Visibility = Visibility.Collapsed;
            AuthButton.Content = "Войти";
            OrdersButton.Visibility = Visibility.Collapsed;
        }

        ///<summary>
        ///Загружает список производителей для фильтрации
        ///</summary>
        private void LoadManufacturers()
        {
            try
            {
                var manufacturers = productService.GetAllManufacturers();
                ManufacturerComboBox.Items.Clear();
                ManufacturerComboBox.Items.Add("Все производители");
                foreach (var manufacturer in manufacturers)
                    ManufacturerComboBox.Items.Add(manufacturer);
                ManufacturerComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки производителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///<summary>
        ///Загружает товары с применением фильтров
        ///</summary>
        private void LoadProducts()
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///<summary>
        ///Применяет фильтры, поиск и сортировку к списку товаров
        ///</summary>
        private void ApplyFilters()
        {
            if (!isLoaded)
                return;

            string searchText = SearchTextBox?.Text?.Trim() ?? string.Empty;

            string manufacturer = "Все производители";
            if (ManufacturerComboBox?.SelectedItem != null)
                manufacturer = ManufacturerComboBox.SelectedItem.ToString();

            int? minPrice = ParsePrice(MinPriceTextBox?.Text);
            int? maxPrice = ParsePrice(MaxPriceTextBox?.Text);

            if (SortComboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content?.ToString())
                {
                    case "По названию":
                        currentSortBy = "name";
                        break;
                    case "По цене":
                        currentSortBy = "price";
                        break;
                    case "По производителю":
                        currentSortBy = "manufacturer";
                        break;
                    default:
                        currentSortBy = "name";
                        break;
                }
            }

            isDescending = DescendingCheckBox?.IsChecked ?? false;

            var (filteredProducts, totalCount, filteredCount) = productService
                .GetFilteredProducts(
                    searchText,
                    manufacturer,
                    minPrice,
                    maxPrice,
                    currentSortBy,
                    isDescending
                );

            ProductList.ItemsSource = filteredProducts;
            CountLabel.Text = $"Записей: {filteredCount} из {totalCount}";
        }

        ///<summary>
        ///Преобразует строку в число для фильтрации по цене
        ///</summary>
        ///<param name="text">Строка с ценой</param>
        ///<returns>Целочисленное значение или NULL</returns>
        private int? ParsePrice(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string cleanText = text.Replace(" ", "").Replace(",", "").Replace("₽", "");

            if (int.TryParse(cleanText, out int value))
                return value;

            return null;
        }

        ///<summary>
        ///Обработчик нажатия кнопки входа/выхода
        ///</summary>
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAuthenticated)
                ClearAuthentication();
            else
            {
                var loginWindow = new LoginWindow(authService);
                loginWindow.Owner = this;
                loginWindow.ShowDialog();

                if (loginWindow.IsAuthenticated && loginWindow.AuthenticatedUser is not null)
                    SetAuthenticatedUser(loginWindow.AuthenticatedUser);
            }
        }

        ///<summary>
        ///Обработчик нажатия кнопки Заказы(доступно только админам и менеджерам)
        ///</summary>
        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser is null)
            {
                MessageBox.Show("Пожалуйста, войдите в систему.", "Доступ запрещен",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!authService.CanManageOrders(currentUser.Role))
            {
                MessageBox.Show("У вас нет прав для просмотра заказов.", "Доступ запрещен",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Открытие окна управления заказами для {currentUser.FullName} (роль: {currentUser.Role})",
                "Заказы", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        ///<summary>
        ///Обработчик изменения текста в поле ввода поиска
        ///</summary>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        ///<summary>
        ///Обработчик изменения выбранного производителя
        ///</summary>
        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        ///<summary>
        ///Обработчик изменения текста в поле ввода цены
        ///</summary>
        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        ///<summary>
        ///Обработчик изменения типа сортировкиы
        ///</summary>
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        ///<summary>
        ///Обработчик нажатия галочки "по убыванию"
        ///</summary>
        private void DescendingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        ///<summary>
        ///Обработчик снятия галочки "по убыванию"
        ///</summary>
        private void DescendingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}