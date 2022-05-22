using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivityTracker
{
	public partial class DetailsForm : Form
	{
		static private IPresenter _presenter;
		static private string _processName;

		private Graphics _timeSlotDisplayer;
		private Graphics _graphics;
		private Bitmap _surface;

		private Pen _linePen;
		SolidBrush _timeslotBrush;

		long _timelineSize = 300;

		private const int _xSize = 650;
		private const int _ySize = 60;

		public DetailsForm(IPresenter presenter, String processName)
		{
			InitializeComponent();

			_presenter = presenter;
			_processName = processName;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.Text = "Details for " + processName;

			drawTimer.Enabled = true;

			//graphics
			_linePen = new Pen(Color.Black, 1);
			_timeslotBrush = new SolidBrush(Color.Blue);

			initCanvas();
		}

		/// <summary>
		/// Function that cleans up the drawn canvas for refreshing the timeslots
		/// </summary>
		private void cleanCanvas()
		{
			_timeSlotDisplayer.Clear(Color.White);
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


		public void displayTimeslots(List<Timeslot> times)
		{
			DrawBackgroundLines();

			long currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); ;

			long lastDisplayedSecond = currentTime - _timelineSize;

			foreach (Timeslot time in times)
			{
				if (time.EndTime >= lastDisplayedSecond && time.Duration > 2.5)
				{
					float relativeStart = (time.StartTime - lastDisplayedSecond) * _xSize / _timelineSize;
					float relativeEnd = (time.EndTime - lastDisplayedSecond) * _xSize / _timelineSize;

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
		/// Adding the process information(name and total time) to the Details section
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DetailsForm_Load(object sender, EventArgs e)
		{
			label3.Text = _processName;
		}

		public void displayTotalTime(uint totalTime)
		{
			int hours, minutes, seconds;
			minutes = (int)totalTime / 60;
			seconds = (int)(totalTime - minutes * 60);
			hours = (int)(minutes / 60);
			minutes = (int)(minutes - hours * 60);

			label4.Text = hours.ToString() + " : " + minutes.ToString() + " : " + seconds.ToString();
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

		//Discarded

		private void pictureBox1_Click(object sender, EventArgs e) { }

		private void helpToolStripMenuItem_Click(object sender, EventArgs e) { }

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (comboBox1.Text)
			{
				case "1 minute":  // statement sequence
					_timelineSize = 60;
					break;
				case "5 minutes":
					_timelineSize = 300;
					break;
				case "30 minutes":
					_timelineSize = 1800;
					break;
				case "1 hour":
					_timelineSize = 3600;
					break;
				case "24 hours":
					_timelineSize =86400;
					break;
			}

		}
	}
}
