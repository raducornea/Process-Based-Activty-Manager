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
		private CompositeProcessList _generalProcessList;

		private IPresenter _presenter;
		private DatabaseManager _database;

		/// <summary>
		/// Constructor pentru Model - trebuie initializata si lista de timestamp-uri, fiind prima rulare a programului
		/// </summary>
		public Model()
		{
			// se obtine instanta bazei de date
			_database = new DatabaseManager("");

			// For performance reasons we synchronize with the database only once at the start of the program.
			// All the processes here don't have an active timestamp
			_generalProcessList = new CompositeProcessList();

			_generalProcessList.AddDormantProcesses(_database.GetProcesses());
		}

		//INTERFACE

		public List<StoredProcess> AllProcessesList
		{
			get { return _generalProcessList.AllProcessesList; }
		}

		public List<string> AllProcessNames
		{
			get {
				List<string> processNames = new List<string>();
				foreach (StoredProcess storedProcess in _generalProcessList.AllProcessesList)
				{
					processNames.Add(storedProcess.ProcessName);
				}
				return processNames;
			}
		}
		public List<ActiveProcess> ActiveProcess
		{
			get { return _generalProcessList.ActiveProcessesList; }
		}

		/// <summary>
		/// Proprietate pentru a primi numele proceselor curente
		/// </summary>
		public List<string> ActiveProcessNames
		{
			get
			{
				List<string> processNames = new List<string>();
				foreach (StoredProcess storedProcess in _generalProcessList.ActiveProcessesList)
				{
					processNames.Add(storedProcess.ProcessName);
				}
				return processNames;
			}
		}


		///END INTERFACE

		public List<string> DormandProcessNames
		{
			get
			{
				List<string> processNames = new List<string>();
				foreach (StoredProcess storedProcess in _generalProcessList.DormantProcessesList)
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

				foreach (ActiveProcess activeProcess in _generalProcessList.ActiveProcessesList)
				{
					timeslotIDs.Add(activeProcess.CurrentTimeslotID);
				}
				return timeslotIDs;
			}
		}


		public void ResetAllInformation()
		{
			_database.DeleteTables();
			_generalProcessList.RemoveEverything();
		}


		/// <summary>
		/// Se cauta obtinerea timpului total petrecut pe un anumit proces din baza de date, in functie de numele lui
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		public uint GetProcessTotalTime(string processName)
		{
			return _database.GetTotalTimeForProcess("id:" + processName);
		}

		/// <summary>
		/// Se cauta toate timestamp-urile unui proces spre a fi afisate pe interfata grafica
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		public List<Timeslot> GetProcessTimeslots(string processName)
		{
			return _database.GetTimeSlotsForProcess("id:" + processName);
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
			foreach (var activeProcess in _generalProcessList.ActiveProcessesList)
			{
				_database.UpdateTimeSlot(activeProcess.CurrentTimeslotID, activeProcess.UniqueProcesID);
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
		/// The core of our fuctionality
		/// </summary>
		public void ScreenProcesses()
		{
			//We recover all active processes
			var processCollection = new List<Process>(Process.GetProcesses());

			foreach (Process APIProcess in processCollection)
			{
				//Daca e in lista de procese recuperate dar nu e in lista de procese active,
				//recupereaza detalii din baza de date si adauga la lista de procese active
				if (!ActiveProcessNames.Contains(APIProcess.ProcessName))
				{
					StoredProcess storedProcess = null;

					//Daca nu exista deloc in baza de date trebuie adaugat si aici
					if (!DormandProcessNames.Contains(APIProcess.ProcessName))
					{
						try
						{
							//We insert the new found process and also recover it's ID
							string processID = _database.AddProcess(APIProcess.ProcessName);

							storedProcess = new StoredProcess(processID, APIProcess.ProcessName);

							_generalProcessList.AddDormantProcess(storedProcess);
						}
						catch (Exception e)
						{
							Console.WriteLine("Model exception" + e.Message);
						}
						Console.WriteLine("Am adaugat {0} in baza de date.", APIProcess.ProcessName);
					}
					else   //Daca exista in baza de date, doar recuperam detalii pe baza numelui
					{
						//storedProcess = _database.GetProcessFromName(APIProcess.ProcessName);

						storedProcess = new StoredProcess("id:"+APIProcess.ProcessName, APIProcess.ProcessName);
					}

					//Aici doar adaugam un nou timeslot pentru procesul activ
					long newTimeslotID = _database.AddNewTimeSlot(storedProcess.UniqueProcesID);

					//Activam procesul ca timeslot-ul lui sa fie updatat
					_generalProcessList.ActivateProcess(storedProcess, APIProcess, newTimeslotID);
				}
			}

			// Verificare daca vreun process activ nu a disparut cumva din API
			List<string> processCollectionNames = new List<string>();
			foreach( var process in processCollection)
			{
				processCollectionNames.Add(process.ProcessName);
			}

			foreach (ActiveProcess activeProcess in _generalProcessList.ActiveProcessesList)
			{
				if (!processCollectionNames.Contains(activeProcess.ProcessName))
				{
					_generalProcessList.DeactivateProcess(activeProcess);
					Console.WriteLine("Am dezactivat procesul {0}", activeProcess.ProcessName);
				}
			}
			_generalProcessList.ReadyToRemove();

			Console.WriteLine("Ciclu terminat");
		}
	}
}