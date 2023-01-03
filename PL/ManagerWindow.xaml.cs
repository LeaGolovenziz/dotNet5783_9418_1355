using PL.OrderWindows;
using PL.ProductWindows;
using System.Windows;

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
        private void MenuItem_Click_ShowProductList(object sender, RoutedEventArgs e) => new ProductList().ShowDialog();


        private void MenuItem_Click_ShowOrderList(object sender, RoutedEventArgs e) => new OrderList().ShowDialog();

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // to prevent failure
        }
    }
}
