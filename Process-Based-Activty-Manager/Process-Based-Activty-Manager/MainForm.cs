using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ActivityTracker
{
	public partial class MainForm : Form
	{

		static private IPresenter _presenter;
		private DetailsForm currentDetailsWindow;

		public MainForm()
		{
			InitializeComponent();
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}

		//TOOL MENU
		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		//ACTIVE PROCESS LIST

		// when a process is clicked a new window of type Form2 is opened for displaying information about the process
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxActiveProcesses.SelectedItem != null) {
				//We close the old window

				if(currentDetailsWindow != null) { 
					currentDetailsWindow.Close();

				}
		

				//And open a new updated window
				currentDetailsWindow = new DetailsForm();

				var timeslots = _presenter.RequestTimeslots(listBoxActiveProcesses.SelectedItem.ToString());
				currentDetailsWindow.displayTimeslots(timeslots);

				currentDetailsWindow.Text = listBoxActiveProcesses.SelectedItem.ToString();
				currentDetailsWindow.Show();
			}
		}

		//Here we run the recurent logic of this aplication (witch is most of 
		private void sampleTimer_Tick(object sender, EventArgs e)
		{
			if (_presenter != null)
			{
				_presenter.presenterTick();


			//	if (currentDetailsWindow != null)
			//	{
					//Multithreaded issues
					//var timeslots = _presenter.RequestTimeslots(listBoxActiveProcesses.SelectedItem.ToString());
				//	currentDetailsWindow.displayTimeslots(timeslots);
			//	}
			}
		}

		public void AddProcessToList(List<string> processNames)
		{
			foreach(var name in processNames)
			{
				if (!listBoxActiveProcesses.Items.Contains(name))
				{
					listBoxActiveProcesses.Items.Add(name);
				}
			}


			List<object> toRemoveList = new List<object>();

			foreach (var displayedName in listBoxActiveProcesses.Items)
			{
				if (!processNames.Contains(displayedName))
				{
					toRemoveList.Add(displayedName);
				}
			}

			foreach( var displayedName in toRemoveList)
			{
				listBoxActiveProcesses.Items.Remove(displayedName);
			}
		}



	}
}
