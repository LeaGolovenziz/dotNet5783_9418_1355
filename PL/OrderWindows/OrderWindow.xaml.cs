using BO;
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

                orderDetailsGrid.DataContext = order;

                // The orderItems of the window
                orderItems = new ObservableCollection<OrderItem>(order.OrderItems);
                OrderItemsDataGrid.ItemsSource = orderItems;

                // The checkBoxes of the status of the order
                if (order.OrderStatus.ToString() == "Sent")
                {
                    orderShippedcheckBox.IsChecked = true;
                    SaveButtun.Content = "close";
                }
                else if (order.OrderStatus.ToString() == "Delivered")
                {
                    orderDeliveredcheckBox.IsChecked = true;
                    orderShippedcheckBox.IsChecked = true;
                    SaveButtun.Content = "close";
                }

                this.action = action;
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileSavingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileLoadingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (XmlFormatError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        // Updates the amount of the orderItem
        void updateOrderItem(OrderItem orderItem, int newAmount)
        {
            try
            {
                double? price = bl.Order.UpdateOrderDetails(orderItem.OrderID, orderItem.ID, newAmount).Price;
                order.Price = price;

                int index = orderItems.IndexOf(orderItem);
                orderItems.RemoveAt(index);
                orderItem.ProductAmount = newAmount;
                orderItem.TotalPrice = newAmount * orderItem.Price;

                orderItems.Insert(index, orderItem);
            }
            catch (ProductNotInStock ex)
            {
                MessageBox.Show("There is no such amount in stock of this product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (UnvalidAmount ex)
            {
                MessageBox.Show("The amount you entered is unvalid, enter again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileSavingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileLoadingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (XmlFormatError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            else
                MessageBox.Show("Enter an amount in the text box", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // If the order Delivered - prevent deliverimg it
        private void orderDeliveredcheckBox_Checked(object sender, RoutedEventArgs e)
            => orderShippedcheckBox.IsEnabled = false;

        // delegate og adding new orderItem to the order
        private void addOrderItem(Order newOrder, int productID)
        {
            order = newOrder;
            orderDetailsGrid.DataContext = order;

            // updates the collection with the new updated order item
            OrderItem newOrderItem = order.OrderItems.FirstOrDefault(x => x.ID == productID)!;
            OrderItem oLDorderItem = orderItems.FirstOrDefault(x => x.ID == productID)!;
            orderItems.Remove(oLDorderItem);
            orderItems.Add(newOrderItem);

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
                    bl.Order.ShipOrder(order.ID);
                    order.OrderStatus = BO.Enums.OrderStatus.Sent;
                    order.ShipDate = DateTime.Now;
                }
                catch (AlreadyShipped ex)
                {
                    MessageBox.Show("The order has been alredy shipped", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (FileSavingError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (FileLoadingError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (XmlFormatError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    bl.Order.DeliverOrder(order.ID);
                    order.OrderStatus = BO.Enums.OrderStatus.Delivered;
                    order.DeliveryDate = DateTime.Now;
                }
                catch (AlreadyDelivered ex)
                {
                    MessageBox.Show("The order has been alredy delivered", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (DoesntExist ex)
                {
                    MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (FileSavingError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (FileLoadingError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                catch (XmlFormatError)
                {
                    MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
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
                Close();
            }
            catch (FileSavingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileLoadingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (XmlFormatError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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
