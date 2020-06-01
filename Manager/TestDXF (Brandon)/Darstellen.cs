using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using netDxf;
using netDxf.Entities;


namespace TestDXF
{
    public partial class Darstellen : Form
    {
        List<LwPolyline> list_LWP;
        Graphics Graph;
        public Darstellen(List<LwPolyline> List_LWP)
        {
            list_LWP = List_LWP;
            InitializeComponent();
            Graph = this.CreateGraphics();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Maximized;
        }

        private void Darstellen_Paint(object sender, PaintEventArgs e)
        {
            Drawer(false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Drawer(checkBox1.Checked);
        }
        void Drawer(bool Checked)
        {
            Pen stift = new Pen(Color.Black, 1);
  
            Graph.Clear(this.BackColor);
            if (!Checked)
            {
                foreach (LwPolyline poly in list_LWP)
                {
                    List<System.Drawing.PointF> listPts = new List<System.Drawing.PointF>();
                    foreach (LwPolylineVertex polyV in poly.Vertexes)
                    {
                        listPts.Add(new System.Drawing.PointF((float)polyV.Location.X * 2, (float)polyV.Location.Y * 2));
                    }

                    Graph.DrawPolygon(stift, listPts.ToArray());
                }
            }
            else
            {
                foreach (LwPolyline poly in list_LWP)
                {

                    foreach (LwPolylineVertex polyV in poly.Vertexes)
                    {
                        Rectangle rect = new Rectangle(new System.Drawing.Point((int)polyV.Location.X*2, (int)polyV.Location.Y*2), new Size(4, 4));

                        Graph.FillRectangle(new SolidBrush(Color.Red), rect);

                    }


                }
            }

        }
    }
}
