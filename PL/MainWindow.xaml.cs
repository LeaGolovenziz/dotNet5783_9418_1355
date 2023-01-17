using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        bool isManeger;

        /// <summary>
        /// the first window - the user chooses if he is a customer or a worker
        /// </summary>
        public MainWindow()//user type- worker or customer
        {
            InitializeComponent();

            GridUser.Visibility = Visibility.Visible;
            GridPassword.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// the password that the customer goes in as a worker or a costomer
        /// </summary>
        /// <param name="bl"></param>
        public MainWindow(BlApi.IBl bl)//password window
        {
            InitializeComponent();
            GridUser.Visibility = Visibility.Hidden;
            GridPassword.Visibility = Visibility.Visible;
            GridLogCustomer.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// whne pressed enter after the user eneters the password - check of the password is valid->customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCustomerEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = bl.User.Get().ToList().Find(x => x.Password == passwordBox.Password && x.Name == NameBox.Text && x.IsManeger == false);
                if (user != null) //varifyig the customer is a customer and not a worker 
                {
                    new BuyerWindow(user).ShowDialog();
                    CustomerMode();
                }
                else
                {
                    MessageBox.Show("Incorrect user name or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    passwordBox.Clear();
                }
            }
            catch (DoesntExist)
            {

                MessageBox.Show("Incorrect user name or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordBox.Clear();
            }
        }

        /// <summary>
        /// clears data of the password and name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_clear(object sender, RoutedEventArgs e)
        {
            NameBox.Clear();
            passwordBox.Clear();
        }

        /// <summary>
        /// the password window for a worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_WorkerMode(object sender, RoutedEventArgs e)
        {
            ManegerMode();
        }

        /// <summary>
        /// the password window for a worker
        /// </summary>
        private void ManegerMode()
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Visible;
            btnEnterCustomer.Visibility = Visibility.Hidden;
            btnWorkerEnter.Visibility = Visibility.Visible;
            isManeger = true;
        }

        /// <summary>
        /// opens the password window for the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_CustomerMode(object sender, RoutedEventArgs e)
        {
            CustomerMode();
        }

        /// <summary>
        /// the password window for a custumer 
        /// </summary>
        private void CustomerMode()
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Visible;
            btnWorkerEnter.Visibility = Visibility.Hidden;
            isManeger = false;
        }

        /// <summary>
        /// bring you to the adding customer window where you can add a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            new CreateUserWindow(isManeger).ShowDialog();
        }

        /// <summary>
        /// brings you to the password window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_LogIn(object sender, RoutedEventArgs e)
        {
            GridPassword.Visibility = Visibility.Visible;
            GridLogCustomer.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// go back to starting view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInBack_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Clear();
            passwordBox.Clear();
            GridUser.Visibility = Visibility.Visible;
            GridPassword.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// checking the password after entered for worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMenagerEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var maneger = bl.User.Get().ToList().Find(x => x.Password == passwordBox.Password && x.Name == NameBox.Text && x.IsManeger==true);
                if (maneger != null)
                {
                    new ManagerWindow().ShowDialog();
                    ManegerMode();
                }
                else
                    MessageBox.Show("Incorrect menager name or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (DoesntExist)
            {
                MessageBox.Show("Incorrect menager name or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///  reset password mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForgetPasword_Click(object sender, RoutedEventArgs e)
        {
            GridPassword.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// the ability to update the password if you forgot it 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = bl.User.Get(Convert.ToInt32(txbEnterId.Text));
                if (user.Name != txbEnterName.Text)
                {
                    MessageBox.Show("The name does not matched to the customer ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bl.User.ResetPassword(Convert.ToInt32(txbEnterId.Text), txbEnterYourNewP.Text);
                    MessageBox.Show("Succsesfully reset password!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DoesntExist)
            {
                MessageBox.Show("Incorrect user name or ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// close reset password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButtom_Click(object sender, RoutedEventArgs e)
        {
            resetPasword.Visibility = Visibility.Hidden;
            GridPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// makes sure the user can enter only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
