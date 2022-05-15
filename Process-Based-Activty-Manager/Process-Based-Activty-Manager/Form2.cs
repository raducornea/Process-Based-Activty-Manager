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
    public partial class Form2 : Form
    {

        public Graphics timeSlotDisplayer;
        public Graphics graph;
        public Pen pen = new Pen(Color.Black, 1);
        Bitmap surface;

        public Form2()
        {
            InitializeComponent();

            //initializing the graphic display
           
            //200 is the amount we go back
            initCanvas(500,60);

        }


        //resizing the information for what we need
        private void initCanvas(int x ,int y)
        {


            timeSlotDisplay.Width = x;
            timeSlotDisplay.Height = y;


            timeSlotDisplayer = timeSlotDisplay.CreateGraphics();

            timeSlotDisplayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);

            surface = new Bitmap(timeSlotDisplay.Width, timeSlotDisplay.Height);

            graph = Graphics.FromImage(surface);

            timeSlotDisplay.BackgroundImage = surface;
            timeSlotDisplay.BackgroundImageLayout = ImageLayout.None;


            //this is only for markers
            Point pointX1 = new Point(0,0);
            Point pointX2 = new Point(x, 0);
            for(int hourIndex = 0; hourIndex< 6; hourIndex++)
            {
                pointX1.Y += y/6;
                pointX2.Y += y/6;
                timeSlotDisplayer.DrawLine(pen, pointX1, pointX2);
                graph.DrawLine(pen, pointX1, pointX2);

            }

            Timeslot ceva1 = new Timeslot(100, 150, "ceva", "acolo");
            Timeslot ceva2 = new Timeslot(200, 2000, "ceva", "acolo");
            Timeslot ceva3 = new Timeslot(7500, 8000, "ceva", "acolo");
            Timeslot ceva4 = new Timeslot(1000, 2400, "ceva", "acolo");

            List <Timeslot> list = new List<Timeslot>();
            list.Add(ceva1);
            list.Add(ceva2);
            list.Add(ceva3);
            list.Add(ceva4);


            insertProcess(list, x, y);

        }


        private void insertProcess(List<Timeslot> times, int x, int y)
        {

            int currentTime = 10000;
            int backTime = 5000;

            int realSize = currentTime - backTime;
            pen.Color = Color.Blue;
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            pen.Width = 5;
            foreach (Timeslot time in times)
            {


                if (time.getStartTime() >= backTime)
                {
                    int relativeStart = (int)((time.getStartTime() - backTime) * x / realSize);
                    int relativeEnd = (int)((time.getEndTime()-backTime) * x / realSize);

                    //MessageBox.Show(relativeStart.ToString() +" "+ x.ToString());
                    Point pointX1 = new Point(relativeStart, 0);
                    Point pointX2 = new Point(relativeEnd, 0);

                    Point[] points = { pointX1, new Point(relativeStart, y), new Point(relativeEnd, y), pointX2 };
                   
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
