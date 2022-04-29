using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivityTracker
{
	 class View :IView
	{
		private Form1 mainWindow;
		[STAThread]
		
		void IView.init()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			mainWindow = new Form1();

		}
		
		
		void IView.Display()
		{
			
			Application.Run(mainWindow);

		}

        void IView.addProcessToList()
        {
			mainWindow.informationInjection();
		}

    }


	class Program
	{
		static void Main(string[] args)
		{
			IView view = new View();
			view.init();
			view.addProcessToList();
			view.Display();
			

		}
	}

}
