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
using System.Collections;

namespace ActivityTracker
{
	public partial class Form1 : Form
	{

		static private IPresenter _presenter;
		private Process_Based_Activty_Manager.Form2 _detailsWindow;
		private ArrayList _descendingListTemporary;
		private ArrayList _searchListTemporary;

		public Form1()
		{
			InitializeComponent();
			_detailsWindow = new Process_Based_Activty_Manager.Form2();
			_descendingListTemporary = new ArrayList();
			_searchListTemporary = new ArrayList();
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
			if (_detailsWindow.IsDisposed)
				_detailsWindow = new Process_Based_Activty_Manager.Form2();
			_detailsWindow.Text = listBox1.SelectedItem.ToString();
			_detailsWindow.Show();
		}

		private void sampleTimer_Tick(object sender, EventArgs e)
		{
			if (_presenter != null)
			{
				_presenter.presenterTick();
			}
		}

		public void updateProcessList(List<string> processNames)
		{
			// a constraint for updating the listbox when the search is active 
			if(textBox1.Text == "")
			{ 
				foreach (var process in processNames)
				{
					if (!listBox1.Items.Contains(process))
					{
						listBox1.Items.Add(process);
					}
				}
			}
            else
            {
				// if a new process coresponds with the searched item it will be added to the listbox

				_searchListTemporary.Clear();
				foreach (String o in listBox1.Items)
				{

					if(o.Contains(textBox1.Text))
						_searchListTemporary.Add(o);
				}


				listBox1.Items.Clear();


				foreach (object o in _searchListTemporary)
				{
					listBox1.Items.Add(o);
				}

				foreach (var process in processNames)
				{
					if (!listBox1.Items.Contains(process) && process.Contains(textBox1.Text))
					{
						listBox1.Items.Add(process);
					}
				}


			}
		}

		public void setPresenter(IPresenter presenter)
		{
			_presenter = presenter;
		}

		///button to sort thing in ascending order 

        private void button1_Click_1(object sender, EventArgs e)
        {
			listBox1.Sorted = true;
        }

		///button to sort the listbox in descending order - kind of not efficient memory wise but whatever
		private void button2_Click(object sender, EventArgs e)
		{

			_descendingListTemporary.Clear();


			if (listBox1.Sorted)
				listBox1.Sorted = false;
			
			
			foreach (object o in listBox1.Items)
			{
				_descendingListTemporary.Add(o);
			}
			_descendingListTemporary.Sort();
			_descendingListTemporary.Reverse();
			listBox1.Items.Clear();
			foreach (object o in _descendingListTemporary)
			{
				listBox1.Items.Add(o);
			}
		}

		private void tabPage1_Click(object sender, EventArgs e)
        {
			
		}

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }


		/// <summary>
		/// this function is called for restoring the listbox to the original state before something was searched by treating all of the processes as if they were new
		/// </summary> 
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			if (textBox1.Text == "")
				listBox1.Items.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
