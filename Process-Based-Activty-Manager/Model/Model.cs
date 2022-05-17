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
		private List<Timeslot> _timeslotsList;
		private List<StoredProcess> _generalProcessList;
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
				List<string> _processNames = new List<string>();

				foreach (StoredProcess storedProcess in _generalProcessList)
				{
					_processNames.Add(storedProcess.ProcessName);
				}
				return _processNames;
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
		public void UpdateTimeSlot(int timeslotID, int duration) 
		{
			_database.UpdateTimeSlot(timeslotID, duration);
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
			_timeslotsList = new List<Timeslot>();
			foreach (StoredProcess process in _generalProcessList)
            {
				// se genereaza un timeslot nou procesului tinta
				_database.AddNewTimeSlot(process.UniqueProcesID);

				// se adauga in lista toate timeslot-urile unui proces
				_timeslotsList.AddRange(_database.GetTimeSlotsForProcess(process.UniqueProcesID));
			}
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