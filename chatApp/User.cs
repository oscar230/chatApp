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
            string query = @"SELECT id, username FROM user";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            
            userList = new List<string>();

            foreach (DataRow user in data.Rows)
            {
                Debug.WriteLine("Added " + user["id"].ToString() + " - " + user["username"].ToString() + " to the userList.");
                userList.Add(user["username"].ToString());
            }
            dbh.Close();

        }
        //Gets the list of all friends for a specifik user (the user who is logged in)
        public List<string> GetFriends()
        {
            List<string> friends = new List<string>();

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
                friends.Add(GetUsername(Convert.ToInt32(row["id2"].ToString())));
            }

            dbh.Close();

            if (data != null)
            {
                return null;
            }
            return friends;
        }
        //Adds users ids to the friends list table
        public void AddFreind(int id)
        {
            dbh.Open();
            string query = "INSERT INTO friend (id1, id2) VALUES (@id1, @id2)";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", id);
            cmd.ExecuteNonQuery();
            dbh.Close();

            Debug.WriteLine("Added: (" + account.id + ", " + id + ").");
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

            int id = 0;

            foreach (DataRow row in data.Rows)
            {
                if (row["username"].ToString() == username)
                {
                    username = row["id"].ToString();
                    dbh.Close();
                    return id;
                }
            }
            dbh.Close();
            return id;
        }
        public string GetUsername(int id)
        {
            dbh.Close();
            dbh.Open();
            string query = @"SELECT id, username FROM user WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            
            string username = null;

            foreach (DataRow row in data.Rows)
            {
                if (Convert.ToInt32(row["id"].ToString()) == id)
                {
                    username = row["id"].ToString();
                    return username;
                }
            }
            dbh.Close();
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