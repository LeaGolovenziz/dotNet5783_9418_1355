using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.Primitives;

namespace PL.CartWindows
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<OrderItem> Items { get; set; }

        private Cart cart=new Cart();

        private OrderItem orderItem = new OrderItem();

        private Action closePrevWindow;
        private Action<OrderItem> updateCartAction;


        public CartWindow(Cart cart, Action<OrderItem> updateCartAction, Action closePrevWindow)
        {
            InitializeComponent();

            Items = new ObservableCollection<OrderItem>(cart.OrderItems);

            ItemListView.DataContext = Items;

            this.cart = cart;

            totalPriceLable.DataContext = this.cart;

            this.updateCartAction = updateCartAction;
            this.closePrevWindow = closePrevWindow; 


        }

        private void ItemListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ItemListView.SelectedItem != null)
            {
                SelectedItemGrid.Visibility = Visibility.Visible;

                orderItem = (OrderItem)ItemListView.SelectedItem;
                SelectedItemGrid.DataContext= orderItem;  

                try
                {
                    Uri resourceUri = new Uri(bl.Product.GetProductDetails(orderItem.ID).Image, UriKind.Absolute);
                    ProductImage.Source = new BitmapImage(resourceUri);
                }
                // incase there is no image
                catch (Exception ex) { }

            }
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
           // OrderItem orderItem = (OrderItem)ItemListView.SelectedItem;
            SelectedItemGrid.DataContext = orderItem;


            if (orderItem != null)
                if (bl.Product.GetProductDetails(orderItem.ID).InStock - orderItem.ProductAmount > 0)
                {
                    bl.Cart.AddProductToCart(cart, orderItem.ID);

                    int index = Items.IndexOf(orderItem);

                    orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                    updateCartAction(orderItem);

                    orderItem.TotalPrice = orderItem.ProductAmount * orderItem.Price;
                    Items.RemoveAt(index);
                    Items.Insert(index, orderItem);
                }
                else
                    MessageBox.Show("product is not in stock");
        }

        private void SubButton(object sender, RoutedEventArgs e)
        {
            //orderItem = (OrderItem)ItemListView.SelectedItem;

            if (orderItem != null)
            {
                bl.Cart.UpdateProductAmountInCart(cart, orderItem.ID, (int)(orderItem.ProductAmount - 1)!);

                int index = Items.IndexOf(orderItem);

                orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                updateCartAction(orderItem);

                if (orderItem.ProductAmount > 0)
                {
                    Items.RemoveAt(index);
                    Items.Insert(index, orderItem);
                }
                else
                {
                    Items.RemoveAt(index);
                    SelectedItemGrid.Visibility = Visibility.Hidden;
                }
                this.DataContext = Items;
            }
        }

        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            // OrderItem orderItem = (OrderItem)ItemListView.SelectedItem;
            if (orderItem != null)
            {
                Delete(orderItem);
                SelectedItemGrid.Visibility = Visibility.Hidden;
            }
 
        }
        private void Delete(OrderItem orderItem)
        {
                bl.Cart.UpdateProductAmountInCart(cart, orderItem.ID, 0);
                orderItem.ProductAmount = 0;

                updateCartAction(orderItem);

                int index = Items.IndexOf(orderItem);
                Items.RemoveAt(index);
                this.DataContext = Items;
        }

        private void CloseAction()
        {
            closePrevWindow();
            Close();
        }
        private void PlaceOrder(object sender, RoutedEventArgs e)
        {
            new PlaceOrderWindow(cart, CloseAction).Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderItem orderItem in Items.ToList())
            {
                Delete(orderItem);
            }
        }
    }
}
