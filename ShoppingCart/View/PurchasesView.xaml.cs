using ShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PurchasesView.xaml
    /// </summary>
    public partial class PurchasesView : Window
    {
        public PurchasesView()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            StackPanel textBoxParent = textBox.Parent as StackPanel;
            TextBlock firstChildren = textBoxParent.Children[0] as TextBlock;

            MainWindowViewModel.TextBox_TextChanged(firstChildren.Text, Int32.Parse(textBox.Text));
        }
    }
}

