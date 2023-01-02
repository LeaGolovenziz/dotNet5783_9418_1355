using BO;
using Microsoft.Win32;
using System;
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

        bool checkTextBoxes()
        {
            if (idTextBox.Text.Length != 6)
            {
                idExceptionLable.Content = "Enter 6 digits!";
                idExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if (categoryComboBox.SelectedValue == null)
            {
                isCategoryLable.Visibility = Visibility.Visible;
                return false;
            }
            if (nameTextBox.Text.Length == 0)
            {
                nameExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if (priceTextBox.Text.Length == 0)
            {
                priceExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if (inStockTextBox.Text.Length == 0)
            {
                inStockExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }


        // constructor for add product window
        public ProductWindow()
        {
            InitializeComponent();
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

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

            this.action = action;

            addButton.Visibility = Visibility.Hidden;

            idTextBox.IsEnabled = false;
        }


        // updates the product with the new details in the textboxs
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
                catch (IdAlreadyExist ex)
                {
                    idExceptionLable.Content = " This ID already exist!";
                    idExceptionLable.Visibility = Visibility.Visible;
                }
            }
        }

        // adds a product with the details in the textboxs
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkTextBoxes())
            {
                //insertProductDetails(ref product);
                try
                {
                    bl.Product.AddProduct(newProduct);
                    action(bl.Product.GetProductForList(newProduct.ID));
                    MessageBox.Show("product added!");
                    this.Close();
                }
                catch (IdAlreadyExist ex)
                {
                    idExceptionLable.Content = " The ID you entered already exists!";
                    idExceptionLable.Visibility = Visibility.Visible;
                }
            }

        }

        // make sure the user can enter only numbers as ID
        private void IDPrev(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // make sure the user can enter only letters as name
        private void NamePrev(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // make sure the user can enter only numbers as price
        private void PricePrev(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // make sure the user can enter only numbers as amount in stock
        private void InStockPrev(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // make sure the user entered 6 numbers for ID
        private void idTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Length < 6)
            {
                idExceptionLable.Content = "Enter 6 digits!";
                idExceptionLable.Visibility = Visibility.Visible;
            }
            else
            {
                idExceptionLable.Visibility = Visibility.Hidden;
            }

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            }
        }

        private void idTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

    }
}



