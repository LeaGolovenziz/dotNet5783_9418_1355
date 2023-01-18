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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txbID.Text.==0|| txbName.Text.Length == 0 || txbPassword.Text.Length == 0)
                MessageBox.Show("Fill in all the details!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    bl.User.AddUser(user);
                    MessageBox.Show("User has added", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (UnvalidID)
                {
                    MessageBox.Show("Unvalid ID, you should enter 9 numbers!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IdAlreadyExist)
                {
                    MessageBox.Show("There is already a user with this ID!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    txbID.Clear();
                }
                catch (FileSavingError)
                {
                    MessageBox.Show("We are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileLoadingError)
                {
                    MessageBox.Show("We are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (XmlFormatError)
                {
                    MessageBox.Show("We are sorry, there was a system error. try again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
