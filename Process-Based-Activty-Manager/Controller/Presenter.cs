using System;
using System.Collections.Generic;

namespace ActivityTracker
{
	public class Presenter : IPresenter
	{
		private IView _view;
		private IModel _model;

		public Presenter(IView view, IModel model)
		{
			_view = view;
			_model = model;
		}

		//This fuction works periodically, called by a timer in the main form.
		public void presenterTick()
		{
			//Update the process lists
			_model.ScreenProcesses();

			//Display the updated process lists
			_view.DisplayActiveProcess(_model.ActiveProcessNames);


			//Display the complete list of processes
			_view.DisplayAllProcess(_model.AllProcessNames);

			//Update the timeslots
			_model.UpdateTimeSlots();
		}

		public void DeleteDatabase()
		{
			_model.ResetAllInformation();
		}


		public List<Timeslot> RequestTimeslots(string processName)
		{
			return _model.GetProcessTimeslots(processName);
		}

		public uint RequestProcessTotalTime(string processName)
        {
			return _model.GetProcessTotalTime(processName);
        }
	}
}
