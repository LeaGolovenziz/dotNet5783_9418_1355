
using PL.ProductWindows;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e) => new ManagerWindow().ShowDialog();

        private void Button_Click_1(object sender, RoutedEventArgs e) => new BuyerWindow().ShowDialog();


    }
}
