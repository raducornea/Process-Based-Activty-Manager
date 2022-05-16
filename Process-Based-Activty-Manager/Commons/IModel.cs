using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
	public interface IModel
	{
		List<string> ProcessNameList
		{
			get;
		}


		void updateTimeSlot(int timeslotID, int duration);


		void setPresenter(IPresenter presenter);
		void ScreenWindowsProcesses();
		uint getProcessTotalTime(string processID);
		List<Timeslot> getProcessTimeslots(string processID);
		void addNewTimeSlot(string processID);
	}
}
