using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{

	public interface IPresenter
	{

		List<Timeslot> RequestTimeslots(string processName);
		uint RequestProcessTotalTime(string processName);

		//Actions
		void presenterTick();
		void DeleteDatabase();
	}
}
