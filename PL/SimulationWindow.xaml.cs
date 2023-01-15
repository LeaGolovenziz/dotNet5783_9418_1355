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
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if(backgroundWorker.IsBusy!=true)
            {
                    this.Cursor = Cursors.Wait;
                    backgroundWorker.RunWorkerAsync();
            }
        }

        private void buttunCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BwDeliver_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while(true/*bl.Order.GetOrderList().ToList().Exists(order=> order.OrderStatus==BO.Enums.OrderStatus.Sent || order.OrderStatus == BO.Enums.OrderStatus.Confirmed)*/)
            {
                if (backgroundWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    SimulatorC.startSimulation();
                    Thread.Sleep(500);

                    if (backgroundWorker.WorkerReportsProgress == true)
                        backgroundWorker.ReportProgress((int)stopwatch.ElapsedMilliseconds);
                }
            }
        }
        private void BwDeliver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar

            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());
            OrderListView.DataContext = Orders;

            System.TimeSpan time = System.TimeSpan.FromMilliseconds(e.ProgressPercentage);
            String timeText = time.ToString();

            //int precent = e.ProgressPercentage;

            ////progressBar

            //if (precent > 50)
            //    (sender as OrderForList)!.OrderStatus = Enums.OrderStatus.Delivered;
        }
        private void BwDeliver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                SimulatorC.stopSimulation();
            }

            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());
            OrderListView.DataContext = Orders;
            


        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation)
                backgroundWorker.CancelAsync();
        }
    }
}
