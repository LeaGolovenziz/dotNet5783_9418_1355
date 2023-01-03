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
        public OrderWindow(int OrderID, Action<OrderForList> action)
        {
            InitializeComponent();

            // The order of the window
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

            this.action = action;
        }

        void updateOrderItem(OrderItem orderItem, int newAmount)
        {
            // Update the amount of the orderItem
            try
            {
                Order temporder = bl.Order.UpdateOrderDetails(orderItem.OrderID, orderItem.ID, newAmount);

                order.Price = temporder.Price;

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

            if (newAmountTextBox.Text != "")
            {
                OrderItem orderItem = (sender as Button).DataContext as BO.OrderItem;

                updateOrderItem(orderItem, int.Parse(newAmountTextBox.Text));
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

        private void addOrderItem(Order newOrder, int productID)
        {
            order = newOrder;
            orderItems.Add(order.OrderItems.FirstOrDefault(item => item.ID == productID));
        }
        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductList(addOrderItem, order.ID).ShowDialog();
            OrderItemsDataGrid.DataContext = orderItems;
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
                catch (DoesntExist ex)
                {
                    MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

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


        private void subProductButton_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteOrderItembutton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).DataContext as BO.OrderItem;
            updateOrderItem(orderItem, 0);
        }

        private void Amountprev(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numbers
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OrderItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void orderShippedcheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            shipDateTextBlock.Text = DateTime.Now.ToString();
        }

        private void orderDeliveredcheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            deliveryDateTextBlock.Text = DateTime.Now.ToString();
        }
    }
}
