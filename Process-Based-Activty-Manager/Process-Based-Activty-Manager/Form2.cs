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


        private const int _xSize = 590;
        private const int _ySize = 60;

        public DetailsForm(IPresenter presenter, String processName)
        {
            InitializeComponent( );

            _presenter = presenter;
            _processName = processName;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = processName;

            drawTimer.Enabled = true;

            //graphics
            _linePen = new Pen(Color.Black, 1);
            _timeslotBrush = new SolidBrush(Color.Blue);

            initCanvas();
            var timeslots = _presenter.RequestTimeslots(processName);
            displayTimeslots(timeslots);
            var totalTime = _presenter.RequestProcessTotalTime(processName);
            displayTotalTime(totalTime);


        }

        /// <summary>
        /// Function that cleans up the drawn canvas for refreshing the timeslots
        /// </summary>
        private void cleanCanvas()
        {
            _timeSlotDisplayer.Dispose();
            _graphics.Clear(Color.Transparent);
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

            _surface = new Bitmap(timeSlotDisplay.Width, timeSlotDisplay.Height);

            _graphics = Graphics.FromImage(_surface);

            timeSlotDisplay.BackgroundImage = _surface;
            timeSlotDisplay.BackgroundImageLayout = ImageLayout.None;

           

          //List <Timeslot> list = new List<Timeslot>();
          //  insertProcess(_timeslots);

        }


        public void displayTimeslots(List<Timeslot> times)
        {

            //this is only for markers
            Point pointX1 = new Point(0, 0);
            Point pointX2 = new Point(_xSize, 0);
            for (int hourIndex = 0; hourIndex < 6; hourIndex++)
            {
                pointX1.Y += _ySize / 6;
                pointX2.Y += _ySize / 6;
                _timeSlotDisplayer.DrawLine(_linePen, pointX1, pointX2);
                _graphics.DrawLine(_linePen, pointX1, pointX2);

            }

            long currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); ;
            long timelineSize = 3000;

            long realSize = currentTime - timelineSize;

           
            foreach (Timeslot time in times)
            {
                if (time.getStartTime() >= realSize)
                {
                    int relativeStart = (int)((time.getStartTime() - realSize) * _xSize / timelineSize);
                    int relativeEnd = (int)((time.getEndTime()- realSize) * _xSize / timelineSize);

                    //MessageBox.Show(relativeStart.ToString() +" "+ x.ToString());
                    pointX1 = new Point(relativeStart, 0);
                    pointX2 = new Point(relativeEnd, 0);

                    Point[] points = { pointX1, new Point(relativeStart, _ySize), new Point(relativeEnd, _ySize), pointX2 };
                   
                    _graphics.FillPolygon(_timeslotBrush, points);
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
            label3.Text = this.Text;
        }

        public void displayTotalTime(uint totalTime)
        {
            int hours,minutes, seconds;
            minutes = (int)totalTime / 60;
            seconds = (int)(totalTime - minutes * 60);
            hours = (int)(minutes / 60);
            minutes = (int)(minutes - hours * 60);

            label4.Text = hours.ToString() +" : " + minutes.ToString() + " : " + seconds.ToString()  ;
        }

        /// <summary>
        /// Since the data is already changing while this is running there is no point of adding this in the presenter.
        /// This is only useful for refreshing the drawing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawTimer_Tick(object sender, EventArgs e)
        {
            cleanCanvas();
            var timeslots = _presenter.RequestTimeslots(this.Text);
            displayTimeslots(timeslots);

            var totalTime = _presenter.RequestProcessTotalTime(this.Text);
            displayTotalTime(totalTime);
        }

        //Discarded

        private void pictureBox1_Click(object sender, EventArgs e){}

        private void helpToolStripMenuItem_Click(object sender, EventArgs e){}
    }
}
