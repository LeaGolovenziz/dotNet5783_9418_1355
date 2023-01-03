
using PL.ProductWindows;
using System.IO;
using System;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public MainWindow()
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

        private void Button_Click(object sender, RoutedEventArgs e) => new ManagerWindow().ShowDialog();

        private void Button_Click_1(object sender, RoutedEventArgs e) => new BuyerWindow().ShowDialog();


    }
}
