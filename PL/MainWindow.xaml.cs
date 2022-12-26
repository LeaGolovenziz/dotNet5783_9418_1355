
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

        private void toProductsList_Click(object sender, RoutedEventArgs e) { }

        
        private void MenuItem_Click_ShowList(object sender, RoutedEventArgs e) => new ProductList().Show();

    }
}
