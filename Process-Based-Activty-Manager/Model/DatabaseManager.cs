using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;

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

        /// <summary>
        /// Realizeaza o conexiune SQLite in functie de path-uri diferite (dinamice)
        /// </summary>
        private DatabaseManager()
        {
            // trebuie avut in vedere ca aplicatia poate fi rulata de pe orice calculator, de aceea luam path-ul dinamic si nu-l setam ca absolute path
            string path = Directory.GetCurrentDirectory() + @"\database.sqlite3";

            // de asemenea, path-ul gasit este in ActivityManager, NU ActivityTracker. Am pus in "ActivtyManager\bin\Debug\netcoreapp3.1" database.sqlite3
            connection = new SQLiteConnection(@"Data Source=" + path);

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
