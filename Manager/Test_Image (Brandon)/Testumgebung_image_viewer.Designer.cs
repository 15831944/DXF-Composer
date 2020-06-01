namespace Test_Image
{
    partial class Testumgebung_image_viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.vectorizedImage_pictBox = new System.Windows.Forms.PictureBox();
            this.originalImage_pictBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.punkte_anzeigen_checkbox = new System.Windows.Forms.CheckBox();
            this.befuellen_CheckBox = new System.Windows.Forms.CheckBox();
            this.tracing_checkBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.vectorizedImage_pictBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.originalImage_pictBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            // 
            // vectorizedImage_pictBox
            // 
            this.vectorizedImage_pictBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vectorizedImage_pictBox.Location = new System.Drawing.Point(493, 3);
            this.vectorizedImage_pictBox.Name = "vectorizedImage_pictBox";
            this.vectorizedImage_pictBox.Size = new System.Drawing.Size(486, 771);
            this.vectorizedImage_pictBox.TabIndex = 1;
            this.vectorizedImage_pictBox.TabStop = false;
            // 
            // originalImage_pictBox
            // 
            this.originalImage_pictBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalImage_pictBox.Location = new System.Drawing.Point(3, 3);
            this.originalImage_pictBox.Name = "originalImage_pictBox";
            this.originalImage_pictBox.Size = new System.Drawing.Size(484, 771);
            this.originalImage_pictBox.TabIndex = 0;
            this.originalImage_pictBox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.94913F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.05086F));
            this.tableLayoutPanel1.Controls.Add(this.originalImage_pictBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.vectorizedImage_pictBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(982, 911);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tracing_checkBox);
            this.groupBox1.Controls.Add(this.befuellen_CheckBox);
            this.groupBox1.Controls.Add(this.punkte_anzeigen_checkbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 780);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 129);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zeichnung bearbeiten";
            // 
            // punkte_anzeigen_checkbox
            // 
            this.punkte_anzeigen_checkbox.AutoSize = true;
            this.punkte_anzeigen_checkbox.Location = new System.Drawing.Point(6, 40);
            this.punkte_anzeigen_checkbox.Name = "punkte_anzeigen_checkbox";
            this.punkte_anzeigen_checkbox.Size = new System.Drawing.Size(136, 21);
            this.punkte_anzeigen_checkbox.TabIndex = 0;
            this.punkte_anzeigen_checkbox.Text = "Punkte anzeigen";
            this.punkte_anzeigen_checkbox.UseVisualStyleBackColor = true;
            this.punkte_anzeigen_checkbox.CheckedChanged += new System.EventHandler(this.punkte_anzeigen_checkbox_CheckedChanged);
            // 
            // befuellen_CheckBox
            // 
            this.befuellen_CheckBox.AutoSize = true;
            this.befuellen_CheckBox.Location = new System.Drawing.Point(6, 67);
            this.befuellen_CheckBox.Name = "befuellen_CheckBox";
            this.befuellen_CheckBox.Size = new System.Drawing.Size(81, 21);
            this.befuellen_CheckBox.TabIndex = 1;
            this.befuellen_CheckBox.Text = "Befüllen";
            this.befuellen_CheckBox.UseVisualStyleBackColor = true;
            this.befuellen_CheckBox.CheckedChanged += new System.EventHandler(this.befuellen_CheckBox_CheckedChanged);
            // 
            // tracing_checkBox
            // 
            this.tracing_checkBox.AutoSize = true;
            this.tracing_checkBox.Location = new System.Drawing.Point(6, 95);
            this.tracing_checkBox.Name = "tracing_checkBox";
            this.tracing_checkBox.Size = new System.Drawing.Size(78, 21);
            this.tracing_checkBox.TabIndex = 2;
            this.tracing_checkBox.Text = "Tracing";
            this.tracing_checkBox.UseVisualStyleBackColor = true;
            this.tracing_checkBox.CheckedChanged += new System.EventHandler(this.tracing_checkBox_CheckedChanged);
            // 
            // Testumgebung_image_viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 911);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Name = "Testumgebung_image_viewer";
            this.Text = "Testumgebung Image Symbol Viewer";

            ((System.ComponentModel.ISupportInitialize)(this.vectorizedImage_pictBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.originalImage_pictBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox vectorizedImage_pictBox;
        private System.Windows.Forms.PictureBox originalImage_pictBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox tracing_checkBox;
        private System.Windows.Forms.CheckBox befuellen_CheckBox;
        private System.Windows.Forms.CheckBox punkte_anzeigen_checkbox;
    }
}