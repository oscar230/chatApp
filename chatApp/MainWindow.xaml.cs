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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient; //MySql
using System.Diagnostics; //Debug

namespace chatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccountManager account = new AccountManager();
        User user;

        //Builds and initializes the form.
        public MainWindow()
        {
            //Builds form
            InitializeComponent();

            motd_group.Visibility = Visibility.Visible; //MOTD contains login and logout
            login.Visibility = Visibility.Visible; //Shows the login canvas.
            logout.Visibility = Visibility.Hidden;//Hides the logout canvas.

            canvas_group.Visibility = Visibility.Hidden;
            canvas_chat.Visibility = Visibility.Hidden;
            canvas_userlist.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Group
        {
            if (canvas_group.Visibility == Visibility.Visible)
            {
                canvas_group.Visibility = Visibility.Hidden;
                motd_group.Visibility = Visibility.Visible;
            }
            else
            {
                canvas_group.Visibility = Visibility.Visible;
                canvas_chat.Visibility = Visibility.Hidden;
                canvas_userlist.Visibility = Visibility.Hidden;
                motd_group.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Chat
        {
            if (canvas_chat.Visibility == Visibility.Visible)
            {
                canvas_chat.Visibility = Visibility.Hidden;
                motd_group.Visibility = Visibility.Visible;
            }
            else
            {
                canvas_group.Visibility = Visibility.Hidden;
                canvas_chat.Visibility = Visibility.Visible;
                canvas_userlist.Visibility = Visibility.Hidden;
                motd_group.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //Userlist
        {
            if (canvas_userlist.Visibility == Visibility.Visible)
            {
                canvas_userlist.Visibility = Visibility.Hidden;
                motd_group.Visibility = Visibility.Visible;
            }
            else
            {
                canvas_group.Visibility = Visibility.Hidden;
                canvas_chat.Visibility = Visibility.Hidden;
                canvas_userlist.Visibility = Visibility.Visible;
                motd_group.Visibility = Visibility.Hidden;

                if (user == null && account.ToString() == "True")
                {
                    user = new User();
                    Debug.WriteLine("Created user object.");
                    while (user.userList == null)
                    {
                        Debug.WriteLine("Waiting for database.");
                    }
                    //Get the full userlist.
                    userListBox.ItemsSource = user.userList;
                    //If the logged in user has any friends they will be handled here.
                    if (user.GetFriends() != null)
                    {
                        friendListBox.ItemsSource = user.GetFriends().Values;
                    }
                }
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) //Login
        {
            
            //Logs the user in.
            if (this.account.Login(loginUsername.Text, loginPassword.Text))
            {
                Debug.WriteLine("Funkar! " + account.ToString());
                //Login sucessfull
                login.Visibility = Visibility.Hidden;
                logout.Visibility = Visibility.Visible;
                logoutLabel.Content = loginUsername.Text;
            }
            else
            {
                //Login failed. Reset login fields.
                loginUsername = null;
                loginPassword = null;
            }
        }

        private void regButton_Click(object sender, RoutedEventArgs e) //Register
        {
            //Registers the user.
            if (this.account.Reg(loginUsername.Text.ToString(), loginPassword.Text.ToString()))
            {
                //Register sucessfull
                loginUsername.Text = null;
                loginPassword.Text = null;
            }
            else
            {
                //Login failed. Reset login fields.
                loginUsername.Text = null;
                loginPassword.Text = null;
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e) //Logout
        {
            if (account.Logout())
            {
                login.Visibility = Visibility.Visible;
                logout.Visibility = Visibility.Hidden;
                logoutLabel.Content = null;
            }
        }

        private void closing(object sender, System.ComponentModel.CancelEventArgs e) //When closing WPF with X
        {
            account.Logout();
        }

        private void userlistAddFriend_Click(object sender, RoutedEventArgs e) //userlistAddFriend
        {
            Debug.WriteLine("selected user in userlist: " + user.GetId(userListBox.SelectedItem.ToString()) + " - " + userListBox.SelectedItem.ToString());
        }

        private void userlistDeleteFriend_Click(object sender, RoutedEventArgs e) //userlistDeleteFriend
        {

        }
    }
}
