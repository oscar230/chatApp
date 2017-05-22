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

        public Dictionary<int, string> userList { get; private set; } //A list of all users using their username.

        //Constructor
        public User()
        {
            dbh.Open();
            string query = @"SELECT id, username FROM user";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            dbh.Close();

            userList = new Dictionary<int, string>();

            foreach (DataRow user in data.Rows)
            {
                Debug.WriteLine("Added " + user["id"].ToString() + " - " + user["username"].ToString() + " to the userList.");
                userList.Add(Convert.ToInt32(user["id"].ToString()) ,user["username"].ToString());
            }
            
        }

        public Dictionary<int, string> GetFriends()
        {
            Dictionary<int, string> friends = new Dictionary<int, string>();

            dbh.Open();

            string query = @"SELECT id2 FROM friend WHERE id1 = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id", Convert.ToString(account.ToString()));
            MySqlDataReader friendGet;
            friendGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(friendGet);

            foreach (DataRow row in data.Rows)
            {
                friends.Add(Convert.ToInt32(row["id2"].ToString()), GetUsername(Convert.ToInt32(row["id2"].ToString())));
            }

            dbh.Close();

            if (data != null)
            {
                return null;
            }
            return friends;
        }

        //Returns the id of the user that the client is looking for.
        public int GetId(string username)
        {
            dbh.Open();
            string query = @"SELECT id, username FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            dbh.Close();

            int id = 0;

            foreach (DataRow row in data.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    username = row["id"].ToString();
                    return id;
                }
            }
            return id;
        }
        public string GetUsername(int id)
        {
            dbh.Open();
            string query = @"SELECT id, username FROM user WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            dbh.Close();

            string username = null;

            foreach (DataRow row in data.Rows)
            {
                if (Convert.ToInt32(row["id"].ToString()) == id)
                {
                    username = row["id"].ToString();
                    return username;
                }
            }
            return username;
        }

        //Returns the state of the user (true online, false offline).
        public bool GetOnlineState(int id)
        {
            dbh.Open();
            string query = @"SELECT online FROM user WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@username", id.ToString());
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            dbh.Close();

            foreach (DataRow row in data.Rows)
            {
                if (row["online"].ToString() == "True")
                {
                    return true;
                }
            }
            return false;
        }
    }
}