using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{

	public interface IPresenter
	{
		List<string> ComputerProcesses
		{
			get;
		}

		void presenterTick();

		List<Timeslot> RequestTimeslots(string processName);
	}
}
