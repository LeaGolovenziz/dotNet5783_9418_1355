using BO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PL.CartWindows
{
    /// <summary>
    /// Interaction logic for PlaceOrderWindow.xaml
    /// </summary>
    public partial class PlaceOrderWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public Cart cart;

        public User user=new User();

        private Action action;

        public PlaceOrderWindow(Cart cart, Action action)
        {
            InitializeComponent();

            this.cart = cart;

            this.action = action;

            this.DataContext = this.cart;

        }

        // make acxeption visible if text is empty anf unvisible otherwise
        private void CustomerNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                CustomerNameExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                CustomerNameExceptionLable.Visibility = Visibility.Hidden;
            }
        }

        // make acxeption visible if text is empty anf unvisible otherwise
        private void CustomerAddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                CustomerAddressExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                CustomerAddressExceptionLable.Visibility = Visibility.Hidden;
            }
        }

        // make acxeption visible if text is empty anf unvisible otherwise
        private void CustomerEmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                CustomerEmailExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                CustomerEmailExceptionLable.Visibility = Visibility.Hidden;
            }
        }

        // place order
        private void PlaceOrder(object sender, RoutedEventArgs e)
        {
            // if all details hasn't entered show message
            if (CustomerAddressExceptionLable.Visibility == Visibility.Visible ||
                CustomerEmailExceptionLable.Visibility == Visibility.Visible ||
                CustomerNameExceptionLable.Visibility == Visibility.Visible)
            {
                MessageBox.Show("please enter all details!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else try
                {
                    // place order
                    int orderID = bl.Cart.PlaceOrder(cart, user);
                    MessageBox.Show("Your order has been confirmed! \nyour tracking number is " + orderID, "ATTENTION", MessageBoxButton.OK, MessageBoxImage.Information);

                    // close two previouse windows
                    action();

                    this.Close();

                }
                catch (UnvalidEmail ex)
                {
                    MessageBox.Show("your email address is not valid!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    CustomerEmailTextBox.Clear();
                    CustomerEmailExceptionLable.Visibility = Visibility.Visible;
                }
                catch (UnvalidName)
                {
                    MessageBox.Show("your name is not valid!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    CustomerEmailTextBox.Clear();
                    CustomerNameExceptionLable.Visibility = Visibility.Visible;
                }
                catch (UnvalidAddress)
                {
                    MessageBox.Show("your name is not valid!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    CustomerEmailTextBox.Clear();
                    CustomerEmailExceptionLable.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("can't find one of the products", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

        }

    }
}
