/*******************************************************************************
 *                                                                             *
 *  File:        Program.cs                                                    *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Entry point for application where MVP pattern is made         *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

using ActivityTracker;
using System;

namespace ActivtyManager
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			// MVP Pattern implemented
			IView view = new ActivityTracker.View();
			IModel model = new Model();
			IPresenter presenter = new Presenter(view, model);
			view.SetPresenter(presenter);
			model.SetPresenter(presenter);
			view.Display();
		}
	}
}
