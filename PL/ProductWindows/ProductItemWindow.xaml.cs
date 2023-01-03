using BO;
using DO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductItemWindow.xaml
    /// </summary>
    public partial class ProductItemWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        private Action<ProductItem?> addToCartAction;

        private ProductItem ProductItem;

        Cart Cart;

        public ProductItemWindow(int id,Cart cart, Action<ProductItem?> action)
        {
            InitializeComponent();

            try
            { 
                ProductItem = bl.Product.GetProductFromCatalog(id, cart); 
            }
            // incase the product doesn't exists
            catch (DoesntExist ex)
            {
                MessageBox.Show("can't find the product","ERROR",MessageBoxButton.OK,MessageBoxImage.Error);    
                Close();
            }

            Cart = cart;

            this.DataContext = ProductItem;

            this.addToCartAction = action;

            // try to upload the product image if exists
            try
            {
                Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", ProductItem.Image), UriKind.Absolute); ;
                ProductImage.Source = new BitmapImage(resourceUri);
            }
            // incase there is no image
            catch (Exception ex) { }

        }
        private void closeButton_Click(object sender, RoutedEventArgs e) => this.Close();


        // add product item to cart and update its amount in the cart window
        private void addToCartButton_Click(object sender, RoutedEventArgs e) => addToCartAction(ProductItem);
        

    }
    
}
