using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		//You recover every process, be it dormant or active
		public List<StoredProcess> AllProcessesList
		{
			get{
				List<StoredProcess> allProcessesList = _dormantProcessList;

				allProcessesList.Concat(_activeProcessList);
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

		//The cornerstone of this design
		public void ActivateProcess(StoredProcess storedProcess, System.Diagnostics.Process p, long newTimeslotID)
		{
			_dormantProcessList.Remove(storedProcess);
			_activeProcessList.Add(new ActiveProcess(storedProcess, p, newTimeslotID));
		}

		public void DeactivateProcess(ActiveProcess activeProcess)
		{
			_activeProcessToRemove.Add(activeProcess);
			_dormantProcessList.Add(activeProcess);//not sure what happens here
		}

		public void UpdateList()
		{
			foreach(var activeProcess in _activeProcessToRemove)
			{
				_activeProcessList.Remove(activeProcess);
			}
			_activeProcessToRemove.Clear();
		}

		//Some other stuff

		public void AddDormantProcesses(List<StoredProcess> newDormantProcesses)
		{
			_dormantProcessList.Concat(newDormantProcesses);
		}

		public void AddDormantProcesses(StoredProcess newDormantProcess)
		{
			_dormantProcessList.Add(newDormantProcess);
		}

	}
}
