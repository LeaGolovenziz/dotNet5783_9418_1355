using BO;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
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

        public Cart cart = new Cart();

        public OrderItem orderItem = new OrderItem();

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

        // show one products details and give option to update its amount
        private void ItemListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ItemListView.SelectedItem != null)
            {
                // shoe products details
                SelectedItemGrid.Visibility = Visibility.Visible;

                orderItem = (OrderItem)ItemListView.SelectedItem;
                SelectedItemGrid.DataContext = orderItem;

                // try to upload the product image if exists
                try
                {
                    Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", bl.Product.GetProductDetails(orderItem.ID).Image), UriKind.Absolute);
                    ProductImage.Source = new BitmapImage(resourceUri);
                }
                // incase there is no image
                catch (Exception ex) { }

            }
        }

        // add amount of product
        private void AddButton(object sender, RoutedEventArgs e)
        {
            if (orderItem != null)
                try
                {
                    // add product to cart
                    bl.Cart.AddProductToCart(cart, orderItem.ID);

                    // find order item index
                    int index = Items.IndexOf(orderItem);

                    // update amount in catalog window
                    orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                    updateCartAction(orderItem);

                    // update order items collection
                    orderItem.TotalPrice = orderItem.ProductAmount * orderItem.Price;
                    Items.RemoveAt(index);
                    Items.Insert(index, orderItem);
                }
                catch (ProductNotInStock ex)
                {
                    MessageBox.Show("product is not in stock!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (DoesntExist)
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
        private void SubButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (orderItem != null)
                {
                    // remove 1 from product amount in cart 
                    bl.Cart.UpdateProductAmountInCart(cart, orderItem.ID, (int)(orderItem.ProductAmount - 1)!);

                    // find order item index
                    int index = Items.IndexOf(orderItem);

                    // update amount in catalog window
                    orderItem.ProductAmount = cart.OrderItems.Find(x => x.ID == orderItem.ID)?.ProductAmount;
                    updateCartAction(orderItem);

                    // update order items collection
                    // if amount is positive update amount
                    if (orderItem.ProductAmount > 0)
                    {
                        Items.RemoveAt(index);
                        Items.Insert(index, orderItem);
                    }
                    // if amont is 0 delete from collection
                    else
                    {
                        Items.RemoveAt(index);
                        SelectedItemGrid.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (ProductNotInStock)
            {
                MessageBox.Show("product is not in stock!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DoesntExist)
            {
                MessageBox.Show("can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnvalidAmount)
            {
                MessageBox.Show("unvalid product's amount", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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

        // delete product from cart
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            if (orderItem != null)
            {
                Delete(orderItem);
                SelectedItemGrid.Visibility = Visibility.Hidden;
            }

        }

        // deleting product from cart
        private void Delete(OrderItem orderItem)
        {
            try
            {
                // update product amount in cart to 0
                bl.Cart.UpdateProductAmountInCart(cart, orderItem.ID, 0);
                orderItem.ProductAmount = 0;

                // apdate product amount in catalog window
                updateCartAction(orderItem);

                // update order items collection
                int index = Items.IndexOf(orderItem);
                Items.RemoveAt(index);
            }
            catch (ProductNotInStock ex)
            {
                MessageBox.Show("product is not in stock!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnvalidAmount ex)
            {
                MessageBox.Show("unvalid product's amount", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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

        // action that closes all the this and privious window 
        private void CloseAction()
        {
            closePrevWindow();
            Close();
        }

        // open place order window
        private void PlaceOrder(object sender, RoutedEventArgs e) => new PlaceOrderWindow(cart, CloseAction).ShowDialog();


        // clear cart
        private void button_clearCart(object sender, RoutedEventArgs e)
        {
            foreach (OrderItem orderItem in Items.ToList())
            {
                Delete(orderItem);
            }
        }
    }
}
