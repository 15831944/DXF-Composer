namespace TestDXF
{
    partial class Arbeitseigenschaften
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
            this.werkzeug_txtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.height_txtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.width_txtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.depth_txtBox = new System.Windows.Forms.TextBox();
            this.Vornehmen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // werkzeug_txtBox
            // 
            this.werkzeug_txtBox.Location = new System.Drawing.Point(12, 74);
            this.werkzeug_txtBox.Name = "werkzeug_txtBox";
            this.werkzeug_txtBox.Size = new System.Drawing.Size(262, 22);
            this.werkzeug_txtBox.TabIndex = 0;
         this.werkzeug_txtBox.TextChanged += new System.EventHandler(this.werkzeug_txtBox_TextChanged);
    
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Werkzeug";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Height";
            // 
            // height_txtBox
            // 
            this.height_txtBox.Location = new System.Drawing.Point(12, 132);
            this.height_txtBox.Name = "height_txtBox";
            this.height_txtBox.Size = new System.Drawing.Size(262, 22);
            this.height_txtBox.TabIndex = 2;
            this.height_txtBox.TextChanged += new System.EventHandler(this.height_txtBox_TextChanged);
            this.height_txtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Width";
            // 
            // width_txtBox
            // 
            this.width_txtBox.Location = new System.Drawing.Point(12, 191);
            this.width_txtBox.Name = "width_txtBox";
            this.width_txtBox.Size = new System.Drawing.Size(262, 22);
            this.width_txtBox.TabIndex = 4;
            this.width_txtBox.TextChanged += new System.EventHandler(this.width_txtBox_TextChanged);
            this.width_txtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Depth";
            // 
            // depth_txtBox
            // 
            this.depth_txtBox.Location = new System.Drawing.Point(9, 249);
            this.depth_txtBox.Name = "depth_txtBox";
            this.depth_txtBox.Size = new System.Drawing.Size(265, 22);
            this.depth_txtBox.TabIndex = 6;
           this.depth_txtBox.TextChanged += new System.EventHandler(this.depth_txtBox_TextChanged);
            this.depth_txtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // Vornehmen
            // 
            this.Vornehmen.Location = new System.Drawing.Point(9, 307);
            this.Vornehmen.Name = "Vornehmen";
            this.Vornehmen.Size = new System.Drawing.Size(265, 69);
            this.Vornehmen.TabIndex = 8;
            this.Vornehmen.Text = "Vornehmen";
            this.Vornehmen.UseVisualStyleBackColor = true;
            this.Vornehmen.Click += new System.EventHandler(this.Vornehmen_Click);
            // 
            // Arbeitseigenschaften
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 388);
            this.Controls.Add(this.Vornehmen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.depth_txtBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.width_txtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.height_txtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.werkzeug_txtBox);
            this.Name = "Arbeitseigenschaften";
            this.Text = "Arbeitseigenschaften";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox werkzeug_txtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox height_txtBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox width_txtBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox depth_txtBox;
        private System.Windows.Forms.Button Vornehmen;
    }
}