namespace DocumentSupport
{
    partial class TFZReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFZReport));
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbl_Cnt = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.StockList = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_import_inbound = new System.Windows.Forms.Button();
            this.btn_import_Outbound = new System.Windows.Forms.Button();
            this.until = new ProManApp.UltraDateTimePicker();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chb_Outbound = new System.Windows.Forms.CheckBox();
            this.chb_Inbound = new System.Windows.Forms.CheckBox();
            this.btn_stockList = new System.Windows.Forms.Button();
            this.btn_OutputReport = new System.Windows.Forms.Button();
            this.btn_Outbound = new System.Windows.Forms.Button();
            this.btn_Inbound = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.until)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.SteelBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(737, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 37);
            this.label3.TabIndex = 1;
            this.label3.Text = "TFZ Report";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(2, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(932, 528);
            this.panel1.TabIndex = 15;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(923, 522);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbl_Cnt);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(915, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Inbound Actual";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbl_Cnt
            // 
            this.lbl_Cnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbl_Cnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_Cnt.Location = new System.Drawing.Point(92, 525);
            this.lbl_Cnt.Name = "lbl_Cnt";
            this.lbl_Cnt.Size = new System.Drawing.Size(100, 14);
            this.lbl_Cnt.TabIndex = 13;
            this.lbl_Cnt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbl_Cnt.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(4, 525);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 14);
            this.label6.TabIndex = 12;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(5, 5);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(904, 491);
            this.dataGridView1.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(915, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Outbound Actual";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(5, 5);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(904, 491);
            this.dataGridView2.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(94, 534);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 14);
            this.label4.TabIndex = 20;
            this.label4.Text = "999,999,999";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(6, 534);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 14);
            this.label5.TabIndex = 19;
            this.label5.Text = "Total Stock :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(915, 496);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Stock List Final";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(3, 5);
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(907, 491);
            this.dataGridView3.TabIndex = 22;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(915, 496);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Error list";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(4, 5);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView4.Size = new System.Drawing.Size(905, 491);
            this.dataGridView4.TabIndex = 0;
            // 
            // StockList
            // 
            this.StockList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StockList.BackColor = System.Drawing.SystemColors.Control;
            this.StockList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StockList.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.StockList.Location = new System.Drawing.Point(322, 639);
            this.StockList.Name = "StockList";
            this.StockList.Size = new System.Drawing.Size(141, 40);
            this.StockList.TabIndex = 23;
            this.StockList.Text = "Stock List";
            this.StockList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.StockList.UseVisualStyleBackColor = false;
            this.StockList.Click += new System.EventHandler(this.StockList_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_refresh.BackColor = System.Drawing.SystemColors.Control;
            this.btn_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_refresh.Image = global::DocumentSupport.Properties.Resources.database;
            this.btn_refresh.Location = new System.Drawing.Point(684, 639);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(120, 40);
            this.btn_refresh.TabIndex = 19;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_refresh.UseVisualStyleBackColor = false;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.SystemColors.Control;
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_close.Image = global::DocumentSupport.Properties.Resources.close;
            this.btn_close.Location = new System.Drawing.Point(810, 639);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(120, 40);
            this.btn_close.TabIndex = 20;
            this.btn_close.Text = "Close";
            this.btn_close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_import_inbound
            // 
            this.btn_import_inbound.BackColor = System.Drawing.SystemColors.Control;
            this.btn_import_inbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_import_inbound.Image = global::DocumentSupport.Properties.Resources.folder;
            this.btn_import_inbound.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_import_inbound.Location = new System.Drawing.Point(6, 11);
            this.btn_import_inbound.Name = "btn_import_inbound";
            this.btn_import_inbound.Size = new System.Drawing.Size(242, 40);
            this.btn_import_inbound.TabIndex = 27;
            this.btn_import_inbound.Text = "Import Inbound/MoveIn";
            this.btn_import_inbound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_import_inbound.UseVisualStyleBackColor = false;
            this.btn_import_inbound.Click += new System.EventHandler(this.btn_import_inbound_Click);
            // 
            // btn_import_Outbound
            // 
            this.btn_import_Outbound.BackColor = System.Drawing.SystemColors.Control;
            this.btn_import_Outbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_import_Outbound.Image = global::DocumentSupport.Properties.Resources.folder;
            this.btn_import_Outbound.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_import_Outbound.Location = new System.Drawing.Point(6, 57);
            this.btn_import_Outbound.Name = "btn_import_Outbound";
            this.btn_import_Outbound.Size = new System.Drawing.Size(242, 40);
            this.btn_import_Outbound.TabIndex = 28;
            this.btn_import_Outbound.Text = "Import Outbound/MoveOut";
            this.btn_import_Outbound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_import_Outbound.UseVisualStyleBackColor = false;
            this.btn_import_Outbound.Click += new System.EventHandler(this.btn_import_Outbound_Click);
            // 
            // until
            // 
            this.until.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.until.Location = new System.Drawing.Point(421, 59);
            this.until.Name = "until";
            this.until.Size = new System.Drawing.Size(128, 20);
            this.until.TabIndex = 47;
            this.until.Value = new System.DateTime(2020, 7, 30, 0, 0, 0, 0);
            // 
            // btn_Clear
            // 
            this.btn_Clear.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_Clear.Image = global::DocumentSupport.Properties.Resources.close;
            this.btn_Clear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Clear.Location = new System.Drawing.Point(254, 57);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(135, 40);
            this.btn_Clear.TabIndex = 48;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Clear.UseVisualStyleBackColor = false;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(396, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 49;
            this.label2.Text = "～";
            // 
            // chb_Outbound
            // 
            this.chb_Outbound.AutoSize = true;
            this.chb_Outbound.Checked = true;
            this.chb_Outbound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_Outbound.Location = new System.Drawing.Point(482, 81);
            this.chb_Outbound.Name = "chb_Outbound";
            this.chb_Outbound.Size = new System.Drawing.Size(73, 17);
            this.chb_Outbound.TabIndex = 52;
            this.chb_Outbound.Text = "Outbound";
            this.chb_Outbound.UseVisualStyleBackColor = true;
            // 
            // chb_Inbound
            // 
            this.chb_Inbound.AutoSize = true;
            this.chb_Inbound.Checked = true;
            this.chb_Inbound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_Inbound.Location = new System.Drawing.Point(422, 81);
            this.chb_Inbound.Name = "chb_Inbound";
            this.chb_Inbound.Size = new System.Drawing.Size(65, 17);
            this.chb_Inbound.TabIndex = 51;
            this.chb_Inbound.Text = "Inbound";
            this.chb_Inbound.UseVisualStyleBackColor = true;
            // 
            // btn_stockList
            // 
            this.btn_stockList.BackColor = System.Drawing.SystemColors.Control;
            this.btn_stockList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_stockList.Image = global::DocumentSupport.Properties.Resources.folder;
            this.btn_stockList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_stockList.Location = new System.Drawing.Point(254, 11);
            this.btn_stockList.Name = "btn_stockList";
            this.btn_stockList.Size = new System.Drawing.Size(135, 40);
            this.btn_stockList.TabIndex = 53;
            this.btn_stockList.Text = "Stock List";
            this.btn_stockList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_stockList.UseVisualStyleBackColor = false;
            this.btn_stockList.Click += new System.EventHandler(this.btn_stockList_Click);
            // 
            // btn_OutputReport
            // 
            this.btn_OutputReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OutputReport.BackColor = System.Drawing.SystemColors.Control;
            this.btn_OutputReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_OutputReport.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_OutputReport.Location = new System.Drawing.Point(471, 639);
            this.btn_OutputReport.Name = "btn_OutputReport";
            this.btn_OutputReport.Size = new System.Drawing.Size(158, 40);
            this.btn_OutputReport.TabIndex = 54;
            this.btn_OutputReport.Text = "Output Report";
            this.btn_OutputReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_OutputReport.UseVisualStyleBackColor = false;
            this.btn_OutputReport.Click += new System.EventHandler(this.btn_OutputReport_Click);
            // 
            // btn_Outbound
            // 
            this.btn_Outbound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Outbound.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Outbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Outbound.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_Outbound.Location = new System.Drawing.Point(156, 639);
            this.btn_Outbound.Name = "btn_Outbound";
            this.btn_Outbound.Size = new System.Drawing.Size(158, 40);
            this.btn_Outbound.TabIndex = 22;
            this.btn_Outbound.Text = "Outbound Actual";
            this.btn_Outbound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Outbound.UseVisualStyleBackColor = false;
            this.btn_Outbound.Click += new System.EventHandler(this.btn_Outbound_Click);
            // 
            // btn_Inbound
            // 
            this.btn_Inbound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Inbound.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Inbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Inbound.Image = global::DocumentSupport.Properties.Resources.Excel;
            this.btn_Inbound.Location = new System.Drawing.Point(4, 639);
            this.btn_Inbound.Name = "btn_Inbound";
            this.btn_Inbound.Size = new System.Drawing.Size(147, 40);
            this.btn_Inbound.TabIndex = 21;
            this.btn_Inbound.Text = "Inbound Actual";
            this.btn_Inbound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Inbound.UseVisualStyleBackColor = false;
            this.btn_Inbound.Click += new System.EventHandler(this.btn_Inbound_Click);
            // 
            // TFZReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(937, 687);
            this.Controls.Add(this.btn_OutputReport);
            this.Controls.Add(this.btn_stockList);
            this.Controls.Add(this.chb_Outbound);
            this.Controls.Add(this.chb_Inbound);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.until);
            this.Controls.Add(this.btn_import_Outbound);
            this.Controls.Add(this.btn_import_inbound);
            this.Controls.Add(this.StockList);
            this.Controls.Add(this.btn_Outbound);
            this.Controls.Add(this.btn_Inbound);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(953, 726);
            this.Name = "TFZReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Support";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.until)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button StockList;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btn_import_inbound;
        private System.Windows.Forms.Button btn_import_Outbound;
        private ProManApp.UltraDateTimePicker until;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chb_Outbound;
        private System.Windows.Forms.CheckBox chb_Inbound;
        private System.Windows.Forms.Button btn_stockList;
        private System.Windows.Forms.Button btn_OutputReport;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lbl_Cnt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_Outbound;
        private System.Windows.Forms.Button btn_Inbound;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridView4;
    }
}