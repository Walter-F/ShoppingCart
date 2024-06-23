using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Controls;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace ShoppingCart.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static MainWindowViewModel instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance => instance;

        public static DateTime DateTime {  get; set; } = DateTime.Now;
        public ObservableCollection<Product> Products { get; set; }
        
        public ObservableCollection<History> PurchaseHistories { get; set; }

        public static ObservableCollection<Purchase> Purchases { get; set; }
        public static ObservableCollection<PurchaseUIElements> PurchaseUIElements { get; set; }

        public static DataTable ProductsTable { get; set; }
        public DataTable HistoryTable { get; set; }

        public Product selectedProduct {  get; set; }
        public static Product selectedProductForChange { get; set; }

        public StackPanel StackPanelPurchases { get; set; }

        public RelayCommand addNewProduct => new RelayCommand(execute => AddNewProduct());
        public RelayCommand addToCart => new RelayCommand(execute => AddToCart());
        public RelayCommand clearCart => new RelayCommand(execute => ClearCart());
        public RelayCommand saveToHistory => new RelayCommand(execute => SaveToHistory());
        public RelayCommand clearHistory => new RelayCommand(execute => ClearHistory());

        private DataTable _allProductsTable;
        public DataTable AllProductsTable 
        { 
            get => _allProductsTable; 
            set
            {
                if (_allProductsTable != value) 
                { 
                    _allProductsTable = value;
                    OnPropertyChanged(nameof(AllProductsTable));
                }
            }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                if (value != this.selectedItem)
                {
                    this.selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                }
            }
        }

        private string _newProduct;
        public string NewProduct
        {
            get { return _newProduct; }
            set
            {
                _newProduct = value;
                OnPropertyChanged(nameof(NewProduct));
            }
        }



        private int _totalAmount;
        public int TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }
        public MainWindowViewModel()
        {
            Products           = new ObservableCollection<Product>();
            Purchases          = new ObservableCollection<Purchase>();
            PurchaseHistories  = new ObservableCollection<History>();
            PurchaseUIElements = new ObservableCollection<PurchaseUIElements>();

            DB db = new DB();
            AllProductsTable = new DataTable();
            ProductsTable = new DataTable();
            HistoryTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            db.openConnection();
            MySqlCommand commandFirst = new MySqlCommand("SELECT * FROM `products`", db.getConnection());
            AllProductsTable.Load(commandFirst.ExecuteReader());

            MySqlCommand commandSecond = new MySqlCommand("SELECT * FROM `products` WHERE `isChecked` = 1", db.getConnection());
            ProductsTable.Load(commandSecond.ExecuteReader());

            MySqlCommand commandThird = new MySqlCommand("SELECT * FROM `history`", db.getConnection());
            HistoryTable.Load(commandThird.ExecuteReader());

            db.closeConnection();
        }

        public static void UpdateProduct(int id, bool newValue)
        {
            DB db = new DB();

            db.openConnection();

            MySqlCommand commandFirst = new MySqlCommand("UPDATE `products` SET `isChecked` = @isChecked WHERE `id` = @id", db.getConnection());
            commandFirst.Parameters.Add("@isChecked", MySqlDbType.Bit).Value = newValue;
            commandFirst.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            commandFirst.ExecuteNonQuery();

            MySqlCommand commandSecond = new MySqlCommand("SELECT * FROM `products` WHERE `isChecked` = 1", db.getConnection());
            ProductsTable.Clear();
            ProductsTable.Load(commandSecond.ExecuteReader());

            db.closeConnection();
        }
        public void AddNewProduct()
        {
            try 
            { 
                int newId = AllProductsTable.Rows.Count + 1;
                string nameNewProduct = NewProduct.Split(' ')[0];
                int priceNewProduct = Int32.Parse(NewProduct.Split(' ')[1]);

                Product product = new Product();
                product.Id = newId;
                product.Name = nameNewProduct;
                product.Price = priceNewProduct;
                product.isChecked = true;

                DataRow newRow = ProductsTable.NewRow();
                newRow["Id"] = product.Id;
                newRow["Name"] = product.Name;
                newRow["Price"] = product.Price;
                newRow["isChecked"] = product.isChecked;

                ProductsTable.Rows.Add(newRow);
                Products.Add(product);

                DB db = new DB();

                db.openConnection();

                MySqlCommand command = new MySqlCommand("INSERT INTO `products` (`Id`, `Name`, `Price`, `isChecked`) VALUES (NULL, @Name, @Price, @isChecked)", db.getConnection());
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = nameNewProduct;
                command.Parameters.Add("@Price", MySqlDbType.Int32).Value = priceNewProduct;
                command.Parameters.Add("@isChecked", MySqlDbType.Bit).Value = 1;

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show(
                        "Товар был успешно добавлен!",
                        "Успех!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                   "Товар не был добавлен",
                   "Ошибка!",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }

                db.closeConnection();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddToCart()
        {
            var selectedItemConverted = (DataRowView)selectedItem;

            if (selectedItemConverted != null)
            {

                foreach (Purchase item in Purchases)
                {
                    if (item.Product.Name == selectedItemConverted["Name"].ToString())
                    {
                        return;
                    }
                }

                Product product = new Product
                {
                    Id = Int32.Parse(selectedItemConverted["Id"].ToString()),
                    Name = selectedItemConverted["Name"].ToString(),
                    Price = Int32.Parse(selectedItemConverted["Price"].ToString()),
                    isChecked = Boolean.Parse(selectedItemConverted["isChecked"].ToString())
                };

                Purchase purchase = new Purchase
                {
                    Product = product,
                    Quantity = 1,
                    PriceTotal = product.Price,
                };

                Purchases.Add(purchase);

                PurchaseUIElements purchaseUIElements = new PurchaseUIElements
                {
                    NameTextBlock = new TextBlock(),
                    QuantityTextBox = new TextBox(),
                    PriceTextBlock = new TextBlock(),
                    PriceDetailTextBlock = new TextBlock(),
                    TotalPriceTextBlock = new TextBlock(),
                    TotalPriceDetailTextBlock = new TextBlock()
                };

                purchaseUIElements.NameTextBlock.Text = selectedItemConverted["Name"].ToString();
                purchaseUIElements.QuantityTextBox.Name = selectedItemConverted["Name"].ToString();
                purchaseUIElements.QuantityTextBox.Text = purchase.Quantity.ToString();
                purchaseUIElements.PriceTextBlock.Text = "Цена";
                purchaseUIElements.PriceDetailTextBlock.Text = purchase.Product.Price.ToString();
                purchaseUIElements.TotalPriceTextBlock.Text = "Общая сумма";
                purchaseUIElements.TotalPriceDetailTextBlock.Text = (purchase.Product.Price * purchase.Quantity).ToString();

                PurchaseUIElements.Add(purchaseUIElements);
            }
        }

        public static void TextBox_TextChanged(string nameProduct, int newQuantity)
        {
            foreach(var item in Purchases)
            {
                if(item.Product.Name == nameProduct)
                {
                    item.Quantity = newQuantity;
                }
            }

            foreach (var item in PurchaseUIElements)
            {
                if(item.NameTextBlock.Text == nameProduct) 
                {
                    item.TotalPriceDetailTextBlock.Text = (Int32.Parse(item.PriceDetailTextBlock.Text) * newQuantity).ToString();
                }
            }

            Debug.WriteLine(DateTime);
        }

        public void ClearCart()
        {
            Purchases.Clear();
            PurchaseUIElements.Clear();
        }

        public void SaveToHistory()
        {
            DateTime dateTime = DateTime;
            string formatForMySql = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder PurchasesStringBuilder = new StringBuilder();
            int TotalPrice = 0;

            foreach(var item in Purchases) 
            {
                PurchasesStringBuilder.AppendLine($"{item.Product.Name} {item.Quantity} шт. {item.PriceTotal} р.");
                TotalPrice += item.PriceTotal;
            }

            DB db = new DB();

            db.openConnection();

            MySqlCommand command = new MySqlCommand("INSERT INTO `history` (`id`, `Date`, `Purchases`, `TotalPrice`) VALUES (NULL, @Date, @Purchases, @TotalPrice)", db.getConnection());
            command.Parameters.Add("@Date", MySqlDbType.Date).Value = formatForMySql;
            command.Parameters.Add("@Purchases", MySqlDbType.VarChar).Value = PurchasesStringBuilder.ToString();
            command.Parameters.Add("@TotalPrice", MySqlDbType.Int32).Value = TotalPrice;
           
            command.ExecuteNonQuery();

            HistoryTable.Clear();
            MySqlCommand commandSecond = new MySqlCommand("SELECT * FROM `history`", db.getConnection());
            HistoryTable.Load(commandSecond.ExecuteReader());

            db.closeConnection();
        }

        public void ClearHistory()
        {
            DB db = new DB();

            db.openConnection();

            MySqlCommand command = new MySqlCommand("DELETE FROM `history`", db.getConnection());

            command.ExecuteNonQuery();
            HistoryTable.Clear();

            db.closeConnection();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
