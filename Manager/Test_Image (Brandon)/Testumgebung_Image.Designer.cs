namespace Test_Image
{
    partial class Testumgebung_Image
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Testumgebung_Image));
            this.load_image_Button = new System.Windows.Forms.Button();
            this.pfad_textBox = new System.Windows.Forms.TextBox();
            this.polyLine_listBox = new System.Windows.Forms.ListBox();
            this.points_listBox = new System.Windows.Forms.ListBox();
            this.draw_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ruckgangig_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // load_image_Button
            // 
            this.load_image_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.load_image_Button.Location = new System.Drawing.Point(597, 27);
            this.load_image_Button.Name = "load_image_Button";
            this.load_image_Button.Size = new System.Drawing.Size(147, 74);
            this.load_image_Button.TabIndex = 0;
            this.load_image_Button.Text = "Bild hochladen";
            this.load_image_Button.UseVisualStyleBackColor = true;
            this.load_image_Button.Click += new System.EventHandler(this.load_image_Button_Click);
            // 
            // pfad_textBox
            // 
            this.pfad_textBox.Location = new System.Drawing.Point(12, 53);
            this.pfad_textBox.Name = "pfad_textBox";
            this.pfad_textBox.ReadOnly = true;
            this.pfad_textBox.Size = new System.Drawing.Size(579, 22);
            this.pfad_textBox.TabIndex = 1;
            // 
            // polyLine_listBox
            // 
            this.polyLine_listBox.FormattingEnabled = true;
            this.polyLine_listBox.ItemHeight = 16;
            this.polyLine_listBox.Location = new System.Drawing.Point(12, 140);
            this.polyLine_listBox.Name = "polyLine_listBox";
            this.polyLine_listBox.ScrollAlwaysVisible = true;
            this.polyLine_listBox.Size = new System.Drawing.Size(271, 324);
            this.polyLine_listBox.TabIndex = 2;
            this.polyLine_listBox.SelectedIndexChanged += new System.EventHandler(this.PolyLine_SelectedIndexChanged);
            // 
            // points_listBox
            // 
            this.points_listBox.FormattingEnabled = true;
            this.points_listBox.ItemHeight = 16;
            this.points_listBox.Location = new System.Drawing.Point(305, 140);
            this.points_listBox.Name = "points_listBox";
            this.points_listBox.ScrollAlwaysVisible = true;
            this.points_listBox.Size = new System.Drawing.Size(286, 324);
            this.points_listBox.TabIndex = 3;
            // 
            // draw_button
            // 
            this.draw_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.draw_button.Location = new System.Drawing.Point(597, 310);
            this.draw_button.Name = "draw_button";
            this.draw_button.Size = new System.Drawing.Size(147, 74);
            this.draw_button.TabIndex = 6;
            this.draw_button.Text = "Zeichnen";
            this.draw_button.UseVisualStyleBackColor = true;
            this.draw_button.Click += new System.EventHandler(this.draw_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Polylinien";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(302, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Points";
            // 
            // ruckgangig_btn
            // 
            this.ruckgangig_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ruckgangig_btn.Location = new System.Drawing.Point(597, 390);
            this.ruckgangig_btn.Name = "ruckgangig_btn";
            this.ruckgangig_btn.Size = new System.Drawing.Size(147, 74);
            this.ruckgangig_btn.TabIndex = 10;
            this.ruckgangig_btn.Text = "Zurücksetzen";
            this.ruckgangig_btn.UseVisualStyleBackColor = true;
            this.ruckgangig_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Pfad";
            // 
            // Testumgebung_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(749, 476);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ruckgangig_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.draw_button);
            this.Controls.Add(this.points_listBox);
            this.Controls.Add(this.polyLine_listBox);
            this.Controls.Add(this.pfad_textBox);
            this.Controls.Add(this.load_image_Button);
            this.Name = "Testumgebung_Image";
            this.Text = "Testumgebung Image Symbol";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load_image_Button;
        private System.Windows.Forms.TextBox pfad_textBox;
        private System.Windows.Forms.ListBox polyLine_listBox;
        private System.Windows.Forms.ListBox points_listBox;
        private System.Windows.Forms.Button draw_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ruckgangig_btn;
        private System.Windows.Forms.Label label4;
    }
}

