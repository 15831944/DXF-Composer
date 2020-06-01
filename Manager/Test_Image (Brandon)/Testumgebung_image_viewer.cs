using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsPotrace;
using Image_Symbol_Management;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Test_Image
{
    public partial class Testumgebung_image_viewer : Form
    {


        ArrayList ListOfCurveArray;
        Bitmap image;
        public Testumgebung_image_viewer(ArrayList listOfCurveArray, Bitmap bild)
        {

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            ListOfCurveArray = listOfCurveArray;
            image = bild;
            befuellen_CheckBox.Checked = true;
            tracing_checkBox.Checked = true;
            punkte_anzeigen_checkbox.Checked = true;
            originalImage_pictBox.Image = bild;
          Draw();
        }

        /// <summary>
        /// Zeichnet das erzeugte Vektorbild auf dem PictureBox
        /// </summary>
      void Draw()
        {
            if (ListOfCurveArray == null) return;
            Bitmap neuImage = new Bitmap(this.tableLayoutPanel1.Width / 2, this.tableLayoutPanel1.Height / 2);
            vectorizedImage_pictBox.Image = neuImage;
            Graphics neueZeichnung = Graphics.FromImage(neuImage);
            GraphicsPath neuerPfad = new GraphicsPath();
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                GraphicsPath Contour = null;
                GraphicsPath Hole = null;
                GraphicsPath Current = null;
                for (int j = 0; j < CurveArray.Count; j++)
                {
                    if (j == 0)
                    {
                        Contour = new GraphicsPath();
                        Current = Contour;
                    }
                    else
                    {
                        Hole = new GraphicsPath();
                        Current = Hole;
                    }
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];
                    for (int k = 0; k < Curves.Length; k++)
                    {
                        if (Curves[k].Kind == Potrace.CurveKind.Bezier)
                            Current.AddBezier((float)Curves[k].A.X, (float)Curves[k].A.Y, (float)Curves[k].ControlPointA.X, (float)Curves[k].ControlPointA.Y,
                                        (float)Curves[k].ControlPointB.X, (float)Curves[k].ControlPointB.Y, (float)Curves[k].B.X, (float)Curves[k].B.Y);
                        else
                            Current.AddLine((float)Curves[k].A.X, (float)Curves[k].A.Y, (float)Curves[k].B.X, (float)Curves[k].B.Y);
                    }
                    if (j > 0)
                    {
                        Contour.AddPath(Hole, false);
                    }
                    neuerPfad.AddPath(Contour, false);
                }
            }
            if (befuellen_CheckBox.Checked) neueZeichnung.FillPath(Brushes.DarkCyan, neuerPfad);
            if (tracing_checkBox.Checked) neueZeichnung.DrawPath(Pens.Black, neuerPfad);
            if (punkte_anzeigen_checkbox.Checked) Punkte_anzeigen();
        }
        private void Punkte_anzeigen()
        {
            if (ListOfCurveArray == null) return;
            Graphics neueZeichnung = Graphics.FromImage(vectorizedImage_pictBox.Image);
            for (int i = 0; i < ListOfCurveArray.Count; i++)
            {
                ArrayList CurveArray = (ArrayList)ListOfCurveArray[i];
                for (int j = 0; j < CurveArray.Count; j++)
                {
                    Potrace.Curve[] Curves = (Potrace.Curve[])CurveArray[j];

                    for (int k = 0; k < Curves.Length; k++)
                    {
                        neueZeichnung.FillRectangle(Brushes.Red, (float)((Curves[k].A.X)), (float)((Curves[k].A.Y)), 3, 3);
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void punkte_anzeigen_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void befuellen_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void tracing_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

    }
}
