namespace lab1
{
    partial class frmCustomerBill
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
            this.btnResidential = new System.Windows.Forms.Button();
            this.btnIndustrial = new System.Windows.Forms.Button();
            this.btnCommercial = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnResidential
            // 
            this.btnResidential.Location = new System.Drawing.Point(45, 28);
            this.btnResidential.Name = "btnResidential";
            this.btnResidential.Size = new System.Drawing.Size(110, 46);
            this.btnResidential.TabIndex = 0;
            this.btnResidential.Text = "&Residential";
            this.btnResidential.UseVisualStyleBackColor = true;
            this.btnResidential.Click += new System.EventHandler(this.btnResidential_Click);
            // 
            // btnIndustrial
            // 
            this.btnIndustrial.Location = new System.Drawing.Point(45, 168);
            this.btnIndustrial.Name = "btnIndustrial";
            this.btnIndustrial.Size = new System.Drawing.Size(110, 46);
            this.btnIndustrial.TabIndex = 1;
            this.btnIndustrial.Text = "&Industrial";
            this.btnIndustrial.UseVisualStyleBackColor = true;
            this.btnIndustrial.Click += new System.EventHandler(this.btnIndustrial_Click);
            // 
            // btnCommercial
            // 
            this.btnCommercial.Location = new System.Drawing.Point(45, 99);
            this.btnCommercial.Name = "btnCommercial";
            this.btnCommercial.Size = new System.Drawing.Size(110, 46);
            this.btnCommercial.TabIndex = 2;
            this.btnCommercial.Text = "&Commercial";
            this.btnCommercial.UseVisualStyleBackColor = true;
            this.btnCommercial.Click += new System.EventHandler(this.btnCommercial_Click);
           
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(161, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(845, 457);
            this.panel.TabIndex = 3;
            //this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // frmCustomerBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1010, 452);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btnCommercial);
            this.Controls.Add(this.btnIndustrial);
            this.Controls.Add(this.btnResidential);
            this.Name = "frmCustomerBill";
            this.Text = "Customer Bill";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnResidential;
        private System.Windows.Forms.Button btnIndustrial;
        private System.Windows.Forms.Button btnCommercial;
        private System.Windows.Forms.Panel panel;
    }
}

