using BO;
using DO;
using PL.CartWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<ProductItem?> Products { get; set; }

        public CatalogWindow()
        {
            InitializeComponent();

            Products = new ObservableCollection<ProductItem?>((IEnumerable<ProductItem?>)(from item in bl.Product.GetProductsList()
                                                                                          where bl.Product.GetProductDetails(item.ID).InStock > 0
                                                                                          select new ProductItem
                                                                                          {
                                                                                              ID = item.ID,
                                                                                              Name = item.Name,
                                                                                              Price = item.Price,
                                                                                              Category = item.Category,
                                                                                              InStock = true,
                                                                                              Image = item.Image,
                                                                                              AmountInCart = 0
                                                                                          }
                                                                 )
                                                                 );
            this.DataContext = Products;

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));

        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
                Products = new ObservableCollection<ProductItem?>(from item in Products
                                                                  where item?.Category == (BO.Enums.Category)CategorySelector.SelectedItem
                                                                  select item);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProductListView.ItemsSource = Products;
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
