using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;

namespace ActivityTracker
{
	public class Model : IModel
	{
		private IPresenter _presenter;
		private DatabaseManager _database;
		private List<StoredProcess> _generalProcessList;  //We need to avoid using the database very often
														  // so this is it's in memory representation
														  //Any diference between the database and this is a big problem
														  //Properties
		public List<StoredProcess> GeneralProcessList
		{
			get { return _generalProcessList; }
		}

		public List<string> ProcessNameList
		{
			get
			{
				List<string> _processNames = new List<string>();


				foreach (StoredProcess storedProcess in _generalProcessList)
				{
					_processNames.Add(storedProcess.ProcessName);
				}
				return _processNames;
			}
		}

		//TODO: implementeaza - RADUCU
		public uint getProcessTotalTime(uint processID)
		{
			throw new Exception("Nimeni nu facut asta");
		}

		//TODO: implementeaza - RADUCU
		public List<Timeslot> getProcessTimeslots(uint processID)
		{
			throw new Exception("Nimeni nu facut asta");
		}

		public string addNewTimeSlot(uint processID)
		{
			throw new Exception("Nimeni nu facut asta");

			string uniqueID = processID.ToString();


			return uniqueID;
		}

		public void updateTimeSlot(string timeslotID, int duration) 
		{
			throw new Exception("Nimeni nu facut asta");
		}

		public Model()
		{
			_database = DatabaseManager.Instance;

			//For performance reasons we sincronyze with the database only once at the start of the program.
			_generalProcessList = _database.GetProcesses();
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}

		public void ScreenWindowsProcesses()
		{
			try
			{
				Process[] processCollection = Process.GetProcesses();
				foreach (Process p in processCollection)
				{

					if (!ProcessNameList.Contains(p.ProcessName))
					{
						try
						{
							// it might be more performant to agregate all new peocesses and add the all in a single database
							//quary but not really a priority
							_database.AddProcess(p.ProcessName);

							//Usually we would read a process from the database along with it's id.
							//But when a process is first detected, there is no representation of it in the database
							//Because of that we need to att in the database, recover when id it was given and finally
							//add that new process in the runtime list with processes
							//The id is requited as to properly write timeslopts in the database for the new peocess as soon as
							//it is detected

							//TODO: Raducu

							//_generalProcessList.Add(new StoredProcess( , ));
						}
						catch (Exception e)
						{
							Console.WriteLine("Model exception" + e.Message);
						}

						Console.WriteLine("Am adaugat {0}", p.ProcessName);
					}
				}
				Console.WriteLine("Ciclu terminat");

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}