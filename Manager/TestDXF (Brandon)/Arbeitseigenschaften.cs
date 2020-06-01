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
using ElementManager;

namespace TestDXF
{
    public partial class Arbeitseigenschaften : Form
    {
        bool Adding;
        DxfDocument Doc;
        string werkzeug_alt, werkzeug_neu,PFAD;
        netDxf.Entities.Point newPt, altPt;
        int height_alt, height_neu, width_alt, width_neu, depth_alt, depth_neu;

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.NumPad0 && e.KeyCode != Keys.NumPad1 && e.KeyCode != Keys.NumPad2 && e.KeyCode != Keys.NumPad3 && e.KeyCode != Keys.NumPad4 && e.KeyCode != Keys.NumPad5 && e.KeyCode != Keys.NumPad6 && e.KeyCode != Keys.NumPad7 && e.KeyCode != Keys.NumPad8 && e.KeyCode != Keys.NumPad9 && e.KeyCode != Keys.Back)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void Vornehmen_Click(object sender, EventArgs e)
        {


            try
            {
                if (!String.IsNullOrEmpty(werkzeug_neu) && height_neu != 0 && width_neu != 0 && depth_neu != 0 && werkzeug_neu.Length > 4)
                {
                    newPt = new netDxf.Entities.Point(height_neu, width_neu, depth_neu);
                    List<netDxf.Entities.Point> neuList = Doc.Points.ToList();
                    workpiece neuWorkpiece = new workpiece();
                    neuWorkpiece.depth = depth_neu;
                    neuWorkpiece.height = height_neu;
                    neuWorkpiece.width = width_neu;
                    neuWorkpiece.werkzeugName = werkzeug_neu;
                    foreach (netDxf.Entities.Point item in neuList)
                    {

                        Doc.RemoveEntity(item);
                    }



                    DXF_Management.DXF_Writer.JoinToDxF(PFAD, ref Doc, neuWorkpiece);
                    MessageBox.Show("Erfolgreich vorgenommen!", "Arbeitseigenschaften Veränderung", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show("Falsche Eingabe \n Bitte überprüfen Sie Ihre Eingaben!", "Arbeitseigenschaften Veränderung", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Arbeitseigenschaften Veränderung", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            }
        

        public Arbeitseigenschaften(bool adding,string pfad,DxfDocument doc)
        {
            InitializeComponent();
            Adding = adding;
            Doc = doc;
            PFAD = pfad;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            if (!Adding)
            {
                werkzeug_txtBox.Text = doc.LwPolylines.Last().Layer.Name;
                 altPt = doc.Points.Last();
                height_txtBox.Text =  altPt.Location.X.ToString();
                width_txtBox.Text = altPt.Location.Y.ToString();
                depth_txtBox.Text =altPt.Location.Z.ToString();
            }
            else
            {
                werkzeug_txtBox.Text = "NA";
              
                height_txtBox.Text = "0";
                width_txtBox.Text = "0";
                depth_txtBox.Text = "0";
            }

        }

        private void werkzeug_txtBox_TextChanged(object sender, EventArgs e)
        {
            werkzeug_neu = werkzeug_txtBox.Text;
        }

        private void height_txtBox_TextChanged(object sender, EventArgs e)
        {

        
             height_neu = Convert.ToInt32(height_txtBox.Text);
         
        }

        private void width_txtBox_TextChanged(object sender, EventArgs e)
        {
            width_neu = Convert.ToInt32(width_txtBox.Text);
        }

        private void depth_txtBox_TextChanged(object sender, EventArgs e)
        {
            depth_neu = Convert.ToInt32(depth_txtBox.Text);
        }
    }
}
