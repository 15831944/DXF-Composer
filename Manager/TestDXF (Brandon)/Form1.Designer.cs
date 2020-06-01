namespace TestDXF
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.polyline_listBox = new System.Windows.Forms.ListBox();
            this.polylinen_lbl = new System.Windows.Forms.Label();
            this.loadDXF_Button = new System.Windows.Forms.Button();
            this.pfad_TxtBox = new System.Windows.Forms.TextBox();
            this.darstellungEinsehen_btn = new System.Windows.Forms.Button();
            this.Punkte = new System.Windows.Forms.Label();
            this.punkte_listBox = new System.Windows.Forms.ListBox();
            this.depth_lbl = new System.Windows.Forms.Label();
            this.width_lbl = new System.Windows.Forms.Label();
            this.Height_lbl = new System.Windows.Forms.Label();
            this.werkzeug_lbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.arbeitseigenschaftenÄndernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.annulierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
           // this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // polyline_listBox
            // 
            this.polyline_listBox.FormattingEnabled = true;
            this.polyline_listBox.Location = new System.Drawing.Point(9, 143);
            this.polyline_listBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.polyline_listBox.Name = "polyline_listBox";
            this.polyline_listBox.Size = new System.Drawing.Size(199, 225);
            this.polyline_listBox.TabIndex = 0;
            this.polyline_listBox.SelectedIndexChanged += new System.EventHandler(this.polyline_listBox_SelectedIndexChanged);
            // 
            // polylinen_lbl
            // 
            this.polylinen_lbl.AutoSize = true;
            this.polylinen_lbl.Location = new System.Drawing.Point(9, 111);
            this.polylinen_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.polylinen_lbl.Name = "polylinen_lbl";
            this.polylinen_lbl.Size = new System.Drawing.Size(51, 13);
            this.polylinen_lbl.TabIndex = 1;
            this.polylinen_lbl.Text = "Polylinien";
            // 
            // loadDXF_Button
            // 
            this.loadDXF_Button.Location = new System.Drawing.Point(9, 20);
            this.loadDXF_Button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loadDXF_Button.Name = "loadDXF_Button";
            this.loadDXF_Button.Size = new System.Drawing.Size(115, 62);
            this.loadDXF_Button.TabIndex = 6;
            this.loadDXF_Button.Text = "Hochladen DXF Datei";
            this.loadDXF_Button.UseVisualStyleBackColor = true;
            this.loadDXF_Button.Click += new System.EventHandler(this.loadDXF_Button_Click);
            // 
            // pfad_TxtBox
            // 
            this.pfad_TxtBox.Location = new System.Drawing.Point(138, 41);
            this.pfad_TxtBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pfad_TxtBox.Name = "pfad_TxtBox";
            this.pfad_TxtBox.ReadOnly = true;
            this.pfad_TxtBox.Size = new System.Drawing.Size(324, 20);
            this.pfad_TxtBox.TabIndex = 7;
            this.pfad_TxtBox.TextChanged += new System.EventHandler(this.pfad_TxtBox_TextChanged);
            // 
            // darstellungEinsehen_btn
            // 
            this.darstellungEinsehen_btn.Location = new System.Drawing.Point(567, 284);
            this.darstellungEinsehen_btn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.darstellungEinsehen_btn.Name = "darstellungEinsehen_btn";
            this.darstellungEinsehen_btn.Size = new System.Drawing.Size(115, 62);
            this.darstellungEinsehen_btn.TabIndex = 8;
            this.darstellungEinsehen_btn.Text = "Polylinien zeichnen";
            this.darstellungEinsehen_btn.UseVisualStyleBackColor = true;
            this.darstellungEinsehen_btn.Click += new System.EventHandler(this.darstellungEinsehen_btn_Click);
            // 
            // Punkte
            // 
            this.Punkte.AutoSize = true;
            this.Punkte.Location = new System.Drawing.Point(258, 111);
            this.Punkte.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Punkte.Name = "Punkte";
            this.Punkte.Size = new System.Drawing.Size(41, 13);
            this.Punkte.TabIndex = 10;
            this.Punkte.Text = "Punkte";
            // 
            // punkte_listBox
            // 
            this.punkte_listBox.FormattingEnabled = true;
            this.punkte_listBox.Location = new System.Drawing.Point(258, 143);
            this.punkte_listBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.punkte_listBox.Name = "punkte_listBox";
            this.punkte_listBox.Size = new System.Drawing.Size(204, 225);
            this.punkte_listBox.TabIndex = 9;
            // 
            // depth_lbl
            // 
            this.depth_lbl.AutoSize = true;
            this.depth_lbl.Location = new System.Drawing.Point(10, 93);
            this.depth_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.depth_lbl.Name = "depth_lbl";
            this.depth_lbl.Size = new System.Drawing.Size(36, 13);
            this.depth_lbl.TabIndex = 9;
            this.depth_lbl.Text = "Depth";
            // 
            // width_lbl
            // 
            this.width_lbl.AutoSize = true;
            this.width_lbl.Location = new System.Drawing.Point(10, 67);
            this.width_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.width_lbl.Name = "width_lbl";
            this.width_lbl.Size = new System.Drawing.Size(35, 13);
            this.width_lbl.TabIndex = 8;
            this.width_lbl.Text = "Width";
            // 
            // Height_lbl
            // 
            this.Height_lbl.AutoSize = true;
            this.Height_lbl.Location = new System.Drawing.Point(10, 39);
            this.Height_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Height_lbl.Name = "Height_lbl";
            this.Height_lbl.Size = new System.Drawing.Size(38, 13);
            this.Height_lbl.TabIndex = 7;
            this.Height_lbl.Text = "Height";
            // 
            // werkzeug_lbl
            // 
            this.werkzeug_lbl.AutoSize = true;
            this.werkzeug_lbl.Location = new System.Drawing.Point(10, 14);
            this.werkzeug_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.werkzeug_lbl.Name = "werkzeug_lbl";
            this.werkzeug_lbl.Size = new System.Drawing.Size(56, 13);
            this.werkzeug_lbl.TabIndex = 6;
            this.werkzeug_lbl.Text = "Werkzeug";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Height_lbl);
            this.panel1.Controls.Add(this.depth_lbl);
            this.panel1.Controls.Add(this.werkzeug_lbl);
            this.panel1.Controls.Add(this.width_lbl);
            this.panel1.Location = new System.Drawing.Point(486, 159);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 119);
            this.panel1.TabIndex = 11;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(494, 143);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Arbeitseigenschaften";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arbeitseigenschaftenÄndernToolStripMenuItem,
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem,
            this.annulierenToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(249, 70);
            // 
            // arbeitseigenschaftenÄndernToolStripMenuItem
            // 
            this.arbeitseigenschaftenÄndernToolStripMenuItem.Name = "arbeitseigenschaftenÄndernToolStripMenuItem";
            this.arbeitseigenschaftenÄndernToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.arbeitseigenschaftenÄndernToolStripMenuItem.Text = "Arbeitseigenschaften ändern";
            this.arbeitseigenschaftenÄndernToolStripMenuItem.Click += new System.EventHandler(this.arbeitseigenschaftenÄndernToolStripMenuItem_Click_1);
            // 
            // arbeitseigenschaftenHinzufügenToolStripMenuItem
            // 
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem.Name = "arbeitseigenschaftenHinzufügenToolStripMenuItem";
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem.Text = "Arbeitseigenschaften hinzufügen";
            this.arbeitseigenschaftenHinzufügenToolStripMenuItem.Click += new System.EventHandler(this.arbeitseigenschaftenHinzufügenToolStripMenuItem_Click_1);
            // 
            // annulierenToolStripMenuItem
            // 
            this.annulierenToolStripMenuItem.Name = "annulierenToolStripMenuItem";
            this.annulierenToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.annulierenToolStripMenuItem.Text = "Annulieren";
            this.annulierenToolStripMenuItem.Click += new System.EventHandler(this.annulierenToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(458, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 21);
            this.label2.TabIndex = 14;
            this.label2.Text = "label2";
            // 
            // button1
            // 
            /*this.button1.Location = new System.Drawing.Point(477, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);*/
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 377);
           // this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Punkte);
            this.Controls.Add(this.punkte_listBox);
            this.Controls.Add(this.darstellungEinsehen_btn);
            this.Controls.Add(this.pfad_TxtBox);
            this.Controls.Add(this.loadDXF_Button);
            this.Controls.Add(this.polylinen_lbl);
            this.Controls.Add(this.polyline_listBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox polyline_listBox;
        private System.Windows.Forms.Label polylinen_lbl;
        private System.Windows.Forms.Button loadDXF_Button;
        private System.Windows.Forms.TextBox pfad_TxtBox;
        private System.Windows.Forms.Button darstellungEinsehen_btn;
        private System.Windows.Forms.Label Punkte;
        private System.Windows.Forms.ListBox punkte_listBox;
        private System.Windows.Forms.Label depth_lbl;
        private System.Windows.Forms.Label width_lbl;
        private System.Windows.Forms.Label Height_lbl;
        private System.Windows.Forms.Label werkzeug_lbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arbeitseigenschaftenÄndernToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arbeitseigenschaftenHinzufügenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem annulierenToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.Button button1;
    }
}

