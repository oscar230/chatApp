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
    /// <summary>
    /// Handles the userlist and the interaction between the client user and other users.
    /// </summary>
    class User
    {
        //Objects
        AccountManager account; //Initates a empty account obejct.
        MySqlConnection dbh = AccountManager.dbh; //Initiates the database handler object.

        public List<string> userList { get; private set; } //A list of all users using their username.

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="account">The account manager object is needed to represent the client user.</param>
        public User(AccountManager account)
        {
            this.account = account;
            //Gets a userlist from the database.
            dbh.Open();
            string query = @"SELECT id, username FROM user"; //TODO dont get the current user.
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);
            
            userList = new List<string>();

            //Fills the userList List with the usernames from the database.
            foreach (DataRow user in data.Rows)
            {
                Debug.WriteLine("Added " + user["id"].ToString() + " - " + user["username"].ToString() + " to the userList.");
                userList.Add(user["username"].ToString());
            }
            dbh.Close();
        }

        /// <summary>
        /// Gets the list of all friends for a specifik user (the user who is logged in)
        /// </summary>
        /// <returns>Returns a list of strings with usernames of all friends.</returns>
        public List<string> GetFriends()
        {
            Debug.WriteLine("Active user id: " + Convert.ToString(account.id));
            List<string> friends = new List<string>();

            dbh.Open();

            string query = @"SELECT id2 FROM friend WHERE id1 = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id", Convert.ToString(account.id));
            MySqlDataReader friendGet;
            friendGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(friendGet);

            dbh.Close();

            foreach (DataRow row in data.Rows)
            {
                Debug.WriteLine(GetUsername(Convert.ToInt32(row["id2"].ToString())));
                friends.Add(GetUsername(Convert.ToInt32(row["id2"].ToString())));
            }
            
            return friends;
        }

        /// <summary>
        /// Adds a users ids to the friends list table.
        /// </summary>
        /// <param name="id">The user id of the friends to add.</param>
        public void AddFreind(int id)
        {
            List<string> friends = GetFriends();
            string username = GetUsername(id);
            foreach (string friend in friends)
            {
                if (username == friend)
                {
                    //Exits the method.
                    return;
                }
            }
            dbh.Open();
            string query = "INSERT INTO friend (id1, id2) VALUES (@id1, @id2)";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", id);
            cmd.ExecuteNonQuery();
            dbh.Close();

            Debug.WriteLine("Initiate AddFriend()\n Added: (" + account.id + ", " + id + ").");
        }

        /// <summary>
        /// Deletes a users ids from the friends list table
        /// </summary>
        /// <param name="id">The user id to delete from the friend list.</param>
        public void DeleteFreind(int id)
        {
            dbh.Open();
            string query = "DELETE FROM friend WHERE id1 = @id1 AND id2 = @id2";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", id);
            cmd.ExecuteNonQuery();
            dbh.Close();

            Debug.WriteLine("Deleted (" + account.id + ", " + id + ") from the freindlist table. (" + GetUsername(id) + ")");
        }

        /// <summary>
        /// Returns the id of the user that the client is looking for.
        /// Searches the database for the id of the username that matches the input.
        /// </summary>
        /// <param name="username">The input string.</param>
        /// <returns>int user id</returns>
        public int GetId(string username)
        {
            dbh.Close();
            dbh.Open();
            string query = @"SELECT id FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();

            DataTable data = new DataTable();
            data.Load(usernameGet);

            int id = 0;

            foreach (DataRow row in data.Rows)
            {
                Debug.WriteLine("In: " + username + ". Id: " + row["id"].ToString());
                id = Convert.ToInt32(row["id"].ToString());
                dbh.Close();
                return id;
            }
            dbh.Close();
            return id;
        }

        /// <summary>
        /// Returns the username of the user that the client is looking for.
        /// Searches the database for the username of the user id that matches the input.
        /// </summary>
        /// <param name="id">The input user id</param>
        /// <returns>string username</returns>
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
                    username = row["username"].ToString();
                    dbh.Close();
                    return username;
                }
            }
            dbh.Close();
            return username;
        }

        /// <summary>
        /// Returns the state of the user (true online, false offline).
        /// This class isnt curently in use, but is later to be used in the GUI to display which users are avaliable.
        /// </summary>
        /// <param name="id">The user id of the user of which state is requested.</param>
        /// <returns>Boolean. True = online, False = offline.</returns>
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