using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using static BO.Enums;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for TrackingWindow.xaml
    /// </summary>
    public partial class TrackingWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public OrderTracking orderTracking;
        public IEnumerable<OrderForList?> orders;
        public List<Tuple<DateTime?, OrderStatus?>> Tracking;

        User user;

        public TrackingWindow(User user)
        {
            InitializeComponent();

            orders = bl.Order.GetOrderList().Where(x => x.CustomerID == user.ID);
            OrderListView.DataContext = orders;

            this.user = user;
        }

        private void OrderListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OrderForList orderForList=OrderListView.SelectedItem as OrderForList;

            // If the order ID is too short
            if (orderForList != null)
            {
                try
                {
                    orderTracking = bl.Order.TrackOrder(orderForList.OrderID);
                    Tracking = orderTracking.Tracking;

                    trackingGrid.Visibility = Visibility.Visible;
                    trackingListView.Visibility = Visibility.Visible;

                    StatusGridView.DataContext = orderTracking;
                    trackingListView.DataContext = Tracking;
                }
                catch (DoesntExist ex)
                {
                    MessageBox.Show("There is no order with this ID", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        private void IDprev(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numbers
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OrderListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

    }
}
