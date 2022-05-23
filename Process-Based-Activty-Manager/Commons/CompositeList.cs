/*******************************************************************************
 *                                                                             *
 *  File:        CompositeList.cs                                              *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Combines two lists, one for active and one for inactive       *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

using System.Collections.Generic;

namespace ActivityTracker
{
	public class CompositeProcessList
	{
		private List<StoredProcess> _dormantProcessList;
		private List<ActiveProcess> _activeProcessList;
		private List<ActiveProcess> _activeProcessToRemove;

		public CompositeProcessList()
		{
			_dormantProcessList = new List<StoredProcess>();
			_activeProcessList = new List<ActiveProcess>();
			_activeProcessToRemove = new List<ActiveProcess>();
		}

		/// <summary>
		/// You recover every process, be it dormant or active
		/// </summary>
		public List<StoredProcess> AllProcessesList
		{
			get
			{
				List<StoredProcess> allProcessesList = _dormantProcessList;
				allProcessesList.AddRange(_activeProcessList);
				return allProcessesList;
			}
		}

		public List<StoredProcess> DormantProcessesList
		{
			get
			{
				return _dormantProcessList;
			}
		}

		public List<ActiveProcess> ActiveProcessesList
		{
			get
			{
				return _activeProcessList;
			}
		}

		// The cornerstone of this design
		public void ActivateProcess(StoredProcess storedProcess, System.Diagnostics.Process p, long newTimeslotID)
		{
			_dormantProcessList.Remove(storedProcess);
			_activeProcessList.Add(new ActiveProcess(storedProcess, p, newTimeslotID));
		}

		public void DeactivateProcess(ActiveProcess activeProcess)
		{
			_activeProcessToRemove.Add(activeProcess);	// Everything here will be deleted when the 
			_dormantProcessList.Add(activeProcess);		// not sure what happens here
		}

		public void ReadyToRemove()
		{
			foreach(var activeProcess in _activeProcessToRemove)
			{
				_activeProcessList.Remove(activeProcess);
			}
			_activeProcessToRemove.Clear();
		}

		public void AddDormantProcesses(List<StoredProcess> newDormantProcesses)
		{
			_dormantProcessList.AddRange(newDormantProcesses);
		}

		public void AddDormantProcess(StoredProcess newDormantProcess)
		{
			_dormantProcessList.Add(newDormantProcess);
		}

		public void RemoveEverything()
		{
			_dormantProcessList.Clear();
			_activeProcessList.Clear();
		}
	}
}
