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
using Simulator;
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

        public SimulationWindow()
        {
            //DataContext = "{Binding RelativeSource={RelativeSource Self}}"

            InitializeComponent();
            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());
            OrderListView.DataContext = Orders;

            backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += BwDeliver_DoWork;
            backgroundWorker.ProgressChanged += BwDeliver_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BwDeliver_RunWorkerCompleted;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            backgroundWorker.RunWorkerAsync();
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
                    SimulatorC.startSimulation();
                    Thread.Sleep(1000);

                    // TODO: Report only when changed actually occured
                    backgroundWorker.ReportProgress((int)stopwatch.ElapsedMilliseconds);
                }
            }
        }
        private void BwDeliver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());
            OrderListView.DataContext = Orders;

            System.TimeSpan time = System.TimeSpan.FromMilliseconds(e.ProgressPercentage);

            clockTextBlock.Text = time.ToString(@"hh\:mm\:ss");

            //int precent = e.ProgressPercentage;
            //progressBar
        }
        private void BwDeliver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                SimulatorC.stopSimulation();
                MessageBox.Show("The simulation has canceled", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("The simulation has finished!", "Pay attention", MessageBoxButton.OK, MessageBoxImage.Information);

            //Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());
            //OrderListView.DataContext = Orders;

            backgroundWorker.DoWork -= BwDeliver_DoWork;
            backgroundWorker.ProgressChanged -= BwDeliver_ProgressChanged;
            backgroundWorker.RunWorkerCompleted -= BwDeliver_RunWorkerCompleted;

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

        //private void buttonStart_Click(object sender, RoutedEventArgs e)
        //{
        //    if (backgroundWorker.IsBusy != true)
        //    {
        //        this.Cursor = Cursors.Wait;
        //        backgroundWorker.RunWorkerAsync();

        //    }
        //}
    }
}