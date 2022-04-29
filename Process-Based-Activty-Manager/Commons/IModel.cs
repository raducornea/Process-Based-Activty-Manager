using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
	public interface IModel
	{
		List<string> ProcessNameList
		{
			get;
		}

		void setPresenter(IPresenter presenter);

		void StartThread();
	}
}
