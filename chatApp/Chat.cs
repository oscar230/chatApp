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
    /// The Chat class contains the chat system foundation.
    /// The Chat class is used to get messages from the database and to send messages to the database.
    /// </summary>
    class Chat
    {
        //Objects
        MySqlConnection dbh = AccountManager.dbh;
        AccountManager account;
        User user;

        //Variables
        int id = 0; //The id of the user who the chat instance is initiated with.

        /// <summary>
        /// Constructor
        /// An object is created from this class when a chat is initiated between the client and a remote user.
        /// </summary>
        /// <param name="account">The object containing information about the current client user.</param>
        /// <param name="user">The object contains necesary methods.</param>
        /// <param name="id">This is the user id of the remote user who the chat is to be initiated with.</param>
        public Chat(AccountManager account, User user, int id)
        {
            this.account = account;
            this.user = user;
            this.id = id;
        }

        /// <summary>
        /// Retreives the chat history from the database
        /// </summary>
        /// <returns>A list containing the whole chat history between the two chat partisipants.</returns>
        public List<string> GetChat()
        {
            dbh.Open();
            string query = @"SELECT value, id1 FROM chat WHERE (id1 = @id1 AND id2 = @id2) OR (id1 = @id2 AND id2 = @id1) ORDER BY datetime ASC";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", this.id);
            MySqlDataReader chat;
            chat = cmd.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(chat);
            dbh.Close();

            //Itterates trough the chat history and if the client user id is the same as the messages user id the message is treated like a message coming from the client user.
            List<string> chatHistory = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                Debug.WriteLine("chatHistory: " + row["value"].ToString());

                //Displays the username before the message.
                if (row["id1"].ToString() == Convert.ToString(account.id))
                {
                    chatHistory.Add(user.GetUsername(account.id) + " : " + row["value"].ToString());
                }
                else
                {
                    chatHistory.Add(user.GetUsername(this.id) + " : " + row["value"].ToString());
                }
            }
            return chatHistory;
        }

        /// <summary>
        /// Sends messages from the client to the database.
        /// The sent packet contains the message string, the tranceiver id and the reciever id.
        /// </summary>
        /// <param name="input">The message string.</param>
        public void Send(string input)
        {
            dbh.Open();
            string query = @"INSERT INTO chat (id1, id2, value, datetime) VALUES (@id1, @id2, @value, NOW())";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@value", input);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", this.id);
            cmd.ExecuteNonQuery();
            dbh.Close();
        }
    }
}
