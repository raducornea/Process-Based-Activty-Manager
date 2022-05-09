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
		void UpdateProcessList(List<string> processNames);

	}
}
