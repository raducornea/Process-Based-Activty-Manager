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
        public void setPresenter(IPresenter presenter)
        {
            _presenter = presenter;
        }

        public Graphics timeSlotDisplayer;
        public Graphics graphics;
        public Pen pen = new Pen(Color.Black, 1);
        public Bitmap surface;

        const int xSize = 900;
        const int ySize = 60;

        public DetailsForm()
        {
            InitializeComponent();
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
    }
}
