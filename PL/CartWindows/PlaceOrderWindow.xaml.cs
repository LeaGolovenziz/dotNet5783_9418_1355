using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
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

namespace PL.CartWindows
{
    /// <summary>
    /// Interaction logic for PlaceOrderWindow.xaml
    /// </summary>
    public partial class PlaceOrderWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        private Cart cart;

        private Action action;

        public PlaceOrderWindow(Cart cart, Action action)
        {
            InitializeComponent();

            this.cart = cart;

            this.action = action;

            this.DataContext = this.cart;

        }

        private void CustomerNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text=="")
            {
                CustomerNameExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                CustomerNameExceptionLable.Visibility = Visibility.Hidden;
            }
        }

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

        private void CustomerEmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                CustomerEmailExceptionLable.Content = "please enter your Email address!";
                CustomerEmailExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                CustomerEmailExceptionLable.Visibility = Visibility.Hidden;
            }
        }

        private void PlaceOrder(object sender, RoutedEventArgs e)
        {
            if(CustomerAddressExceptionLable.Visibility == Visibility.Visible||
                CustomerEmailExceptionLable.Visibility==Visibility.Visible||
                CustomerNameExceptionLable.Visibility==Visibility.Visible)
            {
                MessageBox.Show("please enter all details!");
            }
            else try
                {
                    int orderID = bl.Cart.PlaceOrder(cart);
                    MessageBox.Show("Your order has been confirmed! \nyour tracking number is "+ orderID);
                    action();
                    this.Close();

                }
                catch (UnvalidEmail ex)
                {
                    CustomerEmailExceptionLable.Content = "Unvalid Email address!";
                    CustomerEmailExceptionLable.Visibility = Visibility.Visible;
                }
        }

    }
}
