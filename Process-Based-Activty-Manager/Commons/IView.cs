/*******************************************************************************
 *                                                                             *
 *  File:        IView.cs                                                      *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Interface for upcoming View classes                           *
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
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
	public interface IView
	{
		void setPresenter(IPresenter presenter);
		void Display();
		void DisplayActiveProcess(List<string> processNames);
		void DisplayAllProcess(List<string> processNames);

	}
}
