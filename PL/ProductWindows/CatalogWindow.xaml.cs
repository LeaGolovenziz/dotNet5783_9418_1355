using BO;
using PL.CartWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Cart cart = new Cart();
        private CollectionView? view;
        public ObservableCollection<ProductItem?> Products { get; set; }
        public ObservableCollection<ProductItem?> GetAllCatalog()
        {
            return new ObservableCollection<ProductItem?>((IEnumerable<ProductItem?>)(from item in bl.Product.GetProductsList()
                                                                                      let amount = bl.Cart.AmountOf(cart, item.ID)
                                                                                      let isInStock = bl.Product.GetProductDetails(item.ID).InStock > 0 ? true : false
                                                                                      select new ProductItem
                                                                                      {
                                                                                          ID = item.ID,
                                                                                          Name = item.Name,
                                                                                          Price = item.Price,
                                                                                          Category = item.Category,
                                                                                          InStock = isInStock,
                                                                                          Image = item.Image,
                                                                                          AmountInCart = amount
                                                                                      }));
        }


        public CatalogWindow()
        {
            InitializeComponent();

            Products = GetAllCatalog();

            this.DataContext = Products;

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));




        }



        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
            {
                Products = new ObservableCollection<ProductItem?>((IEnumerable<ProductItem?>)(from item in bl.Product.GetProductsList()
                                                                                              where item.Category == (BO.Enums.Category)CategorySelector.SelectedItem
                                                                                              let amount = bl.Cart.AmountOf(cart, item.ID)
                                                                                              let isInStock = bl.Product.GetProductDetails(item.ID).InStock > 0 ? true : false
                                                                                              select new ProductItem
                                                                                              {
                                                                                                  ID = item.ID,
                                                                                                  Name = item.Name,
                                                                                                  Price = item.Price,
                                                                                                  Category = item.Category,
                                                                                                  InStock = isInStock,
                                                                                                  Image = item.Image,
                                                                                                  AmountInCart = amount
                                                                                              }
                                                                                              ));
                this.DataContext = Products;
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            view = null;

            Products = GetAllCatalog();

            this.DataContext = Products;

            CategorySelector.SelectedItem = null;
        }


        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ProductItem product = (ProductItem)ProductListView.SelectedItem;
            if (product != null)
            {
                new ProductItemWindow(product.ID, cart, addToCartAction).ShowDialog();
                CategorySelector.SelectedItem = null;
            }
        }

        private void goToCart(object sender, RoutedEventArgs e)
        {
            if (cart.OrderItems == null)
            {
                MessageBox.Show("your cart is empty!");
            }
            else
                new CartWindow(cart, updateFromCartAction, Close).ShowDialog();
        }


        private void addToCartAction(ProductItem? product)
        {

                if (product?.InStock != false && bl.Product.GetProductDetails(product.ID).InStock - product.AmountInCart > 0)
                {
                    cart=bl.Cart.AddProductToCart(cart, product.ID);

                    int index = Products.IndexOf(Products.FirstOrDefault(x=>x.ID==product.ID));
                    product.AmountInCart += 1;
                    Products.RemoveAt(index);
                    Products.Insert(index, product);
                    this.DataContext = Products;
                }
                else
                    MessageBox.Show("product is not in stock");
        }
        private void updateFromCartAction(OrderItem orderItem)
        {
            ProductItem productItem = Products.FirstOrDefault(x => x.ID == orderItem.ID)!;
            productItem!.AmountInCart = orderItem.ProductAmount??0;

            int index = Products.IndexOf(productItem);
            Products.RemoveAt(index);
            Products.Insert(index, productItem);
            this.DataContext = Products;
        }
    private void addToCart(object sender, RoutedEventArgs e)
    {
        ProductItem productItem = (sender as Button).DataContext as ProductItem;
        addToCartAction(productItem);
    }

    private void GroupCatalog(object sender, RoutedEventArgs e)
    {
        if (view == null)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(Products);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
            view.GroupDescriptions.Add(groupDescription);
        }
    }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProductListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
