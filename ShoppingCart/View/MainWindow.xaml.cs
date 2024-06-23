using MySql.Data.MySqlClient;
using ShoppingCart.Model;
using ShoppingCart.View;
using ShoppingCart.ViewModel;
using System;
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
            try
            {
                PurchasesView purchasesView = new PurchasesView();
                purchasesView.Show();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ViewProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductsView productsVieww = new ProductsView();
                productsVieww.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
