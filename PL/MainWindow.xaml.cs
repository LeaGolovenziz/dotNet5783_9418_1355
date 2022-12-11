using BlApi;
using Bllmplementation;
using PL.ProductWindows;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBl bl = new Bl();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void toProductsList_Click(object sender, RoutedEventArgs e) => new ProductList().Show();

    }
}
