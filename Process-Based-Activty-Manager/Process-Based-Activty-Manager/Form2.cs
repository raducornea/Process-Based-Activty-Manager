using ActivityTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Process_Based_Activty_Manager
{
    public partial class FormProcessDetails : Form
    {

        public Graphics timeSlotDisplayer;
        public Graphics graph;
        public Pen pen = new Pen(Color.Black, 1);
        public Bitmap surface;

        List<Timeslot> _timeslots;

        const int xSize = 500;
        const int ySize = 60;

        public FormProcessDetails(List<Timeslot> timeslots)
        {
            InitializeComponent();

            _timeslots = timeslots;

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

            graph = Graphics.FromImage(surface);

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
                graph.DrawLine(pen, pointX1, pointX2);

            }

            List <Timeslot> list = new List<Timeslot>();

            insertProcess(_timeslots);

        }


        private void insertProcess(List<Timeslot> times)
        {

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
                   
                    graph.FillPolygon(blueBrush, points);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
