/*******************************************************************************
 *                                                                             *
 *  File:        Form1.cs                                                      *
 *  Copyright:   (c) 2022, Atomulesei Paul-Costin                              *
 *  E-mail:      paul-costin.atomulesei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Form for showing list of processes                            *
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
using System.Windows.Forms;
using System.Collections;

namespace ActivityTracker
{
    public partial class MainForm : Form
    {
        static private IPresenter _presenter;
        private DetailsForm _currentDetailsWindow;
        private ArrayList _descendingListTemporary;
        private ArrayList _searchListTemporary;

        /// <summary>
        /// Constructor that creates the main window object
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            _descendingListTemporary = new ArrayList();
            _searchListTemporary = new ArrayList();
        }

        public void SetPresenter(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// The closing function for the form. It has the option for minimization in the system tray if the user wills it
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            string message = "Are you sure you want to close?\n"
                            +"You can minimize the application and it will stay in your system tray.";
            switch (MessageBox.Show(this, message, "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Function that opens the about section of the ToolStripMenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //  throw new Exception("Not implemented yet");
        }

        /// <summary>
        /// Function that opens the help section of the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("helper.chm");
        }

        /// <summary>
        /// Function that opens the clean database section of the ToolStripMenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cleanDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.DeleteDatabase();
        }

        /// <summary>
        /// Function that minimizes the process of the Activity Manager App to the system tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // if the form is minimized,
            // hide it from the task bar  
            // and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                trayIcon.Visible = true;
            }
        }

        /// <summary>
        /// A timer function, where the recurent logic of this aplication is ran
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sampleTimer_Tick(object sender, EventArgs e)
        {
            if (_presenter != null)
            {
                _presenter.presenterTick();
            }
        }

        /// <summary>
        /// Function for bringing the main window of the program back into user view from the system tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }

        /// <summary>
        /// Function that updates the acive process list in the listbox. 
        /// It also adds the search functionality into it by using a temporary search list to add the results of the search
        /// This function is called every tick for updated information
        /// </summary>
        /// <param name="processNames"></param>
        public void UpdateActiveProcessesList(List<string> processNames)
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
                foreach (String s in listBoxActiveProcesses.Items)
                {
                    if (s.ToUpper().Contains(textBox1.Text.ToUpper()))
                    {
                        _searchListTemporary.Add(s);
                    }
                }

                listBoxActiveProcesses.Items.Clear();

                foreach (String s in _searchListTemporary)
                {
                    listBoxActiveProcesses.Items.Add(s);
                }

                // in case a new process appeared while the search is made
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

            foreach (var displayedName in toRemoveList)
            {
                listBoxActiveProcesses.Items.Remove(displayedName);
            }
        }

        /// <summary>
        /// Function that opens a new details window for a selected process from the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxActiveProcesses.SelectedItem != null)
            {
                // We close the old window
                if (_currentDetailsWindow != null)
                {
                    _currentDetailsWindow.Close();
                }

                // And open a new updated window
                _currentDetailsWindow = new DetailsForm(_presenter, listBoxActiveProcesses.SelectedItem.ToString());
                _currentDetailsWindow.Show();
            }
        }

        // ALL PROCESSES LIST
        /// <summary>
        /// Function that adds all of the processes ever used to the list 
        /// The search functionality of the form is also included here
        /// After the search is complete the original list is restored by using the tick refresh and the if bracket of the case with null string
        /// </summary>
        /// <param name="allProcessNames"></param>
        public void UpdateAllProcessesList(List<string> allProcessNames)
        {
            if (textBoxAllProcesses.Text == "")
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
                    if (s.ToUpper().Contains(textBoxAllProcesses.Text.ToUpper()))
                        _searchListTemporary.Add(s);
                }

                listBoxAllProcesses.Items.Clear();

                foreach (String s in _searchListTemporary)
                {
                    listBoxAllProcesses.Items.Add(s);
                }

                //in case a new process appeared while the search is made
                foreach (var name in allProcessNames)
                {
                    if (!listBoxAllProcesses.Items.Contains(name) && name.ToUpper().Contains(textBoxAllProcesses.Text.ToUpper()))
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
        /// Function that opens a new details window for a selected process from the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxAllProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAllProcesses.SelectedItem != null)
            {
                //We close the old window

                if (_currentDetailsWindow != null)
                {
                    _currentDetailsWindow.Close();
                }
                //And open a new updated window
                _currentDetailsWindow = new DetailsForm(_presenter, listBoxAllProcesses.SelectedItem.ToString());

                //currentDetailsWindow.Text = ;
                _currentDetailsWindow.Show();
            }
        }

        //BUTTONS
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

            foreach (object listBoxItem in listBoxActiveProcesses.Items)
            {
                _descendingListTemporary.Add(listBoxItem);
            }
            listBoxActiveProcesses.Items.Clear();

            _descendingListTemporary.Sort();
            _descendingListTemporary.Reverse();

            foreach (object listItem in _descendingListTemporary)
            {
                listBoxActiveProcesses.Items.Add(listItem);
            }
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
            {
                listBoxAllProcesses.Sorted = false;
            }
            foreach (object listBoxItem in listBoxAllProcesses.Items)
            {
                _descendingListTemporary.Add(listBoxItem);
            }

            listBoxAllProcesses.Items.Clear();
            _descendingListTemporary.Sort();
            _descendingListTemporary.Reverse();

            foreach (object listItem in _descendingListTemporary)
            {
                listBoxAllProcesses.Items.Add(listItem);
            }
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

        //SEARCH BOXES
        /// <summary>
        /// Function that clears the listbox of processes when the searched word is empty, thus making the program restore the list to its original state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                listBoxActiveProcesses.Items.Clear();
            }
        }

        /// <summary>
        /// Similar to the textbox1, textbox2 needs to clear the search reasult list when the text returns to nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBoxAllProcesses.Text == "")
            {
                listBoxActiveProcesses.Items.Clear();
            }
        }
	}
}
