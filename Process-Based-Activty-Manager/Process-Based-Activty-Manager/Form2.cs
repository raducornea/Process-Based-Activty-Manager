/*******************************************************************************
 *                                                                             *
 *  File:        Form2.cs                                                      *
 *  Copyright:   (c) 2022, Atomulesei Paul-Costin                              *
 *  E-mail:      paul-costin.atomulesei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Form for showing details for a specific process               *
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.ViewManagement;

namespace ActivityTracker
{
	public partial class DetailsForm : Form
	{
		private static IPresenter _presenter;
		private static string _processName;

		private long _timelineSize = 300;
		private const int _xSize = 660;
		private const int _ySize = 60;

		private Graphics _timeSlotDisplayer;
		private Pen _linePen;
		private SolidBrush _timeslotBrush;

		/// <summary>
		/// Constructor that takes a presenter and a process name
		/// It creates the window object and the class members that will be used to draw the time slot
		/// </summary>
		/// <param name="presenter"></param>
		/// <param name="processName"></param>
		public DetailsForm(IPresenter presenter, String processName)
		{
			InitializeComponent();

			_presenter = presenter;
			_processName = processName;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.Text = "Details for " + processName;


			var uiSettings = new UISettings();
			var accentColor = uiSettings.GetColorValue(UIColorType.Accent);

			Color brushColor = Color.FromArgb(255, accentColor.R, accentColor.G, accentColor.B);
			//graphics

			_linePen = new Pen(Color.Black, 1);
			_timeslotBrush = new SolidBrush(brushColor);

			initCanvas();
			//This must be enabled at the end.
			drawTimer.Enabled = true;
		}

		/// <summary>
		/// Function that displays the time slots using a graphics object.
		/// It converts the timeslot from an interval of time in seconds to an interval of pixels 
		/// Also there is a call to the DrawBackgroundLines() function for initializing the lines on which the timeslots are drawn
		/// </summary>
		/// <param name="times"></param>
		public void displayTimeslots(List<Timeslot> times)
		{
			DrawBackgroundLines();

			long currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); ;

			long lastDisplayedSecond = currentTime - _timelineSize;
			int minimalDisplayedDuration = (int)Math.Ceiling((double)_timelineSize / _xSize);

			foreach (Timeslot time in times)
			{
				if (time.EndTime >= lastDisplayedSecond)
				{
					float relativeStart;
					float relativeEnd;

					if (time.Duration < minimalDisplayedDuration)
					{
						relativeStart = (time.StartTime - lastDisplayedSecond) * _xSize / _timelineSize;
						relativeEnd = (time.StartTime + minimalDisplayedDuration - lastDisplayedSecond) * _xSize / _timelineSize;
					}
					else
					{
						relativeStart = (time.StartTime - lastDisplayedSecond) * _xSize / _timelineSize;
						relativeEnd = (time.EndTime - lastDisplayedSecond) * _xSize / _timelineSize;
					}

					int pixelStart = (int)Math.Floor(relativeStart);
					int pixelEnd = (int)Math.Floor(relativeEnd);

					//MessageBox.Show(relativeStart.ToString() +" "+ x.ToString());
					Point pointX1Top = new Point(pixelStart, 0);
					Point pointX2Top = new Point(pixelEnd, 0);

					Point pointX1Bottom = new Point(pixelStart, _ySize);
					Point pointX2Bottom = new Point(pixelEnd, _ySize);

					Point[] points = { pointX1Top, pointX1Bottom, pointX2Bottom, pointX2Top };

					_timeSlotDisplayer.FillPolygon(_timeslotBrush, points);
				}
			}
		}

		/// <summary>
		/// Function that displays the total time a process has been active on a label from the UI
		/// Some calculus is required for having an exact conversion from the seconds stored in the database
		/// </summary>
		/// <param name="totalTime"></param>
		public void displayTotalTime(uint totalTime)
		{
			int hours, minutes, seconds;

			minutes = (int)totalTime / 60;
			seconds = (int)(totalTime - minutes * 60);
			hours = (int)(minutes / 60);
			minutes = (int)(minutes - hours * 60);

			label4.Text = hours.ToString() + " : " +
					 (minutes <= 9 ? "0" : "") + minutes.ToString() + " : " +
					 (seconds <= 9 ? "0" : "") + seconds.ToString();
		}

		/// <summary>
		/// Function that cleans up the drawn canvas for refreshing the timeslots
		/// </summary>
		private void cleanCanvas()
		{
			_timeSlotDisplayer.Clear(Color.FromArgb(255,248,244,244));
		}

		/// <summary>
		/// Function that initializes an empty canvas
		/// </summary>
		private void initCanvas()
		{
			timeSlotDisplay.Width = _xSize;
			timeSlotDisplay.Height = _ySize;

			_timeSlotDisplayer = timeSlotDisplay.CreateGraphics();
			_timeSlotDisplayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			_linePen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
		}

		/// <summary>
		/// Function that draws lines on the graphics object, relative to its size
		/// </summary>
		private void DrawBackgroundLines()
		{
			//this is only for markers
			Point pointX1 = new Point(0, 0);
			Point pointX2 = new Point(_xSize, 0);
			for (int hourIndex = 0; hourIndex < 6; hourIndex++)
			{
				pointX1.Y += _ySize / 6;
				pointX2.Y += _ySize / 6;
				_timeSlotDisplayer.DrawLine(_linePen, pointX1, pointX2);
			}
		}

		/// <summary>
		/// Since the data is already changing while this is running there is no point of adding this in the presenter.
		/// This is only useful for refreshing the drawing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void drawTimer_Tick(object sender, EventArgs e)
		{
			//First of all, prepare all the information
			var timeslots = _presenter.RequestTimeslots(_processName);
			var totalTime = _presenter.RequestProcessTotalTime(_processName);

			//Then you can erase the graphics and draw it fast enough
			cleanCanvas();
			displayTimeslots(timeslots);
			displayTotalTime(totalTime);
		}

		//UI

		/// <summary>
		/// Adding the process information(name and total time) to the Details section
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DetailsForm_Load(object sender, EventArgs e)
		{
			label3.Text = _processName;
		}

		/// <summary>
		/// Function that changes the timeline size via user input in a combo box
		/// The result is changing the range of time that the user can see in the timeslot window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (comboBox1.Text)
			{
				case "1 minute":  // statement sequence
					_timelineSize = 60;
					labelPastLimit.Text = "1 minute ago";
					break;
				case "5 minutes":
					_timelineSize = 300;
					labelPastLimit.Text = "5 minutes ago";
					break;
				case "30 minutes":
					_timelineSize = 1800;
					labelPastLimit.Text = "30 minute ago";
					break;
				case "1 hour":
					_timelineSize = 3600;
					labelPastLimit.Text = "1 hour ago";
					break;
				case "24 hours":
					_timelineSize = 86400;
					labelPastLimit.Text = "24 hours ago";
					break;
			}

		}


		// Discarded Functions
		private void pictureBox1_Click(object sender, EventArgs e) { }

		private void helpToolStripMenuItem_Click(object sender, EventArgs e) { }
	}
}
