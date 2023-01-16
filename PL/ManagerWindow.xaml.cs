using PL.OrderWindows;
using PL.ProductWindows;
using System;
using System.IO;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();

            // try to upload the openning video if exists
            try
            {
                Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", "PL\\images\\openningVideo.mp4"), UriKind.Absolute);
                video.Source = resourceUri;
            }
            // incase there is no image
            catch (Exception ex) { }
        }
        private void MenuItem_Click_ShowProductList(object sender, RoutedEventArgs e) => new ProductList().Show();


        private void MenuItem_Click_ShowOrderList(object sender, RoutedEventArgs e) => new OrderList().Show();

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // to prevent failure
        }

        private void MenuItem_Click_ShowOrderTracking(object sender, RoutedEventArgs e)=>new SimulationWindow().Show();
    }
}
