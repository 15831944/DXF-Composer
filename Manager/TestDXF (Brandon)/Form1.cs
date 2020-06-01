using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DXF_Management;
using netDxf;
using System.IO;
using ElementManager;

namespace TestDXF
{
    public partial class Form1 : Form
    {
        DXF_Management.DXF_Reader file;
        DXF_Management.DXF_Writer file2;
        int zaehler = 0;
        Dictionary<int, netDxf.Entities.LwPolyline> allPolylines = new Dictionary<int, netDxf.Entities.LwPolyline>();
        DxfDocument loadedDXF;
        string Pfad;
        public Form1()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            contextMenuStrip1.Enabled = false;
            label2.Visible = false;
            //  System.Threading.Thread.Sleep(3000);
            label2.Text = "Klicken Sie recht mit der Maus \n in der Nähe von Height, \n um Arbeitseigenschaften \n hinzuzufügen oder zu ändern ";

        }


        private void loadDXF_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "DXF Dateien (*.dxf)|*.dxf";
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                Pfad = openFD.FileName;
                pfad_TxtBox.Text = Pfad;
                label2.Visible = true;
                zaehler = 0;
                zaehler+=1;


            }
        }

        private void pfad_TxtBox_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(pfad_TxtBox.Text))
            {
                darstellungEinsehen_btn.Enabled = true;
                file = new DXF_Reader(pfad_TxtBox.Text);
                loadedDXF = file.loaded_DXF;
                polyline_listBox.Enabled = true;
                punkte_listBox.Enabled = true;
                Update(false);

            }
            else
            {
                darstellungEinsehen_btn.Enabled = false;
                polyline_listBox.Enabled = false;
                punkte_listBox.Enabled = false;
                Update(true);
            }

        }
        private void Update(bool Pfad_leer)
        {
            if (zaehler >= 3 )
            {
                label2.Visible = false;
            }
            if (!Pfad_leer)
            {
                polyline_listBox.Items.Clear();
                punkte_listBox.Items.Clear();
                allPolylines.Clear();
                netDxf.Entities.Point newPt;
                if (loadedDXF.Points.Count > 0)
                {
                    newPt = loadedDXF.Points.Last();
                }
                else
                {
                    newPt = new netDxf.Entities.Point(new Vector2(0, 0));
                }


                Height_lbl.Text = "Height : " + newPt.Location.X;
                width_lbl.Text = "Width : " + newPt.Location.Y;
                depth_lbl.Text = "Depth : " + newPt.Location.Z;
                try
                {
                    werkzeug_lbl.Text = "Werkzeug : " + loadedDXF.LwPolylines.Last().Layer.Name;
                }
                catch
                { }
                    for (int i = 0; i < loadedDXF.LwPolylines.Count; i++)
                    {
                        allPolylines.Add(i, loadedDXF.LwPolylines[i]);
                        int t = i + 1;
                        polyline_listBox.Items.Add("Polyline " + t);
                    }
                }
            else
            {
                    polyline_listBox.Items.Clear();
                    punkte_listBox.Items.Clear();
                    Height_lbl.Text = "Height : ";
                    width_lbl.Text = "Width : ";
                    depth_lbl.Text = "Depth : ";
                    werkzeug_lbl.Text = "Werkzeug : ";
                }
            zaehler+=1;
            }


        private void polyline_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (zaehler>=3)
            {
                label2.Visible = false;
            }

            int selectedIndex = polyline_listBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                punkte_listBox.Items.Clear();
                foreach (netDxf.Entities.LwPolylineVertex _point in allPolylines[selectedIndex].Vertexes)
                {
                    punkte_listBox.Items.Add("X = " + _point.Location.X.ToString() + " Y = " + _point.Location.Y.ToString());
                    werkzeug_lbl.Text = "Werkzeug : " + allPolylines[selectedIndex].Layer.Name;
                }
            }
            zaehler += 1;
        }

        private void darstellungEinsehen_btn_Click(object sender, EventArgs e)
        {
            Darstellen neueZeichnung = new Darstellen(allPolylines.Values.ToList());
            neueZeichnung.ShowDialog();
        }



        private void arbeitseigenschaftenÄndernToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (Height_lbl.Text == "Height : 0")
            {
                contextMenuStrip1.Visible = false;
                contextMenuStrip1.Enabled = false;
                return;
            }
            Arbeitseigenschaften neuAE = new Arbeitseigenschaften(false, pfad_TxtBox.Text, loadedDXF);
            neuAE.ShowDialog();
            pfad_TxtBox.Text = pfad_TxtBox.Text;
            contextMenuStrip1.Visible = false;
            contextMenuStrip1.Enabled = false;
            Update(false);


        }

        private void arbeitseigenschaftenHinzufügenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            if (Height_lbl.Text != "Height : 0")
            {
                contextMenuStrip1.Visible = false;
                contextMenuStrip1.Enabled = false;
                return;
            }

            Arbeitseigenschaften neuAE = new Arbeitseigenschaften(true, pfad_TxtBox.Text, loadedDXF);
            neuAE.ShowDialog();
            pfad_TxtBox.Text = pfad_TxtBox.Text;
            contextMenuStrip1.Visible = false;
            contextMenuStrip1.Enabled = false;
            Update(false);

        }

        private void annulierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Visible = false;
            contextMenuStrip1.Enabled = false;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !String.IsNullOrEmpty(pfad_TxtBox.Text))
            {
                contextMenuStrip1.Show(Cursor.Position);
                contextMenuStrip1.Visible = true;
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                //contextMenuStrip1.Show(Cursor.Position);
                contextMenuStrip1.Visible = false;
                contextMenuStrip1.Enabled = false;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Visible = false;
            contextMenuStrip1.Enabled = false;
        }

       /* private void button1_Click(object sender, EventArgs e)
        {
            //save
            string path = @"D:\Dokumente\_nach dem Abi\HTW Berlin\Programmierprojekt\testdateiErstelltDurchTestDXF.dxf";
            workpiece neuWorkpiece = new workpiece();
            neuWorkpiece.depth = 0;
            neuWorkpiece.height = 0;
            neuWorkpiece.width = 0;
            neuWorkpiece.werkzeugName = "Stift";


            DXF_Writer.JoinToDxF(path, ref loadedDXF, neuWorkpiece);

        }*/
    }
}
