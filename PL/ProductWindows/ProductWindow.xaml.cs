using BlApi;
using Bllmplementation;
using BO;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private IBl bl = new Bl();

        // get a product and initialize it's details from the textboxs
        void insertProductDetails(ref Product product)
        {
            product.ID = int.Parse(idTextBox.Text);
            product.Name = idTextBox.Text;
            product.Category = (Enums.Category)categoryComboBox.SelectedItem;
            product.Price = double.Parse(priceTextBox.Text);
            product.InStock = int.Parse(inStockTextBox.Text);
        }

        // make all the exception hidden
        void blankexceptionLables()
        {
            idExceptionLable.Visibility = Visibility.Hidden;
            isCategoryLable.Visibility = Visibility.Hidden;
            nameExceptionLable.Visibility = Visibility.Hidden;
            priceExceptionLable.Visibility = Visibility.Hidden;
            inStockExceptionLable.Visibility = Visibility.Hidden;
        }

        // clear all texboxs' content
        void clearTextBoxs()
        {
            idTextBox.Text = string.Empty;  
            nameTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            inStockTextBox.Text = string.Empty; 
            categoryComboBox.Text = string.Empty;
        }

        // checks if ther'e are details in the textBoxes
        bool checkTextBoxes()
        {
            if (idTextBox.Text.Length != 6)
            {
                idExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if (categoryComboBox.SelectedValue == null)
            {
                isCategoryLable.Visibility = Visibility.Visible;
                return false;
            }
            if(nameTextBox.Text.Length == 0)
            {
                nameExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if(priceTextBox.Text.Length==0)
            {
                priceExceptionLable.Visibility = Visibility.Visible;
                return false;
            }
            if(inStockTextBox.Text.Length==0)
            {
                inStockExceptionLable.Visibility= Visibility.Visible;
                return false;
            }
            return true;
        }

        // constructor for add product window
        public ProductWindow()
        {
            InitializeComponent();

            blankexceptionLables();

            updateButton.Visibility = Visibility.Hidden;

            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        // constructor for update product window
        public ProductWindow(int id)
        {
            InitializeComponent();

            blankexceptionLables();

            addButton.Visibility = Visibility.Hidden;

            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
            idTextBox.IsEnabled = false;

            Product product = bl.Product.GetProductDetails(id);
            idTextBox.Text = id.ToString();
            categoryComboBox.SelectedItem = product.Category;
            nameTextBox.Text = product.Name;
            priceTextBox.Text = product.Price.ToString();
            inStockTextBox.Text = product.InStock.ToString();

        }

        // updates the product with the new details in the textboxs
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // reset visability of error lables
            blankexceptionLables();

            if (checkTextBoxes())
            {
                Product product = new Product();
                insertProductDetails(ref product);
                try
                {
                    bl.Product.UpdateProduct(product);
                    MessageBox.Show("product updated!");
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex is UnvalidID)
                    {
                        idExceptionLable.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        // adds a product with the details in the textboxs
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // reset visability of error lables

            if (checkTextBoxes())
            {
                Product product = new Product();
                insertProductDetails(ref product);
                try
                {
                    bl.Product.AddProduct(product);
                    MessageBox.Show("product added!");
                    clearTextBoxs();
                }
                catch (Exception ex)
                {
                    if (ex is UnvalidID)
                    {
                        idExceptionLable.Visibility = Visibility.Visible;
                    }
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
            Regex regex = new Regex("^[a-zA-Z]+");
            e.Handled = !regex.IsMatch(e.Text);
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
    }
}
