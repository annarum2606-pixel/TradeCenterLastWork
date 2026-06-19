using DataLibrary.Data;
using System.Windows;
using System.Windows.Controls;

namespace TradeCenterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using var context = new AppDbContext();
            ProductList.ItemsSource = context.Products.ToList();
        }
    }
}