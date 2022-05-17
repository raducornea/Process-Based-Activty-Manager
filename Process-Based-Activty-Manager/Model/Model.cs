using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ActivityTracker
{
	/// <summary>
	/// Model class from MVP Pattern - daca aveti adaugari, puneti aici
	/// </summary>
	public class Model : IModel
	{
		// current processes running ONLY
		// We need to avoid using the database very often
		// so this is it's in memory representation
		// Any diference between the database and this is a big problem
		// Properties
		private List<StoredProcess> _generalProcessList;   //This list is supposed to be a mirror of the database list of processes
														   //Any deviation is a big problem
		private IPresenter _presenter;
		private DatabaseManager _database;

		/// <summary>
		/// Proprietate pentru obtinerea proceselor rulante curente
		/// </summary>
		public List<StoredProcess> GeneralProcessList
		{
			get { return _generalProcessList; }
		}

		/// <summary>
		/// Proprietate pentru a primi numele proceselor curente
		/// </summary>
		public List<string> ProcessNameList
		{
			get
			{
				List<string> processNames = new List<string>();

				foreach (StoredProcess storedProcess in _generalProcessList)
				{
					processNames.Add(storedProcess.ProcessName);
				}
				return processNames;
			}
		}

		public List<long> ActiveTimestampsID
		{
			get
			{
				List<long> timeslotIDs = new List<long>();

				foreach (StoredProcess storedProcess in _generalProcessList)
				{
					timeslotIDs.Add(storedProcess.CurrentTimeslotID);
				}
				return timeslotIDs;
			}
		}


		/// <summary>
		/// Se cauta obtinerea timpului total petrecut pe un anumit proces din baza de date, in functie de numele lui
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		public uint GetProcessTotalTime(string processID)
		{
			return _database.GetTotalTimeForProcess(processID);
		}

		/// <summary>
		/// Se cauta toate timestamp-urile unui proces spre a fi afisate pe interfata grafica
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		public List<Timeslot> GetProcessTimeslots(string processID)
		{
			return _database.GetTimeSlotsForProcess(processID);
		}

		/// <summary>
		/// Se adauga un timeslot nou - posibil atunci cand apare un nou proces pe fundal
		/// </summary>
		/// <param name="processID"></param>
		public void AddNewTimeSlot(string processID)
		{
			_database.AddNewTimeSlot(processID);
		}

		/// <summary>
		/// Se updateaza timeslot-ul din baza de data - mai precis, se modifica durata de executie in functie de end time
		/// </summary>
		/// <param name="timeslotID"></param>
		/// <param name="duration"></param>
		void IModel.UpdateTimeSlots() 
		{
			foreach (var storedProcess in _generalProcessList)
			{
				_database.UpdateTimeSlot(storedProcess.CurrentTimeslotID, storedProcess.UniqueProcesID);
			}
		}

		/// <summary>
		/// Constructor pentru Model - trebuie initializata si lista de timestamp-uri, fiind prima rulare a programului
		/// </summary>
		public Model()
		{
			// se obtine instanta bazei de date
			_database = DatabaseManager.Instance;

			// For performance reasons we synchronize with the database only once at the start of the program.
			_generalProcessList = _database.GetProcesses();

			// se initializeaza lista cu timestamp-uri pentru procesere rulante curente
		}

		/// <summary>
		/// Se seteaza prezentatorul - MVP Pattern
		/// </summary>
		/// <param name="presenter"></param>
		public void SetPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}

		/// <summary>
		/// Se obtin toate procesele rulante curente si se adauga in baza de date
		/// TODO: nu ar trebui sa le transmita mai departe si pe interfata cu user-ul????
		/// </summary>
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
							//We insert the new found process and also recover it's ID
							string newProcessID = _database.AddProcess(p.ProcessName);

							long newTimeslotID = _database.AddNewTimeSlot(newProcessID);
							_generalProcessList.Add(new StoredProcess(newProcessID, p.ProcessName, newTimeslotID));
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