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
		private Process_Based_Activty_Manager.Form2 detailsWindow;

		public Form1()
		{
			InitializeComponent();
			detailsWindow = new Process_Based_Activty_Manager.Form2();
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
			if(detailsWindow.IsDisposed)
				detailsWindow = new Process_Based_Activty_Manager.Form2();
			detailsWindow.Text = listBox1.SelectedItem.ToString();
			detailsWindow.Show();
		}

		private void sampleTimer_Tick(object sender, EventArgs e)
		{
			if( _presenter != null) { 
				_presenter.presenterTick();

			
				foreach (var process in _presenter.ComputerProcesses)
				{
					if (!listBox1.Items.Contains(process)){
						listBox1.Items.Add(process);
					}
				}

			}
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}



	}
}
