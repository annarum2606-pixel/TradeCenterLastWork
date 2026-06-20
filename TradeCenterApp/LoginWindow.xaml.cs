using DataLibrary.Models;
using DataLibrary.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TradeCenterApp
{
    ///<summary>
    ///Окно для входа пользователя в систему
    ///</summary>
    public partial class LoginWindow : Window
    {
        readonly AuthService _authService;

        ///<summary>
        ///Флаг успешной аутентификации
        ///</summary
        public bool IsAuthenticated { get; private set; } = false;

        ///<summary>
        /// Аутентифицированный пользователь
        ///</summary>
        public User AuthenticatedUser { get; private set; } = null;

        ///<summary>
        ///Инициализирует новый экземпляр окна входа
        ///</summary>
        ///<param name="authService">Сервис аутентификации</param>
        public LoginWindow(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            Loaded += LoginWindow_Loaded;
        }

        ///<summary>
        ///Обработчик события загрузки окна(устанавливает фокус на поле логина)
        ///</summary>
        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTextBox.Focus();
        }

        ///<summary>
        ///Обработчик нажатия кнопки Войти
        ///</summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text?.Trim();
            string password = PasswordBox.Password?.Trim();

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Пожалуйста, введите логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                LoginTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            try
            {
                var user = _authService.Login(login, password);

                if (user is not null)
                {
                    IsAuthenticated = true;
                    AuthenticatedUser = user;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с указанным логином и паролем не найден.\nПроверьте правильность введенных данных.",
                        "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    PasswordBox.Clear();
                    PasswordBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при попытке входа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///<summary>
        ///Обработчик нажатия кнопки Отмена
        ///</summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        ///<summary>
        ///Обработчик нажатия клавиши Enter в поле пароля
        ///</summary>
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}