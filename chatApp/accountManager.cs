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

        //Current account information
        string username;
        int id;
        bool active = false;

        //Constructor
        public AccountManager(MySqlConnection dbh)
        {
            this.dbh = dbh; //Declares the dabase handeler of this class.

            this.username = null;
            this.id = 0; //No user can have an user id of 0 and therefore 0 is seen as null.
        }

        //Overrides the toString() method, this is now used to check whetever the user is authenticated or not.
        public override string ToString()
        {
            return (this.active.ToString());
        }

        //Authenticates the user and commits a login if succesfull.
        public bool Login(string inputUsername, string inputPassword)
        {
            //If the user exist in the database
            if (usernameCheck(inputUsername))
            {
                setId(inputUsername);
                this.username = inputUsername;
                Debug.WriteLine("username is valid : " + this.username + " : id : " + this.id);
                if (passwordCheck(inputPassword))
                {
                    Debug.WriteLine("Password is valid");
                    this.active = true;
                }
            }
            else
            {
                Debug.WriteLine("username is not valid");
                this.username = null;
                this.id = 0;
                return false;
            }
            return false;
        }

        //Creates an user account.
        public bool Reg(string inputUsername, string inputPassword)
        {
            return false;
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

        //Checks if the user exists and assigns this.id with an integer. This is essential for loging in.
        private bool usernameCheck(string username)
        {
            string query = "SELECT username FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            dbh.Open();
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            Debug.WriteLine("usernameCheck username : " + rdr[0].ToString());
            if (username == rdr[0].ToString())
            {
                dbh.Close();
                return true;
            }
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
