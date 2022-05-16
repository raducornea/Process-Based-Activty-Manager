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
		private List<Timeslot> _timeslotsList; // current processes running ONLY
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

		public uint getProcessTotalTime(string processID)
		{
			return _database.getTotalTimeForProcess(processID);
		}

		public List<Timeslot> getProcessTimeslots(string processID)
		{
			return _database.gotTimeSlotsForProcess(processID);
		}

		public void addNewTimeSlot(string processID)
		{
			_database.addNewTimeSlot(processID);
		}

		public void updateTimeSlot(int timeslotID, int duration) 
		{
			_database.updateTimeSlot(timeslotID, duration);
		}

		public Model()
		{
			_database = DatabaseManager.Instance;

			// For performance reasons we synchronize with the database only once at the start of the program.
			_generalProcessList = _database.GetProcesses();

			List<Timeslot> _timeslotsList = new List<Timeslot>();
			foreach (StoredProcess process in _generalProcessList)
            {
				_database.addNewTimeSlot(process.UniqueProcesID);
				_timeslotsList.AddRange(_database.gotTimeSlotsForProcess(process.UniqueProcesID));
			}

			foreach(Timeslot timeslot in _timeslotsList)
            {
				Debug.WriteLine(timeslot.getStartTime() + " " + timeslot.getEndTime());
            }
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
							// it might be more performant to aggregate all new peocesses and add all in a single database
							// query, but not really a priority
							_database.AddProcess(p.ProcessName);

							// Usually, we read a process from the database - along with it's ID.
							// But when a process is first detected, there is no representation of it in the database.
							// Because of that, we need to add it in the database, recover when id it was given and finally
							// add that new process in the runtime list with processes
							// The id is requited as to properly write timeslopts in the database for the new peocess as soon as
							// it is detected

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