using BO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using static BO.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for TrackingWindow.xaml
    /// </summary>
    public partial class TrackingWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public OrderTracking orderTracking;
        public List<Tuple<DateTime?, OrderStatus?>> Tracking;

        public TrackingWindow()
        {
            InitializeComponent();
        }

        private void trackOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // If the order ID is too short
            if (OrderIDTextBox.Text.Length!=6)
            {
                MessageBox.Show("The ID is unvalid, enter 6 digits", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                try
                {
                    orderTracking = bl.Order.TrackOrder(int.Parse(OrderIDTextBox.Text));
                    Tracking = orderTracking.Tracking;
                    
                    StatusGridView.Visibility = Visibility.Visible;
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
            OrderIDTextBox.Clear();
        }

        private void IDprev(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numbers
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
