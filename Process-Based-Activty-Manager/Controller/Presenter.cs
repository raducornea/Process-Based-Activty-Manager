using System;
using Commons;

namespace ActivityTracker
{
	public class Presenter : IPresenter
	{
		IView _view;
		IModel _model;

		public Presenter(IView view, IModel model)
		{
			_view = view;
			_model = model;

			while (true) {
				_model.StartThread();
				_view.addProcessToList(_model.ProcessNameList);
			}
		}


	}
}
