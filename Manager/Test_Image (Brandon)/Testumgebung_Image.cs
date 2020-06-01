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
using System.Collections;
using System.Drawing.Drawing2D;
using Image_Symbol_Management;
using System.IO;
using System.Resources;
using Ent = netDxf.Entities;


namespace Test_Image
{
    public partial class Testumgebung_Image : Form
    {

        Image_Class newElement;
        Image loadedImge;
        string pfad,pfad2;
        Dictionary<int, netDxf.Entities.LwPolyline> allPolyline_Entsprechung = new Dictionary<int, netDxf.Entities.LwPolyline>();

        public Testumgebung_Image()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            points_listBox.HorizontalScrollbar = true;
            polyLine_listBox.HorizontalScrollbar = true;
            pfad = pfad2 = Application.StartupPath;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            load_image_Button.Enabled = true;
            ruckgangig_btn.Enabled = false;
            draw_button.Enabled = false;
            this.Text = "Image Vectorizing";

        }
        private void load_image_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "JPG Datei  (*.jpg)|*.jpg|JPEG Datei (*.jpeg)|*.jpeg|PNG Datei (*.png)|*.png";
     
            file.Title = "Please select an image to vectorize.";
            DialogResult result = file.ShowDialog();
            string File_name = "";
            if (result == DialogResult.OK)
            {
                File_name = file.FileName;

            }

            pfad_textBox.Text = File_name;
            loadedImge = Image.FromFile(File_name);

            System.Reflection.Assembly aktuelleAssembly = typeof(Testumgebung_Image).Assembly;
            DateTime now = DateTime.Now;
            pfad = Path.Combine(pfad, "Test_" + now.Day.ToString() + "_" + now.Month.ToString() + "_" + now.Year + "____" + now.Hour + "_" + now.Minute + "_" + now.Second + "_" + now.Millisecond + ".dxf");
            newElement = new Image_Class(loadedImge);
            newElement.Set_and_GetDXF_File(pfad);
            for (int i = 0; i < newElement.allePolylinien.Count; i++)
            {
                allPolyline_Entsprechung.Add(i, newElement.allePolylinien[i]);
                int t = i + 1;
                polyLine_listBox.Items.Add("Polylinie " + t);
            }
            load_image_Button.Enabled = false;
            ruckgangig_btn.Enabled = true;
            draw_button.Enabled = true;

        }



        private void draw_button_Click(object sender, EventArgs e)
        {


            Testumgebung_image_viewer newForm = new Testumgebung_image_viewer(newElement.ListOfCurveArray, new Bitmap( loadedImge));

            newForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points_listBox.Items.Clear();
            polyLine_listBox.Items.Clear();

            pfad_textBox.Text = String.Empty;
            pfad = pfad2;
            draw_button.Enabled = false;
            load_image_Button.Enabled = true;
            ruckgangig_btn.Enabled = false;
            allPolyline_Entsprechung.Clear();
        }

        private void PolyLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = polyLine_listBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                points_listBox.Items.Clear();
                foreach (netDxf.Entities.LwPolylineVertex _point in allPolyline_Entsprechung[selectedIndex].Vertexes)
                {
                    points_listBox.Items.Add("X = " + _point.Location.X.ToString() + " Y = " + _point.Location.Y.ToString());
                    
                }
            }
        }
    }
}
