using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ActivityTracker
{
    public class DatabaseManager
    {
        private SQLiteConnection connection;
        private static DatabaseManager instance = null;

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }
        }

        private DatabaseManager()
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

        ~DatabaseManager()
		{
            CloseConnection();
		}

        public void AddProcess(string title)
        {
            string query = "INSERT INTO user_processes ('title') VALUES(@title)";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@title", title);
            var result = command.ExecuteNonQuery();

            Console.WriteLine("Rows added: {0}", result);
        }

        public List<StoredProcess> GetProcesses()
        {
            List<StoredProcess> userProcesses = new List<StoredProcess>();

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
                        userProcesses.Add(new StoredProcess(id, title));
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

        public uint getTotalTimeForProcess(StoredProcess process)
		{
            throw new Exception("Nobody done this");
		}
        
        public List<Timeslot> gotTimeSlotsForProcess(StoredProcess process)
		{
            throw new Exception("Nobody done this");
        }

        private void OpenConnection()
        {
            if(connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private void CloseConnection()
        {
            if(connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}
