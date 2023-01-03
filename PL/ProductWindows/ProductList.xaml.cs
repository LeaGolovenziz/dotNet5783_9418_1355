using BO;
using Dal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public ObservableCollection<ProductForList?> Products;

        private Action<Order, int> action;
        public Order order = new Order();

        void resetProducts()
        {
            Products = new ObservableCollection<ProductForList?>(from item in bl.Product.GetProductsList()
                                                                 orderby item?.Name
                                                                 select item);
            this.DataContext = Products;
        }

        // Constructor of editing the products by the manager
        public ProductList()
        {
            InitializeComponent();

            resetProducts();

            AddOrderItem.Visibility = Visibility.Hidden;

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
        }

        // Constructor of chosing a product to add to an order
        public ProductList(Action<Order, int> action, int id) : this()
        {
            this.action = action;

            AddNewProduct.Visibility = Visibility.Hidden;
            AddOrderItem.Visibility = Visibility.Visible;

            try
            {
                order = bl.Order.GetOrderDetails(id);
            }
            catch(DoesntExist ex)
            {
                MessageBox.Show("Can't find the order", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sorts the products by the chosen category
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem != null)
            {
                Products = new ObservableCollection<ProductForList?>(bl.Product.GetProductsList().Where(product => product?.Category == (BO.Enums.Category)CategorySelector.SelectedItem));
            }
            this.DataContext = Products;
        }

        // Sows all the products with no filtering
        private void clearCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            resetProducts();
            CategorySelector.SelectedItem = null;
        }

        // Delegate that adds product to the product's list
        private void addProduct(ProductForList productForList)
        => Products.Add(productForList);

        // Adding a product
        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            // Sending a delegate to the window that adding a product
            new ProductWindow(addProduct).ShowDialog();
        }

        // Delegate that updates product to the list
        private void updateProduct(ProductForList productForList)
        {
            var item = Products.FirstOrDefault(item => item?.ID == productForList.ID);
            if (item != null)
                Products[Products.IndexOf(item)] = productForList;
        }

        // Updating a product
        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Only if the window opened for editing and not just adding order item to an order - allow updating 
            if (AddNewProduct.Visibility == Visibility.Visible)
            {
                ProductForList product = (ProductForList)ProductListView.SelectedItem;
                if (product != null)
                {
                    new ProductWindow(updateProduct, product.ID).ShowDialog();
                    CategorySelector.SelectedItem = null;
                }
            }
        }

        // Adding product to an order
        private void AddOrderItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int productID = ((ProductForList)ProductListView.SelectedItem).ID;

                order = bl.Order.AddNewOrderItem(order.ID, productID);

                action(order, productID);

                MessageBox.Show("product added to the order!");
            }
            catch (ProductNotInStock ex)
            {
                MessageBox.Show("The product is out of stock :(", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(DoesntExist ex)
            {
                MessageBox.Show("Can't find the product", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // To prevent failure
        }
    }
}
