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
	public partial class Form1 : Form
	{

		static private IPresenter _presenter;
		private Process_Based_Activty_Manager.FormProcessDetails detailsWindow;

		public Form1()
		{
			InitializeComponent();
			//detailsWindow = new Process_Based_Activty_Manager.FormProcessDetails();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Process[] processlist = Process.GetProcesses();

			foreach (Process theprocess in processlist)
			{
				Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		// when a process is clicked a new window of type Form2 is opened for displaying information about the process
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		
			if (listBox1.SelectedItem != null) { 
				var timeslots = _presenter.RequestTimeslots(listBox1.SelectedItem.ToString());

				detailsWindow = new Process_Based_Activty_Manager.FormProcessDetails(timeslots);

				detailsWindow.Text = listBox1.SelectedItem.ToString();
				detailsWindow.Show();
			}
		}

		private void sampleTimer_Tick(object sender, EventArgs e)
		{
			if (_presenter != null)
			{
				_presenter.presenterTick();
			}
		}

		public void AddProcessToList(List<string> processNames)
		{
			foreach(var name in processNames)
			{
				if (!listBox1.Items.Contains(name))
				{
					listBox1.Items.Add(name);
				}
			}


			List<object> toRemoveList = new List<object>();

			foreach (var displayedName in listBox1.Items)
			{
				if (!processNames.Contains(displayedName))
				{
					toRemoveList.Add(displayedName);
				}
			}

			foreach( var displayedName in toRemoveList)
			{
				listBox1.Items.Remove(displayedName);
			}
		}


		public void setPresenter(IPresenter presenter)
		{
		
			_presenter = presenter;
		}



	}
}
