using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;


namespace PL
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        private BO.Order order;

        BackgroundWorker backgroundWorker;
        public ObservableCollection<OrderForList?> Orders { get; set; }

        private volatile bool stopSimulation;

        public SimulationWindow()
        {
            InitializeComponent();

            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList().OrderBy(x=>x.CustomerName));
            OrderListView.DataContext = Orders;

            backgroundWorker = new BackgroundWorker();

            // If all of the orders has delivered - don't allow to simulate
            if (bl.Order.GetOrderList().ToList().Exists(order => order.OrderStatus == BO.Enums.OrderStatus.Sent || order.OrderStatus == BO.Enums.OrderStatus.Confirmed))
            {
                backgroundWorker.DoWork += BwDeliver_DoWork!;
                backgroundWorker.ProgressChanged += BwDeliver_ProgressChanged!;
                backgroundWorker.RunWorkerCompleted += BwDeliver_RunWorkerCompleted!;

                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;
            }
            else
                buttonStart.Visibility = Visibility.Hidden;
        }


        private void BwDeliver_DoWork(object sender, DoWorkEventArgs e)
        {
            // Start to run the clock
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // While there are orders that havn't delivered - (send) and deliver them
            while (bl.Order.GetOrderList().ToList().Exists(order => order.OrderStatus == BO.Enums.OrderStatus.Sent || order.OrderStatus == BO.Enums.OrderStatus.Confirmed))
            {
                if (backgroundWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Thread.Sleep(1000);

                    backgroundWorker.ReportProgress((int)stopwatch.ElapsedMilliseconds);
                }
            }
        }

        // mixing the list of orders
        private IEnumerable<OrderForList?> shuffel()
        {
            int n = Orders.Count;
            Random random = new Random(); 
            
            while(n>1)
            {
                n--;
                int k = random.Next(n + 1);
                OrderForList? orderForList = Orders[k];
                Orders[k] = Orders[n];
                Orders[n] = orderForList;
            }
            return Orders;
        }

        private void BwDeliver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int currentTime = e.ProgressPercentage;

            // Go over the orders - if there is an order that hasn't been sent or delivered - do it
            foreach (BO.OrderForList orderForList in shuffel())
            {
                if (stopSimulation)
                    break;

                BO.Order order = bl.Order.GetOrderDetails(orderForList.OrderID);

                if (order.DeliveryDate == null)
                {
                    if (order.ShipDate != null && (order.ShipDate - order.OrderDate).Value.TotalDays <= currentTime)
                    {
                        bl.Order.DeliverOrder(order.ID);
                    }
                    else if (order.OrderDate != null && (order.OrderDate - DateTime.Now).Value.TotalDays <= currentTime)
                    {
                        bl.Order.ShipOrder(order.ID);
                    }
                    Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList().OrderBy(x => x.CustomerName));
                    OrderListView.DataContext = Orders;

                    System.TimeSpan time = System.TimeSpan.FromMilliseconds(e.ProgressPercentage);
                    clockTextBlock.Text = time.ToString(@"hh\:mm\:ss");

                    break;
                }
            }
        }
        private void BwDeliver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                buttonStart.Visibility = Visibility.Visible;
                stopSimulation = true;
                MessageBox.Show("The simulation has canceled", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                buttonStart.Visibility = Visibility.Hidden;
                MessageBox.Show("The simulation has finished!", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.Cursor = Cursors.Arrow;

            buttonStop.Visibility = Visibility.Hidden;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation == true)
            {
                backgroundWorker.CancelAsync();
                buttonStop.Visibility = Visibility.Hidden;
            }
        }

        private void histoyButton_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderForList orderForList = (sender as Button).DataContext as BO.OrderForList;

            if (orderForList != null)
            {
                order = bl.Order.GetOrderDetails(orderForList.OrderID);
                MessageBox.Show("Order's details:");
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy != true)
            {
                stopSimulation = false;
                buttonStop.Visibility = Visibility.Visible;
                buttonStart.Visibility = Visibility.Hidden;

                this.Cursor = Cursors.Wait;
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation == true)
            {
                backgroundWorker.CancelAsync();
                backgroundWorker.DoWork -= BwDeliver_DoWork!;
                backgroundWorker.ProgressChanged -= BwDeliver_ProgressChanged!;
                backgroundWorker.RunWorkerCompleted -= BwDeliver_RunWorkerCompleted!;
            }
            Thread.Sleep(1000);
        }
    }
}