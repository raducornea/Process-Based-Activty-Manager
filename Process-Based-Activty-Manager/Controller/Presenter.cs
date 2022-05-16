using System;
using System.Collections.Generic;

namespace ActivityTracker
{
	public class Presenter : IPresenter
	{
		private IView _view;
		private IModel _model;
		private List<string> _processesFromDatabase;

		List<string> IPresenter.ComputerProcesses
		{
			get { return _processesFromDatabase; }
		}

		public Presenter(IView view, IModel model)
		{
			_view = view;
			_model = model;
		}

		public void presenterTick()
		{
			_model.ScreenWindowsProcesses();
			_view.UpdateProcessList(_model.ProcessNameList);

		}
	}
}
