using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; //MySql
using System.Diagnostics; //Debug

namespace chatApp
{
    class AccountManager
    {
        //Creates an empty database handler object for the mysql connection.
        MySqlConnection dbh = new MySqlConnection();

        //Constructor
        public AccountManager(MySqlConnection dbh)
        {
            this.dbh = dbh;
            dbh.Open();
        }

        //Authenticates the user and commits a login if succesfull.
        public bool Login(string inputUsername, string inputPassword)
        {
            return false;
        }

        //Creates an user account.
        public bool Reg(string inputUsername, string inputPassword)
        {
            return false;
        }

        //Checks if the user exists
        private bool usernameCheck(string username)
        {
            MySqlCommand cmd = dbh.CreateCommand();
            cmd.CommandText = "SELECT username FROM user WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", username);
            return true;
        }
    }
}
