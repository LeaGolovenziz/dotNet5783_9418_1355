using PL;
using PL.OrderWindows;
using PL.ProductWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>
     
    public partial class BuyerWindow : Window
    { 
        public BuyerWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) => new CatalogWindow().Show();

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) => new TrackingWindow().Show();
        
    }
}
