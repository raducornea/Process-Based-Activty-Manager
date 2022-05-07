using System;
using System.Collections.Generic;
using Commons;

namespace ActivityTracker
{
	public class Presenter : IPresenter
	{
		IView _view;
		IModel _model;
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
			_model.GetAllProcesses();
			_processesFromDatabase = _model.ProcessNameList;
		}
	}
}
