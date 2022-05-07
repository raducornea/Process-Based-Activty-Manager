using Commons;
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

		private Form1 mainWindow;

		public View()
		{
			init();
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
			mainWindow.setPresenter(_presenter);

		}

		[STAThread]
		void init()
        {
			 Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			mainWindow = new Form1();
		}

		void IView.Display()
		{
			Application.Run(mainWindow);
		}
	}

	class Program {
		static void Main(string[] args)
		{
			
		}
	}
	

}
