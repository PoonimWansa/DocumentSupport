namespace DocumentSupport
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StockList = new System.Windows.Forms.Button();
            this.btn_DebitNote = new System.Windows.Forms.Button();
            this.btn_PackingList = new System.Windows.Forms.Button();
            this.btn_TFZReport = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnManual = new System.Windows.Forms.Button();
            this.btn_user = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 37);
            this.label1.TabIndex = 12;
            this.label1.Text = "Document Support";
            // 
            // btn_StockList
            // 
            this.btn_StockList.BackColor = System.Drawing.SystemColors.Control;
            this.btn_StockList.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StockList.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_StockList.Location = new System.Drawing.Point(340, 100);
            this.btn_StockList.Name = "btn_StockList";
            this.btn_StockList.Size = new System.Drawing.Size(183, 60);
            this.btn_StockList.TabIndex = 10;
            this.btn_StockList.Text = "Stock List";
            this.btn_StockList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_StockList.UseVisualStyleBackColor = false;
            this.btn_StockList.Click += new System.EventHandler(this.btn_StockList_Click);
            // 
            // btn_DebitNote
            // 
            this.btn_DebitNote.BackColor = System.Drawing.SystemColors.Control;
            this.btn_DebitNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DebitNote.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_DebitNote.Location = new System.Drawing.Point(114, 183);
            this.btn_DebitNote.Name = "btn_DebitNote";
            this.btn_DebitNote.Size = new System.Drawing.Size(183, 60);
            this.btn_DebitNote.TabIndex = 10;
            this.btn_DebitNote.Text = "Debit Note";
            this.btn_DebitNote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_DebitNote.UseVisualStyleBackColor = false;
            this.btn_DebitNote.Click += new System.EventHandler(this.btn_DebitNote_Click);
            // 
            // btn_PackingList
            // 
            this.btn_PackingList.BackColor = System.Drawing.SystemColors.Control;
            this.btn_PackingList.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PackingList.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_PackingList.Location = new System.Drawing.Point(114, 100);
            this.btn_PackingList.Name = "btn_PackingList";
            this.btn_PackingList.Size = new System.Drawing.Size(183, 60);
            this.btn_PackingList.TabIndex = 8;
            this.btn_PackingList.Text = "Packing List";
            this.btn_PackingList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_PackingList.UseVisualStyleBackColor = false;
            this.btn_PackingList.Click += new System.EventHandler(this.btn_PackingList_Click);
            // 
            // btn_TFZReport
            // 
            this.btn_TFZReport.BackColor = System.Drawing.SystemColors.Control;
            this.btn_TFZReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TFZReport.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_TFZReport.Location = new System.Drawing.Point(340, 183);
            this.btn_TFZReport.Name = "btn_TFZReport";
            this.btn_TFZReport.Size = new System.Drawing.Size(183, 60);
            this.btn_TFZReport.TabIndex = 10;
            this.btn_TFZReport.Text = "TFZ Report";
            this.btn_TFZReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_TFZReport.UseVisualStyleBackColor = false;
            this.btn_TFZReport.Click += new System.EventHandler(this.btn_TFZReport_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.SystemColors.Control;
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_close.Image = global::DocumentSupport.Properties.Resources.close;
            this.btn_close.Location = new System.Drawing.Point(496, 302);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(124, 44);
            this.btn_close.TabIndex = 10;
            this.btn_close.Text = "Close";
            this.btn_close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblVersion.Location = new System.Drawing.Point(588, 13);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(32, 13);
            this.lblVersion.TabIndex = 13;
            this.lblVersion.Text = "1.01";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(552, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Ver.";
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.SystemColors.Control;
            this.btnManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btnManual.Location = new System.Drawing.Point(160, 301);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(142, 44);
            this.btnManual.TabIndex = 10;
            this.btnManual.Text = "Manual";
            this.btnManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManual.UseVisualStyleBackColor = false;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btn_user
            // 
            this.btn_user.BackColor = System.Drawing.SystemColors.Control;
            this.btn_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_user.Image = global::DocumentSupport.Properties.Resources.folder;
            this.btn_user.Location = new System.Drawing.Point(12, 301);
            this.btn_user.Name = "btn_user";
            this.btn_user.Size = new System.Drawing.Size(142, 44);
            this.btn_user.TabIndex = 15;
            this.btn_user.Text = "Master";
            this.btn_user.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_user.UseVisualStyleBackColor = false;
            this.btn_user.Click += new System.EventHandler(this.btn_user_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(632, 357);
            this.Controls.Add(this.btn_user);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_StockList);
            this.Controls.Add(this.btn_DebitNote);
            this.Controls.Add(this.btn_PackingList);
            this.Controls.Add(this.btn_TFZReport);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.btn_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Support";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_PackingList;
        private System.Windows.Forms.Button btn_DebitNote;
        private System.Windows.Forms.Button btn_TFZReport;
        private System.Windows.Forms.Button btn_StockList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btn_user;
    }
}

