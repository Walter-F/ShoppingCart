using MySql.Data.MySqlClient;
using ShoppingCart.Model;
using ShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShoppingCart.View
{
    /// <summary>
    /// Логика взаимодействия для ProductsView.xaml
    /// </summary>
    public partial class ProductsView : Window
    {
        public ProductsView()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;

            dtGridProducts.CellEditEnding += DataGrid_CellEditEnding;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.Column.Header.ToString() == "Видимость")
            {
                DataGridRow row = e.Row as DataGridRow;
                if (row != null)
                {
                    DataRowView item = (DataRowView)row.Item;
                    if (item != null)
                    {
                        // Устанавливаем данные в SelectedRowData
                        // Product product = new Product(); ... 
                        MainWindowViewModel.UpdateProduct(Int32.Parse(item.Row["Id"].ToString()), !Boolean.Parse(item.Row["isChecked"].ToString()));

                        // MainWindowViewModel.selectedProductForChange = item;
                    }
                }
            }
        }
    }
}
