using MySql.Data.MySqlClient;
using ShoppingCart.Model;
using ShoppingCart.View;
using ShoppingCart.ViewModel;
using System.Diagnostics;
using System.Management.Instrumentation;
using System.Windows;
using System.Windows.Markup;

namespace ShoppingCart
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }

        private void ViewPurchases_Click(object sender, RoutedEventArgs e)
        {
            PurchasesView purchasesView = new PurchasesView();
            purchasesView.Show();
        }

        private void ViewProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductsView productsVieww = new ProductsView();
            productsVieww.Show();   
        }
    }
}
