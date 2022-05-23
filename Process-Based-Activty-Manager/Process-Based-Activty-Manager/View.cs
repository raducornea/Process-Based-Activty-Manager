/*******************************************************************************
 *                                                                             *
 *  File:        View.cs                                                       *
 *  Copyright:   (c) 2022, Atomulesei Paul-Costin                              *
 *  E-mail:      paul-costin.atomulesei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: View class from MVP pattern, to link Forms                    *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

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
