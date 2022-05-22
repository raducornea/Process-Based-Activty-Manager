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
        public static DatabaseManager Instance(string databaseName)
        {
            if (_instance == null)
            {
                _instance = new DatabaseManager(databaseName);
            }
            return _instance;
        }

        /// <summary>
        /// Realizeaza o conexiune SQLite in functie de path-uri diferite (dinamice)
        /// </summary>
        private DatabaseManager(string databaseName)
        {
            // Vor exista doua baze de date: una pentru teste si una pentru aplicatia propriu-zisa.
            if (databaseName == "")
            {
                databaseName = "database.sqlite3";
            }

            // Trebuie avut in vedere ca aplicatia poate fi rulata de pe orice calculator,
            // de aceea luam path-ul dinamic si nu-l setam ca absolute path.
            string path;
            path = Directory.GetCurrentDirectory() + @"\" + databaseName;

            // Path-ul gasit este in ActivityManager
            _connection = new SQLiteConnection(@"Data Source=" + path);

            OpenConnection();

            if (!File.Exists("./" + databaseName))
            {
                SQLiteConnection.CreateFile(databaseName);
            }

            // In cazul in care nu exista tabele, se vor crea
            CreateTables();
        }

        /// <summary>
        /// Destructor de apelat cand se termina form-ul sau testele
        /// </summary>
        ~DatabaseManager()
		{
            Debug.WriteLine("Destructor la DatabaseManager");
            CloseConnection();
		}

        /// <summary>
        /// Cand nu sunt tabele create, creaza-le automat
        /// NOTE: there is not DATE type in SQLite. We use REAL instead
        /// Docs: https://www.sqlite.org/lang_datefunc.html
        /// </summary>
        public void CreateTables()
        {
            string queryUserProcesses = @"CREATE TABLE IF NOT EXISTS user_processes (
                id VARCHAR(100) PRIMARY KEY UNIQUE,
                title VARCHAR(100) NOT NULL UNIQUE
            );";
            SQLiteCommand command = new SQLiteCommand(queryUserProcesses, _connection);
            command.ExecuteNonQuery();

            string queryTimestamps = @"CREATE TABLE IF NOT EXISTS timestamps ( 
                id INTEGER NOT NULL ,  
                pid VARCHAR(100) NOT NULL,  
                date_start REAL, 
                date_stop REAL,  
                FOREIGN KEY(pid) REFERENCES user_processes(id),  
                PRIMARY KEY (id, pid)  
            );";
            command = new SQLiteCommand(queryTimestamps, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Se adauga un nou proces in baza de date
        /// </summary>
        /// <param name="title"></param>
        public string AddProcess(string title)
        {
            // trebuie pus si OR IGNORE in cazul in care nu e Unic
            string query = "INSERT OR IGNORE INTO user_processes ('id', 'title') VALUES(@id, @title)";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            string newProcessID = "id:" + title;
            command.Parameters.AddWithValue("@id", newProcessID);
            command.Parameters.AddWithValue("@title", title);

            // se adauga {result} randuri in tabela;
            var result = command.ExecuteNonQuery();
            return newProcessID;
        }

        /// <summary>
        /// Se obtin procesele din baza de date cu totul
        /// </summary>
        /// <returns>Se returneaza un obiect de tip StoredProcess</returns>
        public StoredProcess GetProcessFromName(string name)
		{
            StoredProcess storedProcess = null;

            string query = @"SELECT * FROM user_processes WHERE title == @title";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            command.Parameters.AddWithValue("@title", name);
       
            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                if (result.StepCount > 1)
				{
                    throw new Exception("Processes with the name name in the database.");
				}
                try
                {
                    string id = (result["id"]).ToString();
                    string title = (result["title"]).ToString();
                    storedProcess = new StoredProcess(id, title);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            return storedProcess;
        }

        /// <summary>
        /// Se obtine lista de procese, ca obiecte de tip StoredProcess, din baza de date
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
                    catch(Exception exception)
                    {
                        Console.WriteLine(exception.StackTrace);
                    }
                }
            }

            return userProcesses;
        }

        /// <summary>
        /// Se sterg toate procesele din tabela, structura ei ramanand intacta
        /// </summary>
        public void DeleteProcessTable()
        {
            string query = "DELETE FROM user_processes";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            SQLiteDataReader result = command.ExecuteReader();
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


        /// <summary>
        /// Pentru un anumit proces se genereaza un timestamp nou care are atat la start cat si end aceeasi data
        /// </summary>
        /// <param name="processID"></param>
        public long AddNewTimeSlot(string processID)
        {
            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            string query = "INSERT OR IGNORE INTO timestamps ('id','pid', 'date_start', 'date_stop') " +
                "VALUES(@ID, @processID, @timestamp, @timestamp)";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.Parameters.AddWithValue("@ID", timestamp);
            command.Parameters.AddWithValue("@processID", processID);
            command.Parameters.AddWithValue("@timestamp", timestamp);

            command.ExecuteNonQuery();

            return timestamp;
        }

        /// <summary>
        /// Se obtin toate timestamp-urile pentru un proces, ca sa le vada utilizatorul
        /// </summary>
        /// <param name="processID"></param>
        /// <returns>Lista tuturor timestamp-urilor</returns>
        public List<Timeslot> GetTimeSlotsForProcess(string processID)
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
                            long.Parse(result["id"].ToString()),
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

        /// <summary>
        /// Avem nevoie sa stim duratele de viata ale proceselor spre informarea utilizatorilor
        /// </summary>
        /// <param name="processID"></param>
        /// <returns>Timpul rularii totale a aplicatiei de la inceput pana la prezent</returns>
        public uint GetTotalTimeForProcess(string processID)
        {
            uint timestampsSum = 0;

            string query = "SELECT sum(date_stop - date_start) AS timestamps_sum " +
                "FROM timestamps " +
                "INNER JOIN user_processes ON " +
                "timestamps.pid == user_processes.id " +
                "WHERE pid == '" + processID + "'";
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            SQLiteDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    try
                    {
                        timestampsSum = uint.Parse(result["timestamps_sum"].ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            return timestampsSum;
        }

        /// <summary>
        /// Pe parcursul aplicatiei e nevoie sa updatam si timestamp-urile din cand in cand, ca sa nu ramana aceleasi date
        /// </summary>
        /// <param name="timeslotID"></param>
        /// <param name="duration"></param>
        public void UpdateTimeSlot(long timestampID, string processID)
        {
            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();


            string query = @"UPDATE timestamps  
                            SET date_stop =  @timestamp
                            WHERE id == @ID AND pid == @processID;";

            SQLiteCommand command = new SQLiteCommand(query, _connection);

            command.Parameters.AddWithValue("@timestamp", timestamp);
            command.Parameters.AddWithValue("@ID", timestampID);
            command.Parameters.AddWithValue("@processID", processID);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Se sterg toate timestamps din tabela, structura ei ramanand intacta
        /// </summary>
        public void DeleteTimeSlotsTable()
        {
            string query = "DELETE FROM timestamps";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            SQLiteDataReader result = command.ExecuteReader();
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
