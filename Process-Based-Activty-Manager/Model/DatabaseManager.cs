using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Commons;

namespace ActivityTracker
{
    public class DatabaseManager
    {
        public SQLiteConnection connection;

        public DatabaseManager()
        {
            connection = new SQLiteConnection("Data Source=database.sqlite3");
            OpenConnection();

            if (!File.Exists("./database.sqlite3"))
            {
                // da override la baza de date
                SQLiteConnection.CreateFile("database.sqlite3"); 
                System.Console.WriteLine("Database file created");
            }
        }

        /// <summary>
        /// INSERT INTO DATABASE
        /// </summary>
        /// <param name="title"></param>
        public void AddProcess(string title)
        {
            string query = "INSERT INTO user_processes ('title') VALUES(@title)";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@title", title);
            var result = command.ExecuteNonQuery();

            Console.WriteLine("Rows added: {0}", result);
        }


        public List<UserProcess> GetProcesses()
        {
            List<UserProcess> userProcesses = new List<UserProcess>();

            string query = "SELECT * FROM user_processes";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    { 
                        uint id = uint.Parse((result["id"]).ToString());
                        string title = (result["title"]).ToString();

                        // Console.WriteLine("Process Title: {0}", result["title"]);
                        userProcesses.Add(new UserProcess(id, title));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            return userProcesses;
        }

        public List<string> GetProcessesNames()
        {
            List<string> userProcesses = new List<string>();

            string query = "SELECT title FROM user_processes";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    {
                        string title = (result["title"]).ToString();
                        userProcesses.Add(title);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            return userProcesses;
        }

        public void OpenConnection()
        {
            if(connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if(connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Clone();
            }
        }
    }
}
