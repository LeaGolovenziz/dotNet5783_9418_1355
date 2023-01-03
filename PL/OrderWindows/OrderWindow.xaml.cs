using BO;
using Dal;
using PL.ProductWindows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        public BO.Order order = new Order();

        public ObservableCollection<OrderItem> orderItems;

        private Action<OrderForList> action;

        // Constructor
        public OrderWindow(int OrderID, Action<OrderForList> action)
        {
            InitializeComponent();

            // Get the order of the window
            try
            {
                order = bl.Order.GetOrderDetails(OrderID);
            }
            catch(DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            orderDetailsGrid.DataContext = order;

            // The orderItems of the window
            orderItems = new ObservableCollection<OrderItem>(order.OrderItems);
            OrderItemsDataGrid.ItemsSource = orderItems;

            // The check box that says "the order confirmed" always checked and not enabled
            orderConfirmedcheckBox.IsChecked = true;
            orderConfirmedcheckBox.IsEnabled = false;

            // The checkBoxes of the status of the order
            if (order.OrderStatus.ToString() == "Sent")
                orderShippedcheckBox.IsChecked = true;
            else if (order.OrderStatus.ToString() == "Delivered")
            {
                orderDeliveredcheckBox.IsChecked = true;
                orderShippedcheckBox.IsChecked = true;
            }

            this.action = action;
        }

        // Updates the amount of the orderItem
        void updateOrderItem(OrderItem orderItem, int newAmount)
        {
            try
            {
                order = bl.Order.UpdateOrderDetails(orderItem.OrderID, orderItem.ID, newAmount);
                orderDetailsGrid.DataContext=order;

                int index = orderItems.IndexOf(orderItem);
                orderItems.RemoveAt(index);
                orderItem.ProductAmount = newAmount;
                orderItem.TotalPrice = newAmount * orderItem.Price;

                orderItems.Insert(index, orderItem);
            }
            catch (ProductNotInStock ex)
            {
                MessageBox.Show("There is no such amount in stock of this product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnvalidAmount ex)
            {
                MessageBox.Show("The amount you entered is unvalid, enter again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            newAmountTextBox.Clear();
        }

        private void UpdateAmountbutton_Click(object sender, RoutedEventArgs e)
        {
            // If the user entered new amount - update the orderItem
            if (newAmountTextBox.Text != "")
            {
                OrderItem orderItem = (sender as Button).DataContext as BO.OrderItem;

                updateOrderItem(orderItem, int.Parse(newAmountTextBox.Text));
            }
        }

        // If the order Delivered - prevent deliverimg it
        private void orderDeliveredcheckBox_Checked(object sender, RoutedEventArgs e)
            => orderShippedcheckBox.IsEnabled = false;

        // delegate og adding new orderItem to the order
        private void addOrderItem(Order newOrder, int productID)
        {
            order = newOrder;
            orderDetailsGrid.DataContext = order;
            orderItems.Add(order.OrderItems.FirstOrDefault(item => item.ID == productID));
        }

        // Adds an orderItem to the order
        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductList(addOrderItem, order.ID).ShowDialog();
            OrderItemsDataGrid.DataContext = orderItems;
        }


        private void DeleteOrderItembutton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).DataContext as BO.OrderItem;
            updateOrderItem(orderItem, 0);
        }

        // Allow to enter only numbers as a new amount
        private void Amountprev(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Ship the order
        private void orderShippedcheckBox_Checked(object sender, RoutedEventArgs e)
        {
            orderShippedcheckBox.IsEnabled = false;

            if (order.ShipDate == null)
            {
                try
                {
                    order = bl.Order.ShipOrder(order.ID);
                    orderDetailsGrid.DataContext = order;
                }
                catch (AlreadyShipped ex)
                {
                    MessageBox.Show("The order has been alredy shipped", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
        }

        // Deliver the order
        private void orderDeliveredcheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            orderDeliveredcheckBox.IsEnabled = false;

            if (order.DeliveryDate == null)
            {
                try
                {
                    order = bl.Order.DeliverOrder(order.ID);
                    orderDetailsGrid.DataContext = order;
                }
                catch (AlreadyDelivered ex)
                {
                    MessageBox.Show("The order has been alredy delivered", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (DoesntExist ex)
                {
                    MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Close the window, if there is changes - save them
        private void SaveButtun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                action(bl.Order.GetOrderForList(order.ID));
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("order saved!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OrderItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
