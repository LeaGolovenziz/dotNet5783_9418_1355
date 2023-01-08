using BO;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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

        public ProductItemWindow(int id, Cart cart, Action<ProductItem?> action)
        {
            InitializeComponent();

            try
            {
                ProductItem = bl.Product.GetProductFromCatalog(id, cart);
            }
            // incase the product doesn't exists
            catch (DoesntExist)
            {
                MessageBox.Show("can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (FileSavingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileLoadingError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (XmlFormatError)
            {
                MessageBox.Show("we are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
