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
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Process[] processlist = Process.GetProcesses();

			foreach (Process theprocess in processlist)
			{
				Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);

				
			}
		}
	}
}
