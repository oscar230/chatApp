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
    class Chat
    {
        //Objects
        MySqlConnection dbh = AccountManager.dbh;
        AccountManager account;

        //Variables
        int id = 0; //The id of the user who the chat instance is initiated with.

        //Constructor
        public Chat(AccountManager account, int id)
        {
            this.account = account;
            this.id = id;
        }

        public List<string> GetChat() //Retreives the chat history from the database
        {
            dbh.Open();
            string query = @"SELECT value FROM chat WHERE (id1 = @id1 AND id2 = @id2) OR (id1 = @id2 AND id2 = @id1) ORDER BY datetime ASC";
            MySqlCommand cmd = new MySqlCommand(query, dbh);
            cmd.Parameters.AddWithValue("@id1", account.id);
            cmd.Parameters.AddWithValue("@id2", this.id);
            MySqlDataReader chat;
            chat = cmd.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(chat);
            dbh.Close();

            List<string> chatHistory = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                Debug.WriteLine("chatHistory: " + row["value"].ToString());
                chatHistory.Add(row["value"].ToString());
            }
            return chatHistory;
        }

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
