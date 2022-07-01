namespace DocumentSupport
{
    partial class TFZOutputRe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFZOutputRe));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.utDateTo = new ProManApp.UltraDateTimePicker();
            this.utDateFrom = new ProManApp.UltraDateTimePicker();
            this.txt_Search = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_OutputReport = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utDateTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utDateFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblID);
            this.panel1.Controls.Add(this.utDateTo);
            this.panel1.Controls.Add(this.utDateFrom);
            this.panel1.Controls.Add(this.txt_Search);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 187);
            this.panel1.TabIndex = 1;
            this.panel1.Tag = "";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(118, 112);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 40);
            this.label3.TabIndex = 82;
            this.label3.Text = "To";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblID
            // 
            this.lblID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblID.Location = new System.Drawing.Point(100, 54);
            this.lblID.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(76, 40);
            this.lblID.TabIndex = 81;
            this.lblID.Text = "From";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // utDateTo
            // 
            this.utDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.utDateTo.Location = new System.Drawing.Point(184, 112);
            this.utDateTo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.utDateTo.Name = "utDateTo";
            this.utDateTo.Size = new System.Drawing.Size(218, 31);
            this.utDateTo.TabIndex = 2;
            this.utDateTo.Value = new System.DateTime(2021, 4, 21, 0, 0, 0, 0);
            this.utDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UtDateTo_KeyDown);
            // 
            // utDateFrom
            // 
            this.utDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.utDateFrom.Location = new System.Drawing.Point(184, 56);
            this.utDateFrom.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.utDateFrom.Name = "utDateFrom";
            this.utDateFrom.Size = new System.Drawing.Size(218, 31);
            this.utDateFrom.TabIndex = 1;
            this.utDateFrom.Value = new System.DateTime(2021, 4, 21, 0, 0, 0, 0);
            this.utDateFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UtDateFrom_KeyDown);
            // 
            // txt_Search
            // 
            this.txt_Search.AutoSize = true;
            this.txt_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txt_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Search.ForeColor = System.Drawing.Color.White;
            this.txt_Search.Location = new System.Drawing.Point(2, 0);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(207, 37);
            this.txt_Search.TabIndex = 50;
            this.txt_Search.Text = "Output report";
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.SystemColors.Control;
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_close.Image = global::DocumentSupport.Properties.Resources.close;
            this.btn_close.Location = new System.Drawing.Point(404, 225);
            this.btn_close.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(182, 77);
            this.btn_close.TabIndex = 53;
            this.btn_close.Text = "Close";
            this.btn_close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_OutputReport
            // 
            this.btn_OutputReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OutputReport.BackColor = System.Drawing.SystemColors.Control;
            this.btn_OutputReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_OutputReport.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_OutputReport.Location = new System.Drawing.Point(12, 225);
            this.btn_OutputReport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_OutputReport.Name = "btn_OutputReport";
            this.btn_OutputReport.Size = new System.Drawing.Size(306, 77);
            this.btn_OutputReport.TabIndex = 3;
            this.btn_OutputReport.Text = "Output Report";
            this.btn_OutputReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_OutputReport.UseVisualStyleBackColor = false;
            this.btn_OutputReport.Click += new System.EventHandler(this.Btn_OutputReport_Click);
            // 
            // TFZOutputRe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(590, 248);
            this.Controls.Add(this.btn_OutputReport);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximumSize = new System.Drawing.Size(616, 319);
            this.MinimumSize = new System.Drawing.Size(616, 319);
            this.Name = "TFZOutputRe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Support";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utDateTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utDateFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label lblID;
        private ProManApp.UltraDateTimePicker utDateTo;
        private ProManApp.UltraDateTimePicker utDateFrom;
        private System.Windows.Forms.Label txt_Search;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_OutputReport;
    }
}