using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivityTracker
{
	public class View : IView
	{
		private IPresenter _presenter;
		private MainForm _mainWindow;

		public View()
		{
			init();
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
			_mainWindow.setPresenter(_presenter);
		}

		[STAThread]
		void init()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			_mainWindow = new MainForm();
		}

		//INTERFACE 

		public void Display()
		{
			Application.Run(_mainWindow);
		}

		public void DisplayActiveProcess(List<string> processNames)
		{
			_mainWindow.UpdateActiveProcessesList(processNames);
		}

		public void DisplayAllProcess(List<string> processNames)
		{
			_mainWindow.UpdateAllProcessesList(processNames);
		}
	}
}
