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
                SQLiteConnection.CreateFile("database.sqlite3");
            }

            // in cazul in care nu exista tabele, se vor crea
            CreateTables();
        }

        // TODO: call CloseConnection upon exiting program
        ~DatabaseManager()
		{
            Debug.WriteLine("Destructor la DatabaseManager");
            CloseConnection();
		}

        /// <summary>
        /// When there are no tables created, create them automatically
        /// NOTE: there is not DATE type in SQLite. We use REAL instead
        /// Docs: https://www.sqlite.org/lang_datefunc.html
        /// </summary>
        public void CreateTables()
        {
            string queryUserProcesses = "CREATE TABLE IF NOT EXISTS user_processes (" +
                "id VARCHAR(100) PRIMARY KEY UNIQUE," +
                "title VARCHAR(100) NOT NULL UNIQUE" +
            ")";
            SQLiteCommand command = new SQLiteCommand(queryUserProcesses, _connection);
            command.ExecuteNonQuery();

            string queryTimestamps = "CREATE TABLE IF NOT EXISTS timestamps (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "pid VARCHAR(100), " +
                "date_start REAL, " +
                "date_stop REAL, " +
                "FOREIGN KEY(pid) REFERENCES user_processes(id), " +
                "UNIQUE (id, pid) " +
            ")";
            command = new SQLiteCommand(queryTimestamps, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Se adauga un nou proces in baza de date
        /// </summary>
        /// <param name="title"></param>
        public void AddProcess(string title)
        {
            // trebuie pus si OR IGNORE in cazul in care nu e Unic
            string query = "INSERT OR IGNORE INTO user_processes ('id', 'title') VALUES(@id, @title)";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.Parameters.AddWithValue("@id", "id:" + title);
            command.Parameters.AddWithValue("@title", title);
            var result = command.ExecuteNonQuery();

            // Console.WriteLine("Rows added: {0}", result);
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
                        string id = (result["id"]).ToString();
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

        public uint getTotalTimeForProcess(string processID)
		{
            uint timestampsSum = 0;

            string query = "SELECT sum(date_stop - date_start) AS timestamps_sum " +
                "FROM timestamps " +
                "INNER JOIN user_processes ON " +
                "timestamps.pid = user_processes.id " +
                "WHERE pid == " + processID;
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    {
                        timestampsSum = uint.Parse(result["timestamps_sum"].ToString());
                        // Debug.WriteLine(timestampsSum);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            return timestampsSum;
        }

        public List<Timeslot> gotTimeSlotsForProcess(string processID)
		{
            List<Timeslot> timestamps = new List<Timeslot>();

            string query = "SELECT id, pid, date_start, date_stop " +
                "FROM timestamps " +
                "WHERE pid = '" + processID + "'";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    {
                        Timeslot timeslot = new Timeslot(
                            result["id"].ToString(),
                            result["pid"].ToString(),
                            long.Parse(result["date_start"].ToString()),
                            long.Parse(result["date_stop"].ToString())
                        );
                        
                        timestamps.Add(timeslot);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            return timestamps;
        }

        public void addNewTimeSlot(string processID)
        {
            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            string query = "INSERT OR IGNORE INTO timestamps ('pid', 'date_start', 'date_stop') " +
                "VALUES(@processID, @timestamp, @timestamp)";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.Parameters.AddWithValue("@processID", processID);
            command.Parameters.AddWithValue("@timestamp", timestamp);

            command.ExecuteNonQuery();
        }

        public void updateTimeSlot(int timeslotID, int duration)
        {
            string query = "UPDATE timestamps " +
                "SET date_stop = date_stop + (" + duration + ") " +
                "WHERE id == " + timeslotID + " ";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.ExecuteNonQuery();
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
