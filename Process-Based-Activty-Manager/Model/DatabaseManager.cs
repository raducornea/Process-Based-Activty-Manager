using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;

namespace ActivityTracker
{
    /// <summary>
    /// Clasa pentru operatii directe cu baza de date, folosita 
    /// pentru extragerea si afisarea informatiilor in interfata
    /// </summary>
    public class DatabaseManager
    {
        private SQLiteConnection _connection;
        private static DatabaseManager _instance = null;

        /// <summary>
        /// Obtine instanta clasei sau creaza una noua - Singleton
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                }
                return _instance;
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
            _connection = new SQLiteConnection(@"Data Source=" + path);

            OpenConnection();

            if (!File.Exists("./database.sqlite3"))
            {
                // da override la baza de date
                SQLiteConnection.CreateFile("database.sqlite3");
            }
        }

        // TODO: call CloseConnection upon exiting program
        ~DatabaseManager()
		{
            Debug.WriteLine("Destructor la DatabaseManager");
            CloseConnection();
		}


        /// <summary>
        /// Se adauga un nou proces in baza de date
        /// </summary>
        /// <param name="title"></param>
        public void AddProcess(string title)
        {
            string query = "INSERT INTO user_processes ('title') VALUES(@title)";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.Parameters.AddWithValue("@title", title);
            var result = command.ExecuteNonQuery();

            Console.WriteLine("Rows added: {0}", result);
        }

        /// <summary>
        /// Se obtin procesele din baza de date cu totul
        /// </summary>
        /// <returns></returns>
        public List<StoredProcess> GetProcesses()
        {
            List<StoredProcess> userProcesses = new List<StoredProcess>();

            string query = "SELECT * FROM user_processes";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    { 
                        uint id = uint.Parse((result["id"]).ToString());
                        string title = (result["title"]).ToString();

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

        /// <summary>
        /// Obtine lista numelor proceselor pentru a le afisa pe interfata
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessesNames()
        {
            List<string> userProcesses = new List<string>();

            string query = "SELECT title FROM user_processes";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

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

        /// <summary>
        /// Porneste conexiunea cu baza de date
        /// </summary>
        private void OpenConnection()
        {
            if(_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// Inchide conexiunea bazei de date
        /// </summary>
        private void CloseConnection()
        {
            if(_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
}
