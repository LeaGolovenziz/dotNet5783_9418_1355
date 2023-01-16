using BO;
using PL.CartWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public Cart cart = new Cart();

        private CollectionView? view;
        public ObservableCollection<ProductItem?> Products { get; set; }

        // get all products and creates an observeable Product Items list of them
        public ObservableCollection<ProductItem?> GetAllCatalog()
        {
            var Products = new ObservableCollection<ProductItem?>((IEnumerable<ProductItem?>)(from item in bl.Product.GetProductsList()
                                                                                              let amount = bl.Cart.AmountOf(cart, item.ID)
                                                                                              let isInStock = bl.Product.GetProductDetails(item.ID).InStock > 0 ? true : false
                                                                                              where isInStock== true || amount>0
                                                                                              select new ProductItem
                                                                                              {
                                                                                                  ID = item.ID,
                                                                                                  Name = item.Name,
                                                                                                  Price = item.Price,
                                                                                                  Category = item.Category,
                                                                                                  InStock = isInStock,
                                                                                                  Image = new Uri(Directory.GetCurrentDirectory().Replace("bin", item.Image), UriKind.Absolute).ToString(),
                                                                                                  AmountInCart = amount
                                                                                              }));

            this.DataContext = Products;
            return Products;
        }

        public CatalogWindow()
        {
            InitializeComponent();

            Products = GetAllCatalog();

            // get enums list for combo box
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        // sort catalog by category
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
            {
                Products = new ObservableCollection<ProductItem?>((IEnumerable<ProductItem?>)(from item in (IEnumerable<ProductItem?>)GetAllCatalog()
                                                                                              where item.Category == (BO.Enums.Category)CategorySelector.SelectedItem
                                                                                              select item));
                this.DataContext = Products;
            }

        }

        // clear all selected option and returns the window to start mode
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // deletes the grouping
            view = null;

            Products = GetAllCatalog();

            // clear combo box view
            CategorySelector.SelectedItem = null;
        }

        // open selecded item product
        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // get selected item 
            ProductItem product = (ProductItem)ProductListView.SelectedItem;

            // open a wondow with its details and option to add to cart
            if (product != null)
            {
                new ProductItemWindow(product.ID, cart, addToCartAction).Show();
            }
        }

        // go to cart
        private void goToCart(object sender, RoutedEventArgs e)
        {
            // if the cart is empty
            if (cart.OrderItems == null)
            {
                MessageBox.Show("your cart is empty!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // open a window with cart deatails
            else
                new CartWindow(cart, updateFromCartAction, Close).Show();
        }


        // add a product to cart
        private void addToCartAction(ProductItem? product)
        {

            // try to add the product to cart
            try
            {
                cart = bl.Cart.AddProductToCart(cart, product.ID);
                // update the products collection
                int index = Products.IndexOf(Products.FirstOrDefault(x => x.ID == product.ID));
                product.AmountInCart += 1;
                Products.RemoveAt(index);
                Products.Insert(index, product);
            }
            catch (ProductNotInStock ex)
            {
                MessageBox.Show("product is not in stock!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileSavingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileLoadingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (XmlFormatError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        // update the amount of the product to the amount od the product accepted from cart window
        private void updateFromCartAction(OrderItem orderItem)
        {
            // get the product from current collection and updates its amount
            ProductItem productItem = Products.FirstOrDefault(x => x.ID == orderItem.ID)!;
            productItem!.AmountInCart = orderItem.ProductAmount ?? 0;

            // update the products collection
            int index = Products.IndexOf(productItem);
            Products.RemoveAt(index);
            Products.Insert(index, productItem);
        }

        // add current product to cart
        private void addToCart(object sender, RoutedEventArgs e)
        {
            // get the product item that contains the button
            ProductItem productItem = (sender as Button).DataContext as ProductItem;
            // add it to cart
            addToCartAction(productItem);
        }

        // group the catelog by categories
        private void GroupCatalog(object sender, RoutedEventArgs e)
        {
            // if already grouped dont group again
            if (view == null)
            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(Products);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // to prevent failure
        }

        private void ProductListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // to prevent failure
        }
    }
}
