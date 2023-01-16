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

            backgroundWorker.DoWork += BwDeliver_DoWork!;
            backgroundWorker.ProgressChanged += BwDeliver_ProgressChanged!;
            backgroundWorker.RunWorkerCompleted += BwDeliver_RunWorkerCompleted!;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (buttonClose.Visibility == Visibility.Hidden)
                e.Cancel = true;
        }


        private void BwDeliver_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
        private void BwDeliver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int currentTime = e.ProgressPercentage;

            foreach (BO.OrderForList orderForList in Orders)
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
                stopSimulation = true;   
                MessageBox.Show("The simulation has canceled", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("The simulation has finished!", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);

            buttonStop.Visibility = Visibility.Hidden;
            buttonClose.Visibility = Visibility.Visible;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation == true)
                backgroundWorker.CancelAsync();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            
            backgroundWorker.DoWork -= BwDeliver_DoWork!;
            backgroundWorker.ProgressChanged -= BwDeliver_ProgressChanged!;
            backgroundWorker.RunWorkerCompleted -= BwDeliver_RunWorkerCompleted!;
            this.Close();
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

                this.Cursor = Cursors.Wait;
                backgroundWorker.RunWorkerAsync();
            }
        }
    }
}