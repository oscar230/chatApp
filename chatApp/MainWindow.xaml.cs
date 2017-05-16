﻿using System;
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
                logout.Visibility = Visibility.Hidden;
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
            
            //Logs the user in.
            if (this.account.Login(loginUsername.Text, loginPassword.Text))
            {
                Debug.WriteLine("Funkar!");
                //Login sucessfull
                login.Visibility = Visibility.Hidden;
                logout.Visibility = Visibility.Visible;
                //logoutLabel.Content = loginUsername;
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
                this.account.Login(loginUsername.Text.ToString(), loginPassword.Text.ToString());
                login.Visibility = Visibility.Hidden;
                logout.Visibility = Visibility.Visible;
                logoutLabel.Content = loginUsername;
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
            account.Logout();
        }
    }
}
