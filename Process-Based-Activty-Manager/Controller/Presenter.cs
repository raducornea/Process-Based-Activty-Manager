/*******************************************************************************
 *                                                                             *
 *  File:        View.cs                                                       *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: View class from MVP pattern, to link View with Model          *
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
	public class Presenter : IPresenter
	{
		private IView _view;
		private IModel _model;

		public Presenter(IView view, IModel model)
		{
			_view = view;
			_model = model;
		}

		/// <summary>
		/// This fuction works periodically, called by a timer in the main form.
		/// </summary>
		public void presenterTick()
		{
			// Update the process lists
			_model.ScreenProcesses();

			// Display the updated process lists
			_view.DisplayActiveProcess(_model.ActiveProcessNames);

			// Display the complete list of processes
			_view.DisplayAllProcess(_model.AllProcessNames);

			// Update the timeslots
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
