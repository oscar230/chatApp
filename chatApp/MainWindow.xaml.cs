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
using System.Data.SqlClient;

namespace chatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection myConnection = new SqlConnection("user id=root;" +
                                       "password=;" +
                                        "server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=database; " +
                                       "connection timeout=30");
        public MainWindow()
        {
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
    }
}
