using BlApi;
using Bllmplementation;
using BO;
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

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private IBl bl = new Bl();
        void insertProductDetails(ref Product product)
        {
            product.ID = int.Parse(idTextBox.Text);
            product.Name = idTextBox.Text;
            product.Category = (Enums.Category)categoryComboBox.SelectedItem;
            product.Price = double.Parse(priceTextBox.Text);
            product.InStock=int.Parse(inStockTextBox.Text); 
        }

        void blankexceptionLables()
        {
            idExceptionLable.Visibility = Visibility.Hidden;
            nameExceptionLable.Visibility = Visibility.Hidden;
            priceExceptionLable.Visibility = Visibility.Hidden;
            inStockExceptionLable.Visibility = Visibility.Hidden;
        }

        void catchException(Exception ex)
        {
            if(ex is UnvalidID)
            {
                idExceptionLable.Visibility = Visibility.Visible;
            }
            if (ex is UnvalidName)
            {
                nameExceptionLable.Visibility = Visibility.Visible;
            }
            if (ex is UnvalidPrice)
            {
                priceExceptionLable.Visibility = Visibility.Visible;
            }
            if (ex is UnvalidAmount)
            {
                inStockExceptionLable.Visibility = Visibility.Visible;
            }

        }
        public ProductWindow()
        {
            InitializeComponent();
            updateButton.Visibility=Visibility.Hidden;

            categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }
        public ProductWindow(int id)
        {
            InitializeComponent();

            addButton.Visibility = Visibility.Hidden;

            categoryComboBox.ItemsSource= Enum.GetValues(typeof(BO.Enums.Category));
            idTextBox.IsEnabled = false;

            Product product =bl.Product.GetProductDetails(id);
            idTextBox.Text = id.ToString();
            categoryComboBox.SelectedItem = product.Category;
            nameTextBox.Text= product.Name; 
            priceTextBox.Text=product.Price.ToString(); 
            inStockTextBox.Text=product.InStock.ToString();

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            blankexceptionLables();
            Product product=new Product();
            insertProductDetails(ref product);
            try
            {
                bl.Product.UpdateProduct(product);
                MessageBox.Show("product updated!");
            }
            catch (Exception ex)
            {
                catchException(ex);
            }

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            blankexceptionLables();
            Product product = new Product();
            insertProductDetails(ref product);
            try
            {
                bl.Product.AddProduct(product);
                MessageBox.Show("product added!");
            }
            catch (Exception ex)
            {
                catchException(ex); 
            }

        }

        
    }
}
