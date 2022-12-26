using BO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<ProductForList?> Products { get; set; }
        public ProductList()
        {
            InitializeComponent();

            Products = new ObservableCollection<ProductForList?>(from item in bl.Product.GetProductsList()
                                                                 orderby item?.Name
                                                                 select item);
            this.DataContext = Products;

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
                Products = new ObservableCollection<ProductForList?>(bl.Product.GetProductsListByCondition(product => product?.Category == (BO.Enums.Category)CategorySelector.SelectedItem, Products));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProductListView.ItemsSource = bl.Product.GetProductsList();
            CategorySelector.SelectedItem = null;
        }

        private void addProduct(ProductForList productForList)
        => Products.Add(productForList);

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ProductWindow(addProduct).ShowDialog();
        }

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ProductForList product = (ProductForList)ProductListView.SelectedItem;
            if (product != null)
            {
                new ProductWindow(product.ID).ShowDialog();
                CategorySelector.SelectedItem = null;
            }
        }
    }
}
