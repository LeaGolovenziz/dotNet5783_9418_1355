using BO;
using PL.OrderWindows;
using PL.ProductWindows;
using System.IO;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PL
{

    /// <summary>
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>

    public partial class BuyerWindow : Window
    {
        public BuyerWindow()
        {
            InitializeComponent();

            // try to upload the openning video if exists
            try
            {
                Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", "PL\\images\\openningVideo.mp4"), UriKind.Absolute); ;
                video.Source = resourceUri;
            }
            // incase there is no image
            catch (Exception ex) { }
        }

        private void MenuItem_OpenCatalog(object sender, RoutedEventArgs e) => new CatalogWindow().ShowDialog();

        private void MenuItem_TrackOrder(object sender, RoutedEventArgs e) => new TrackingWindow().ShowDialog();

    }
}
