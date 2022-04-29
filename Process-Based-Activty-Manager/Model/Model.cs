using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using Commons;
using System.Threading;

namespace ActivityTracker
{
	public class Model : IModel
    {

        private DatabaseManager _database;
        private Thread _thread;

        public Model()
        {
            _database = new DatabaseManager();
            _thread = new Thread(new ThreadStart(GetAllProcesses));
        }

        public void StartThread()
        {
            _thread.Start();
        }

        public void GetAllProcesses()
        {
            try
            {
                while (true)
                {
                    List<string> processesFromDatabase = _database.GetProcessesNames();

                    Process[] processCollection = Process.GetProcesses();
                    foreach (Process p in processCollection)
                    {
                    
                        if (!processesFromDatabase.Contains(p.ProcessName))
                        {
                            try
                            {
                                _database.AddProcess(p.ProcessName);
                            }
                            catch (Exception e)
                            {
                                // Console.WriteLine(e.Message + " local");
                            }

                            Console.WriteLine("Am adaugat {0}", p.ProcessName);
                        }
                    }
                    Console.WriteLine("Ciclu terminat");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
