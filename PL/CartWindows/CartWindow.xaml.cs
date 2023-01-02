using BlApi;
using BO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace PL.CartWindows
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<OrderItem> Items { get; set; }

        private Cart cart;

        private Action action;

        //struct NewType 
        //{
        //    public string? Name;
        //    public double? Price;
        //    public int? ProductAmount;
        //    public double? TotalPrice;
        //    public string? Image;
        //    public BO.Enums.Category? Category;
        //}

        public CartWindow(Cart cart, Action action)
        {
            InitializeComponent();
            
                Items = new ObservableCollection<OrderItem>(cart.OrderItems);
                ItemListView.DataContext = Items;
                totalPriceLable.DataContext = cart;

                this.cart = cart;

                this.action = action;

                totalPriceLable.Content = cart.OrderItems.Sum(x => x.TotalPrice);
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(items);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
            //view.GroupDescriptions.Add(groupDescription);

        }

        private void ItemListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(ItemListView.SelectedItem==null)
            {
                SelectedItemGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                SelectedItemGrid.Visibility = Visibility.Visible;
                OrderItem selectedItem = (OrderItem)ItemListView.SelectedItem;
                nameTextBox.Text = selectedItem.Name;
                priceTextBox.Text = selectedItem.Price.ToString();
                inCartTextBox.Text = selectedItem.ProductAmount.ToString();
                TotalPriceTextBox.Text = selectedItem.TotalPrice.ToString();
                try
                {
                    Uri resourceUri = new Uri(bl.Product.GetProductDetails(selectedItem.ID).Image, UriKind.Absolute);
                    ProductImage.Source = new BitmapImage(resourceUri);
                }
                // incase there is no image
                catch (Exception ex) { }

            }
        }

        private void ADD(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)ItemListView.SelectedItem;

            if (orderItem != null)
                if (bl.Product.GetProductDetails(orderItem.ID).InStock - orderItem.ProductAmount > 0)
                {
                    bl.Cart.AddProductToCart(cart, orderItem.ID);

                    int index = Items.IndexOf(orderItem);
                    orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                    Items.RemoveAt(index);
                    Items.Insert(index, orderItem);
                    this.DataContext = Items;
                }
                else
                    MessageBox.Show("product is not in stock");
        }

        private void SUB(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)ItemListView.SelectedItem;

            if (orderItem != null)
            {
                bl.Cart.UpdateProductAmountInCart(cart,orderItem.ID,(int)(orderItem.ProductAmount - 1)!);

                int index = Items.IndexOf(orderItem);
                orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                if (orderItem.ProductAmount > 0)
                {
                    Items.RemoveAt(index);
                    Items.Insert(index, orderItem);
                }
                else
                    Items.RemoveAt(index);
                this.DataContext = Items;
            }
        }

        private void DELETE(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)ItemListView.SelectedItem;

            if (orderItem != null)
            {
                bl.Cart.UpdateProductAmountInCart(cart, orderItem.ID, 0);

                int index = Items.IndexOf(orderItem);
                Items.RemoveAt(index);
                this.DataContext = Items;
            }
        }

        private void CloseAction()
        {
            action();
            Close();
        }
        private void PlaceOrder(object sender, RoutedEventArgs e)
        { 
            new PlaceOrderWindow(cart, CloseAction).Show();

        }
    }
}
