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
	


		[STAThread]
		void IView.Display()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());

		}

		public void addProcessToTable()
		{



		}
	}
}
