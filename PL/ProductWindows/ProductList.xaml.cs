using BlApi;
using Bllmplementation;
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

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {
        private IBl bl = new Bl();
        public ProductList()
        {
            InitializeComponent();
            ProductListView.ItemsSource = bl.Product.GetProductsList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CategorySelector.SelectedItem!=null)
            ProductListView.ItemsSource = bl.Product.GetProductsList(product => product.Value.Category == (DO.Enums.Category)CategorySelector.SelectedItem);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProductListView.ItemsSource = bl.Product.GetProductsList();
            CategorySelector.SelectedItem = null;
        }
    }
}
