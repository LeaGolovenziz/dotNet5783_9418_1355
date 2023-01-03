using BO;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public ObservableCollection<OrderForList?> Orders;

        void resetOrders()
        {
            Orders = new ObservableCollection<OrderForList?>(from item in bl.Order.GetOrderList()
                                                             orderby item.OrderID
                                                             select item);

            this.DataContext = Orders;
        }

        // Constructor
        public OrderList()
        {
            InitializeComponent();

            resetOrders();

            OrderStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.OrderStatus));
        }

        // Filtering the orders by a certain status of order
        private void OrderStatusSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OrderStatusSelector.SelectedItem != null)
            {
                Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList().Where(order => order?.OrderStatus == (BO.Enums.OrderStatus)OrderStatusSelector.SelectedItem));
            }
            this.DataContext = Orders;
        }

        // Canceling the filter by status
        private void button_Click(object sender, RoutedEventArgs e)
        {
            resetOrders();
            OrderStatusSelector.SelectedItem = null;
        }

        // Delegate of updating an order
        private void UpdateOrder(OrderForList orderForList)
        {
            var item = Orders.FirstOrDefault(order => order.OrderID == orderForList.OrderID);
            if(item != null)
                Orders[Orders.IndexOf(item)] = orderForList;
        }

        // Update an order 
        private void OrderListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // get the Order selected from the list
            OrderForList orderForList = (OrderForList)OrderListView.SelectedItem;

            if(orderForList!=null)
            {
                new OrderWindow(orderForList.OrderID, UpdateOrder).ShowDialog();
            }
        }

        private void OrderListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // To prevent failure
        }
    }
}
