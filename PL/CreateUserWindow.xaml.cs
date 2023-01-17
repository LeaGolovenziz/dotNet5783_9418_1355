using BO;
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
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        User user=new User();
        public CreateUserWindow(bool isManeger)
        {
            InitializeComponent();

            general.DataContext = user;
            user.IsManeger = isManeger;
        }

        // makes sure the user can enter only numbers
        private void allowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                bl.User.AddUser(user);
            }
            catch (UnvalidID)
            {
                MessageBox.Show("Unvalid ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IdAlreadyExist)
            {
                MessageBox.Show("There is already a user with this ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
}
