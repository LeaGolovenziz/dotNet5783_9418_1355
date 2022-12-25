using PL.OrderWindows;
using PL.ProductWindows;
using System;
using System.Collections.Generic;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }
        private void MenuItem_Click_ShowProductList(object sender, RoutedEventArgs e) => new ProductList().Show();

        private void MenuItem_Click_Add(object sender, RoutedEventArgs e) => new ProductWindow().Show();

        private void MenuItem_Click_ShowOrderList(object sender, RoutedEventArgs e) => new OrderWindow().Show();
    }
}
