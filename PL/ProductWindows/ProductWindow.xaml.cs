using BO;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public BO.Product newProduct = new BO.Product();
        private Action<ProductForList> action;

        // constructor
        public ProductWindow()
        {
            InitializeComponent();
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        // constructor for add product window
        public ProductWindow(Action<ProductForList> action):this()
        {
            this.action = action;

            mainGrid.DataContext = newProduct;

            updateButton.Visibility = Visibility.Hidden;
        }

        // constructor for update product window
        public ProductWindow(Action<ProductForList> action,int id):this()
        {
            newProduct = bl.Product.GetProductDetails(id);

            mainGrid.DataContext = newProduct;

            // try to upload the product image if exists
            // try by name
            try
            {
                Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", newProduct.Image), UriKind.Absolute); ;
                ProductImage.Source = new BitmapImage(resourceUri);
            }
            // incase there is no image with this name
            catch (Exception) 
            {
                // try with full path
                try
                {
                    Uri resourceUri = new Uri(newProduct.Image, UriKind.Absolute); ;
                    ProductImage.Source = new BitmapImage(resourceUri);
                }
                // incase there is no image with this path
                catch (Exception) { }
            }

            this.action = action;

            addButton.Visibility = Visibility.Hidden;

            idTextBox.IsEnabled = false;
        }

        // checks the textBoxes of the product's details
        bool checkTextBoxes()
        {
            bool flag = true;
            if (idTextBox.Text.Length != 6)
            {
                idExceptionLable.Visibility = Visibility.Visible;
                flag = false;
            }
            else
                idExceptionLable.Visibility = Visibility.Hidden;

            if (categoryComboBox.SelectedValue == null)
            {
                isCategoryLable.Visibility = Visibility.Visible;
                flag = false;
            }
            else
                isCategoryLable.Visibility = Visibility.Hidden;

            if (nameTextBox.Text.Length == 0)
            {
                nameExceptionLable.Visibility = Visibility.Visible;
                flag = false;
            }
            else
                nameExceptionLable.Visibility = Visibility.Hidden;

            if (priceTextBox.Text.Length == 0)
            {
                priceExceptionLable.Visibility = Visibility.Visible;
                flag = false;
            }
            else
                priceExceptionLable.Visibility = Visibility.Hidden;

            if (inStockTextBox.Text.Length == 0)
            {
                inStockExceptionLable.Visibility = Visibility.Visible;
                flag = false;
            }
            else
                inStockExceptionLable.Visibility = Visibility.Hidden;

            return flag;
        }

        // Updates the product with the new details that in the textBoxs
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkTextBoxes())
            {
                try
                {
                    bl.Product.UpdateProduct(newProduct);

                    action(bl.Product.GetProductForList(newProduct.ID));

                    MessageBox.Show("product updated!");
                    Close();
                }
                catch (DoesntExist )
                {
                    MessageBox.Show("Can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
        }

        // Adds a product with the details that in the textBoxs
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkTextBoxes())
            {
                try
                {
                    bl.Product.AddProduct(newProduct);

                    action(bl.Product.GetProductForList(newProduct.ID));

                    MessageBox.Show("product added!");
                    this.Close();
                }
                catch (IdAlreadyExist )
                {
                    MessageBox.Show("There is already a product with this ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }

        }

        // makes sure the user can enter only numbers
        private void allowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // make sure the user can enter only letters as name
        private void allowOnlyLetters(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                ProductImage.Source = bitmap;
                newProduct.Image = dlg.FileName;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}



