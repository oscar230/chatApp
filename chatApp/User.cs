using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //Data management
using MySql.Data.MySqlClient; //MySql
using System.Diagnostics; //Debug

namespace chatApp
{
    class User
    {
        AccountManager account = new AccountManager(); //Get logged in user details.
        MySqlConnection dbh = AccountManager.dbh; //Dabase handler.

        public List<string> userList { get; private set; } //A list of all users using their username.

        //Constructor
        public User()
        {
            dbh.Open();
            string query = @"SELECT username FROM user";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            dbh.Close();

            userList = new List<string>();

            foreach (DataRow user in data.Rows)
            {
                Debug.WriteLine("Added " + user["username"].ToString() + " to the userList.");
                userList.Add(user["username"].ToString());
            }
            
        }
        //Returns the id of the user that the client is looking for.
        private int GetId(string username)
        {
            dbh.Open();
            string query = @"SELECT id, username FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);

            int id = 0;

            foreach (DataRow row in data.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    id = Convert.ToInt32(row["id"].ToString());
                    return id;
                }
            }
            return id;
        }
    }
}