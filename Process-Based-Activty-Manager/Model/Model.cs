﻿/*******************************************************************************
 *                                                                             *
 *  File:        Model.cs                                                      *
 *  Copyright:   (c) 2022, Cornea Radu-Valentin                                *
 *  E-mail:      radu-valentin.cornea@student.tuiasi.ro                        *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Model class from MVP pattern that works with the data         *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

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
		private IPresenter _presenter;
		private CompositeProcessList _generalProcessList;
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

		/// <summary>
		/// Se seteaza prezentatorul - MVP Pattern
		/// </summary>
		/// <param name="presenter"></param>
		public void SetPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}

		public List<StoredProcess> AllProcessesList
		{
			get => _generalProcessList.AllProcessesList;
		}
		public List<ActiveProcess> ActiveProcess
		{
			get => _generalProcessList.ActiveProcessesList;
		}

		/// <summary>
		/// Proprietate pentru a obtine o lista cu toate numele proceselor
		/// </summary>
		public List<string> AllProcessNames
		{
			get 
			{
				List<string> processNames = new List<string>();
				foreach (StoredProcess storedProcess in _generalProcessList.AllProcessesList)
				{
					processNames.Add(storedProcess.ProcessName);
				}
				return processNames;
			}
		}

		/// <summary>
		/// Proprietate pentru a obtine toate numele proceselor active
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

		/// <summary>
		/// Proprietate pentru a obtine toate numele proceselor inactive
		/// </summary>
		private List<string> DormandProcessNames
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

		//FUCNTII

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
		/// Se updateaza timeslot-ul din baza de data - mai precis, se modifica durata de executie in functie de end time
		/// </summary>
		/// <param name="timeslotID"></param>
		/// <param name="duration"></param>
		public void UpdateTimeSlots() 
		{
			foreach (var activeProcess in _generalProcessList.ActiveProcessesList)
			{
				_database.UpdateTimeSlot(activeProcess.CurrentTimeslotID, activeProcess.UniqueProcesID);
			}
		}

		/// <summary>
		/// The core of our fuctionality
		/// </summary>
		public void ScreenProcesses()
		{
			// We recover all active processes
			var processCollection = new List<Process>(Process.GetProcesses());

			foreach (Process APIProcess in processCollection)
			{
				// Daca e in lista de procese recuperate dar nu e in lista de procese active,
				// recupereaza detalii din baza de date si adauga la lista de procese active
				if (!ActiveProcessNames.Contains(APIProcess.ProcessName))
				{
					StoredProcess storedProcess = null;

					// Daca nu exista deloc in baza de date trebuie adaugat si aici
					if (!DormandProcessNames.Contains(APIProcess.ProcessName))
					{
						try
						{
							// We insert the new found process, also recover it's ID
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
					// Daca exista in baza de date, doar recuperam detalii pe baza numelui
					else
					{
						storedProcess = new StoredProcess("id:" + APIProcess.ProcessName, APIProcess.ProcessName);
					}

					// Adaugam un timeslot nou pentru procesul activ
					long newTimeslotID = _database.AddNewTimeSlot(storedProcess.UniqueProcesID);

					// Activam procesul ca timeslot-ul lui sa fie updatat
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

		/// <summary>
		/// Pentru a sterge in intregime informatia monitorizata
		/// </summary>
		public void ResetAllInformation()
		{
			_database.DeleteTables();
			_generalProcessList.RemoveEverything();
		}
	}
}