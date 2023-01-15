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
            Orders = new ObservableCollection<OrderForList?>(bl.Order.GetOrderList());

            InitializeComponent();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerAsync();
            //backgroundWorker.DoWork += BwDeliver_DoWork;
            //backgroundWorker.ProgressChanged += BwDeliver_ProgressChanged;
            //backgroundWorker.RunWorkerCompleted += BwDeliver_RunWorkerCompleted;

            //backgroundWorker.WorkerReportsProgress = true;  
            //backgroundWorker.WorkerSupportsCancellation = true; 
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if(backgroundWorker.IsBusy!=true)
            {
                foreach (OrderForList order in Orders)
                {
                    BackgroundWorker b= new BackgroundWorker();
                    b.DoWork += BwDeliver_DoWork;
                    b.ProgressChanged += BwDeliver_ProgressChanged;
                    b.RunWorkerCompleted += BwDeliver_RunWorkerCompleted;
                    this.Cursor = Cursors.Wait;
                    b.RunWorkerAsync(order);
                }
            }
        }
        private void BwDeliver_DoWork(object sender, DoWorkEventArgs e)
        {
            //Simulator.StartSimulation();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int timeToDelivery = (int)(e.Argument as OrderForList)!.OrderStatus!+10;

            for (int i = 0; i < timeToDelivery; i++)
            {
                if (backgroundWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Thread.Sleep(500);

                    if (backgroundWorker.WorkerReportsProgress == true)
                        backgroundWorker.ReportProgress(i * 100 / timeToDelivery);
                }
            }
        }
        private void BwDeliver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int precent = e.ProgressPercentage;

            //progressBar

            if (precent > 50)
                (sender as OrderForList)!.OrderStatus = Enums.OrderStatus.Delivered;
        }
        private void BwDeliver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            { 
                int index = Orders.IndexOf(e.Result as OrderForList);
                Orders.RemoveAt(index);
                (e.Result as OrderForList)!.OrderStatus = Enums.OrderStatus.Delivered;
                bl.Order.DeliverOrder((e.Result as OrderForList).OrderID);
                Orders.Insert(index, e.Result as OrderForList);
            }

        }
    }
}
