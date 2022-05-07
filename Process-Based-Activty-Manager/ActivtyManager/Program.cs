using ActivityTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivtyManager
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			IView view = new ActivityTracker.View();
			IModel model = new Model();
			IPresenter presenter = new Presenter(view, model);
			view.setPresenter(presenter);
			model.setPresenter(presenter);
			view.Display();
		}
	}
}
