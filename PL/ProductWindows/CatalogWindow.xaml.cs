using BO;
using PL.CartWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public CatalogWindow()
        {
            InitializeComponent();
            //ProductListView.ItemsSource=bl.Product.

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));

        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
                CategorySelector.ItemsSource = bl.Product.GetProductsList(product => product?.Category == (DO.Enums.Category)CategorySelector.SelectedItem);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProductListView.ItemsSource = bl.Product.GetProductsList();
            CategorySelector.SelectedItem = null;
        }


        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ProductItem product = (ProductItem)ProductListView.SelectedItem;
            if (product != null)
            {
                new ProductWindow(product.ID).ShowDialog();
                CategorySelector.SelectedItem = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => new CartWindow().Show();
    }
}
