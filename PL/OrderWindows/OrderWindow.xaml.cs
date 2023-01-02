using BO;
using Dal;
using PL.ProductWindows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public Order order;

        public ObservableCollection<OrderItem> orderItems;

        private Action<OrderForList> action;
        public OrderWindow(int OrderID, Action<OrderForList> action)
        {
            InitializeComponent();

            order = bl.Order.GetOrderDetails(OrderID);
            orderDetailsGrid.DataContext = order;

            // The check box that says "the order confirmed" always checked and not enabled
            orderConfirmedcheckBox.IsChecked = true;
            orderConfirmedcheckBox.IsEnabled = false;

            // The checkBoxes of the status of the order
            if (order.OrderStatus.ToString() == "Sent")
            {
                orderShippedcheckBox.IsChecked = true;
                orderShippedcheckBox.IsEnabled = false;
            }
            else if (order.OrderStatus.ToString() == "Delivered")
            {
                orderShippedcheckBox.IsChecked = true;
                orderDeliveredcheckBox.IsChecked = true;

                orderShippedcheckBox.IsEnabled = false;
                orderDeliveredcheckBox.IsEnabled = false;
            }


            orderItems = new ObservableCollection<OrderItem>(order.OrderItems);
            OrderItemsDataGrid.ItemsSource = orderItems;

            this.action = action;
        }

        private void UpdateAmountbutton_Click(object sender, RoutedEventArgs e)
        {

            if (newAmountTextBox.Text != "")
            {
                var orderItem = (sender as Button).DataContext as BO.OrderItem;

                if (int.Parse(newAmountTextBox.Text) == 0)
                {

                }
                else
                {
                    try
                    {
                        order = bl.Order.UpdateOrderDetails(orderItem.OrderID, orderItem.ID, int.Parse(newAmountTextBox.Text));
                        orderItems = new ObservableCollection<OrderItem>(order.OrderItems);

                        OrderItemsDataGrid.ItemsSource = orderItems;
                        orderDetailsGrid.DataContext = order;

                        newAmountTextBox.Clear();
                    }
                    catch (ProductNotInStock ex)
                    {
                        MessageBox.Show("There is no such amount in stock", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (UnvalidAmount ex)
                    {
                        MessageBox.Show("The amount you entered is unvalid, enter again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void orderShippedcheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void orderDeliveredcheckBox_Checked(object sender, RoutedEventArgs e)
        {
            orderShippedcheckBox.IsEnabled = false;
        }

        private void newAmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addOrderItem(Order newOrder)
            => order = newOrder;
        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductList(addOrderItem, order.ID).ShowDialog();
        }

        private void SaveButtun_Click(object sender, RoutedEventArgs e)
        {
            if (orderShippedcheckBox.IsEnabled == true && orderShippedcheckBox.IsChecked == true && orderDeliveredcheckBox.IsEnabled == false)
                try
                {
                    order = bl.Order.ShipOrder(order.ID);
                }
                catch (AlreadyShipped ex)
                {
                    MessageBox.Show("The order has been alredy shipped", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            else if (orderDeliveredcheckBox.IsEnabled == true && orderDeliveredcheckBox.IsChecked == true)
                try
                {
                    order = bl.Order.DeliverOrder(order.ID);
                }
                catch (AlreadyDelivered ex)
                {
                    MessageBox.Show("The order has been alredy delivered", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }

            action(bl.Order.GetOrderForList(order.ID));
            MessageBox.Show("order saved!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
