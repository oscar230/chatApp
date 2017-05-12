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
        //Creates connection with MySql server
        private MySqlConnection dbh = new MySqlConnection(@"
                    server=oscarandersson.tk;
                    userid=oscarander_app;
                    password=aB3vtJyQbHWPvY6g;
                    database=oscarander_chatapp;
            ");

        //Builds and initializes the form.
        public MainWindow()
        {
            //Builds form
            InitializeComponent();

            //Try to open a connection with the server
            try
            {
                //dbh.Open(); //Opens a cennection with the MySQL server
                login.Visibility = Visibility.Visible; //Shows the login canvas.
                Debug.WriteLine("MySQL Connected");
            }
            catch (MySqlException exeption)
            {
                //If the connection fails, an error messege is shown and the connection button is visible again.
                Debug.WriteLine("MySQL Disconnected");
                Debug.WriteLine(exeption.ToString()); //Debug
            }
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
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) //Login
        {
            //Creates the account manager object
            AccountManager account = new AccountManager(dbh);

            //Logs the user in.
            if (account.Login(loginUsername.Text.ToString(), loginPassword.Text.ToString()) == true)
            {
                //Login sucessfull
                login.Visibility = Visibility.Hidden;
            }
            else
            {
                //Login failed. Reset login fields.
                loginUsername = null;
                loginPassword = null;
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e) //Register
        {
            //Creates the account manager object
            AccountManager account = new AccountManager(dbh);

            //Logs the user in.
            if (account.Login(loginUsername.Text.ToString(), loginPassword.Text.ToString()) == true)
            {
                //Login sucessfull
                login.Visibility = Visibility.Hidden;
            }
            else
            {
                //Login failed. Reset login fields.
                loginUsername = null;
                loginPassword = null;
            }
        }
    }
}
