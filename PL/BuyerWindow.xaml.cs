using PL.OrderWindows;
using PL.ProductWindows;
using System.Windows;

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
        }

        private void MenuItem_OpenCatalog(object sender, RoutedEventArgs e) => new CatalogWindow().ShowDialog();

        private void MenuItem_TrackOrder(object sender, RoutedEventArgs e) => new TrackingWindow().ShowDialog();

    }
}
