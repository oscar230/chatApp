using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data; //Data management
using MySql.Data.MySqlClient; //MySql
using System.Diagnostics; //Debug

namespace chatApp
{
    /// <summary>
    /// This class mangaes the account that i currently in use.
    /// When the user in logged in an object of this class is created and the constructor assigns the declared variables in this class.
    /// These variables are then used to identify the logged in user.
    /// Technically objects from this class can be created an infite number of times, meaning that multiple users can be active at once. Altough this program is not design for multiple user interactions.
    /// </summary>
    class AccountManager
    {
        /// <summary>
        /// Creates connection with MySql server.
        /// This static function is used by other classes.
        /// </summary>
        public static MySqlConnection dbh = new MySqlConnection(@"
                    server=oscarandersson.tk;
                    userid=oscarander_app;
                    password=aB3vtJyQbHWPvY6g;
                    database=oscarander_chatapp;
            ");

        //Current account information
        public string username { get; private set; }
        public int id { get; private set; }
        bool active = false;

        //Constructor
        public AccountManager()
        {
            this.username = null;
            this.id = 0; //No user can have an user id of 0 and therefore 0 is seen as null.
        }

        //Overrides the toString() method, this is now used to check whetever the user is authenticated or not.
        public override string ToString()
        {
            return (this.active.ToString());
        }

        /// <summary>
        /// Authenticates the user and commits a login if succesfull.
        /// </summary>
        /// <param name="inputUsername">The username that is inputed by ther user in the GUI.</param>
        /// <param name="inputPassword">The password -||-.</param>
        /// <returns>Returns true if the user is sucessfully authenticated. This is only used for the GUI to clear its login input fields.</returns>
        public bool Login(string inputUsername, string inputPassword)
        {
            //If the user exist in the database
            if (usernameCheck(inputUsername))
            {
                //Calls the setId function
                setId(inputUsername);
                Debug.WriteLine("Id set");
                this.username = inputUsername;
                Debug.WriteLine("username is valid : " + this.username + " : id : " + this.id);
                //Valid password check
                if (passwordCheck(inputPassword))
                {
                    Debug.WriteLine("Password is valid");
                    this.active = true;
                    Debug.WriteLine("active");
                    setOnline(true);
                    Debug.WriteLine("online \n Id: " + this.id);
                    return true;
                }
                else
                {
                    Debug.WriteLine("password is not valid");
                    this.username = null;
                    this.id = 0;
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("username is not valid");
                this.username = null;
                this.id = 0;
                return false;
            }
        }

        /// <summary>
        /// Strips the class of its variables wich renders the user as inactive and logged out. The class (object) can now be overwritten, just left alone or simply destroyed.
        /// TODO: Create a destructor that haldes this and deletes the object.
        /// </summary>
        /// <returns>If the logout was sucessfull true is returned</returns>
        public bool Logout()
        {
            setOnline(false);
            this.username = null;
            this.id = 0;
            this.active = false;
            if (this.active == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates an user account.
        /// Sends the new user information to the databse once the username check is complete.
        /// </summary>
        /// <param name="inputUsername"></param>
        /// <param name="inputPassword"></param>
        /// <returns></returns>
        public bool Reg(string inputUsername, string inputPassword)
        {
            //The username needs to be checked. If the username already exists then the user account wont be added to the databse.
            if (usernameCheck(inputUsername) == true)
            {
                Debug.WriteLine("Username already exists in database.");
                return false;
            }
            else
            {
                //Hashes the password. The hashsum is then added to the database. The cleartext password is from now on nowhere to be seen.
                PasswordHash hasher = new PasswordHash();
                string hash = hasher.HashItteration(inputPassword);

                string query = "INSERT INTO user (username, hash) VALUES (@username, @hash)";
                MySqlCommand cmd = new MySqlCommand(query, dbh);
                dbh.Open();
                cmd.Parameters.AddWithValue("@username", inputUsername);
                cmd.Parameters.AddWithValue("@hash", hash);
                cmd.ExecuteNonQuery();
                dbh.Close();
                Debug.WriteLine("User added to the database.");
                return true;
            }      
        }

        //Gets and sets the user id for a specific user and sets the class variable this.id.
        private void setId(string username)
        {
            string query = "SELECT id FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            dbh.Open();
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            Debug.WriteLine("setId id : " + rdr[0].ToString());
            //Assigns the object (login session) with the user's user id.
            this.id = Convert.ToInt32(rdr[0].ToString());
            //Closes the connection
            dbh.Close();
        }

        //Updates the online status the logged in user.
        private void setOnline(bool online)
        {
            string query = "UPDATE user SET online = @online WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            dbh.Open();
            cmd.Parameters.AddWithValue("@id", this.id);
            if (online)
            {
                cmd.Parameters.AddWithValue("@online", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@online", 0);
            }
            cmd.ExecuteNonQuery();
            dbh.Close();
        }

        //Checks if the user exists and assigns this.id with an integer. This is essential for loging in.
        private bool usernameCheck(string username)
        {
            dbh.Open();
            string query = @"SELECT username FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader usernameGet;
            usernameGet = cmd.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(usernameGet);
            
            //Looks for the username
            try
            {
                if (data != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        if (username == row["username"].ToString())
                        {
                            dbh.Close();
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            { }
            dbh.Close();
            return false;
        }

        //Checks if the password hash exists
        private bool passwordCheck(string password)
        {
            //If the user id is valid.
            if (this.id == 0)
            {
                return false;
            }
            //Starts database lookup
            string query = "SELECT hash FROM user WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            dbh.Open();
            cmd.Parameters.AddWithValue("@id", this.id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            //Hashes the plaintext password
            PasswordHash hasher = new PasswordHash();
            string hash = hasher.HashItteration(password);
            //Check
            Debug.WriteLine("client : " + hash.ToString() + "\nserver : " + rdr[0].ToString());
            if (hash == rdr[0].ToString())
            {
                Debug.WriteLine("hashsum compare status : sucess");
                dbh.Close();
                return true;
            }
            else
            {
                Debug.WriteLine("hashsum compare status : fail");
                dbh.Close();
                return false;
            }

        }
    }
}
