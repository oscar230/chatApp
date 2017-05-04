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
        MySqlConnection dbh = new MySqlConnection(@"
                    server=localhost;
                    userid=root;
                    password=mm54rsa;
                    database=chatapp;
            ");

        public MainWindow()
        {
            //Builds form
            InitializeComponent();
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

        private void Button_Click_3(object sender, RoutedEventArgs e) //Connect to server
        {
            //Try to open a connection with the server
            try
            {
                dbh.Open(); //Opens a cennection with the MySQL server
                connectionStatus.Content = "Connected"; //Shows a connected messege.
                connectionButton.Visibility = Visibility.Hidden; //Removes the button.
                login.Visibility = Visibility.Visible; //Shows the login canvas.
            }
            catch (MySqlException exeption)
            {
                //If the connection fails, an error messege is shown and the connection button is visible again.
                connectionStatus.Content = exeption.ToString();
                connectionButton.Visibility = Visibility.Visible;
                Debug.WriteLine(exeption.ToString()); //Debug
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) //Login
        {
            try
            {
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
