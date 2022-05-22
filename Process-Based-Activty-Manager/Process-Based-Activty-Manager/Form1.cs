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
	public partial class MainForm : Form
	{

		static private IPresenter _presenter;
		private DetailsForm currentDetailsWindow;
		private ArrayList _descendingListTemporary;
		private ArrayList _searchListTemporary;

		public MainForm()
		{
			InitializeComponent();
			_descendingListTemporary = new ArrayList();
			_searchListTemporary = new ArrayList();
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
				var totalTime = _presenter.RequestProcessTotalTime(listBoxActiveProcesses.SelectedItem.ToString());
				
				currentDetailsWindow.displayTimeslots(timeslots);
				currentDetailsWindow.displayTotalTime(totalTime);

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

			if (textBox1.Text == "")
			{
				foreach (var name in processNames)
				{
					if (!listBoxActiveProcesses.Items.Contains(name))
					{
						listBoxActiveProcesses.Items.Add(name);
					}
				}
			}
			else
			{
				_searchListTemporary.Clear();
				foreach(String s in listBoxActiveProcesses.Items)
				{
					if(s.ToUpper().Contains(textBox1.Text.ToUpper()))
						_searchListTemporary.Add(s);
				}

				listBoxActiveProcesses.Items.Clear();

				foreach (String s in _searchListTemporary)
					listBoxActiveProcesses.Items.Add(s);

				//in case a new process appeared while the search is made
				foreach (var name in processNames)
				{
					if (!listBoxActiveProcesses.Items.Contains(name) && name.ToUpper().Contains(textBox1.Text.ToUpper()))
					{
						listBoxActiveProcesses.Items.Add(name);
					}
				}
			}

			// this does not break the search functionality

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

		/// <summary>
		/// Function that adds all of the processes ever used to the list 
		/// </summary>
		/// <param name="allProcessNames"></param>
		public void AddAllProcessesToList(List<string> allProcessNames)
		{
			if (textBox2.Text == "")
			{
				foreach (var name in allProcessNames)
				{
					if (!listBoxAllProcesses.Items.Contains(name))
					{
						listBoxAllProcesses.Items.Add(name);
					}
				}
			}
			else
			{
				_searchListTemporary.Clear();
				foreach (String s in listBoxAllProcesses.Items)
				{
					if (s.ToUpper().Contains(textBox2.Text.ToUpper()))
						_searchListTemporary.Add(s);
				}

				listBoxAllProcesses.Items.Clear();

				foreach (String s in _searchListTemporary)
					listBoxAllProcesses.Items.Add(s);

				//in case a new process appeared while the search is made
				foreach (var name in allProcessNames)
				{
					if (!listBoxAllProcesses.Items.Contains(name) && name.ToUpper().Contains(textBox2.Text.ToUpper()))
					{
						listBoxAllProcesses.Items.Add(name);
					}
				}
			}

			// this does not break the search functionality

			List<object> toRemoveList = new List<object>();

			foreach (var displayedName in listBoxActiveProcesses.Items)
			{
				if (!allProcessNames.Contains(displayedName))
				{
					toRemoveList.Add(displayedName);
				}
			}

			foreach (var displayedName in toRemoveList)
			{
				listBoxActiveProcesses.Items.Remove(displayedName);
			}




		}






			/// <summary>
			/// Function that enables the sorted attribute of the listbox of processes, thus sorting it ascending by name 
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			private void button1_Click(object sender, EventArgs e)
		{
			listBoxActiveProcesses.Sorted = true;
		}

		/// <summary>
		/// Function that uses a secondary ArrayList in order to sort the process listbox descending by name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			_descendingListTemporary.Clear();

			if (listBoxActiveProcesses.Sorted)
				listBoxActiveProcesses.Sorted = false;

			foreach (object o in listBoxActiveProcesses.Items)
				_descendingListTemporary.Add(o);
			listBoxActiveProcesses.Items.Clear();

			_descendingListTemporary.Sort();
			_descendingListTemporary.Reverse();

			foreach (object o in _descendingListTemporary)
				listBoxActiveProcesses.Items.Add(o);

		}

		/// <summary>
		/// Function that sorts the list of all processes ascending
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, EventArgs e)
		{
			listBoxAllProcesses.Sorted = true;
		}

		/// <summary>
		/// Function that sorts the list of all processes descending
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{

			_descendingListTemporary.Clear();

			if (listBoxAllProcesses.Sorted)
				listBoxAllProcesses.Sorted = false;

			foreach (object o in listBoxAllProcesses.Items)
				_descendingListTemporary.Add(o);
			listBoxAllProcesses.Items.Clear();

			_descendingListTemporary.Sort();
			_descendingListTemporary.Reverse();

			foreach (object o in _descendingListTemporary)
				listBoxAllProcesses.Items.Add(o);
		}


		/// <summary>
		/// Function that clears the listbox of processes when the searched word is empty, thus making the program restore the list to its original state
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (textBox1.Text == "")
				listBoxActiveProcesses.Items.Clear();
		}

		/// <summary>
		/// Similar to the textbox1, textbox2 needs to clear the search reasult list when the text returns to nothing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
			if (textBox2.Text == "")
				listBoxActiveProcesses.Items.Clear();
		}

    }
}
