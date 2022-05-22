﻿using System;
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

        public Graphics timeSlotDisplayer;
        public Graphics graphics;
        public Pen pen = new Pen(Color.Black, 1);
        public Bitmap surface;

        const int xSize = 590;
        const int ySize = 60;

        public DetailsForm(IPresenter presenter, String processName)
        {
            InitializeComponent( );

            _presenter = presenter;
            _processName = processName;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = processName;

            drawTimer.Enabled = true;

            initCanvas();
        }


        //resizing the information for what we need
        private void initCanvas()
        {
            timeSlotDisplay.Width = xSize;
            timeSlotDisplay.Height = ySize;


            timeSlotDisplayer = timeSlotDisplay.CreateGraphics();

            timeSlotDisplayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);

            surface = new Bitmap(timeSlotDisplay.Width, timeSlotDisplay.Height);

            graphics = Graphics.FromImage(surface);

            timeSlotDisplay.BackgroundImage = surface;
            timeSlotDisplay.BackgroundImageLayout = ImageLayout.None;

            //this is only for markers
            Point pointX1 = new Point(0,0);
            Point pointX2 = new Point(xSize, 0);
            for(int hourIndex = 0; hourIndex< 6; hourIndex++)
            {
                pointX1.Y += ySize / 6;
                pointX2.Y += ySize / 6;
                timeSlotDisplayer.DrawLine(pen, pointX1, pointX2);
                graphics.DrawLine(pen, pointX1, pointX2);

            }

            List <Timeslot> list = new List<Timeslot>();
          //  insertProcess(_timeslots);

        }


        public void displayTimeslots(List<Timeslot> times)
        {
             //graphics.Clear(Color.Transparent);

            long currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); ;
            long timelineSize = 3000;

            long realSize = currentTime - timelineSize;

            pen.Color = Color.Blue;
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            pen.Width = 5;

            foreach (Timeslot time in times)
            {
                if (time.getStartTime() >= realSize)
                {
                    int relativeStart = (int)((time.getStartTime() - realSize) * xSize / timelineSize);
                    int relativeEnd = (int)((time.getEndTime()- realSize) * xSize / timelineSize);

                    //MessageBox.Show(relativeStart.ToString() +" "+ x.ToString());
                    Point pointX1 = new Point(relativeStart, 0);
                    Point pointX2 = new Point(relativeEnd, 0);

                    Point[] points = { pointX1, new Point(relativeStart, ySize), new Point(relativeEnd, ySize), pointX2 };
                   
                    graphics.FillPolygon(blueBrush, points);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            initCanvas();
            var timeslots = _presenter.RequestTimeslots(this.Text);
            displayTimeslots(timeslots);

            var totalTime = _presenter.RequestProcessTotalTime(this.Text);
            displayTotalTime(totalTime);
        }
    }
}
