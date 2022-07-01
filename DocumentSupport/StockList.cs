using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using ComFunction;
using Microsoft.VisualBasic.FileIO;

namespace DocumentSupport
{
    public partial class StockList : Form
    {

        public StockList()
        {
            try
            {
                InitializeComponent();

                setScreen();
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E0301";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }
        #region PATH

        string PathImportStockList = Application.StartupPath + @"\Import\StockBySumCond.csv";
        string PathTemplateLocation = Application.StartupPath + @"\Template\Q02_Location Check.xls";
        string PathOutpuLocation = Application.StartupPath + @"\Output\Q02_Location Check.xls";

        #endregion
        private void GenerateReportFG()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateLocation, PathOutpuLocation, true);
            XlsCreator1.OpenBook(PathOutpuLocation, "");

            string sql = ComFunc.Q05_PackingList_Draft();
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                }
                RowCnt++;
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathOutpuLocation);
        }

        private void DeleteData()
        {
            try
            {
                string sql = "DELETE FROM T07_StockBySumCond_Original";
                ComFunc.ConnectDatabase(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void ImportStockBySum()
        {
           
            DataTable File = (DataTable)ReadCSV(PathImportStockList);
            string[] data = new string[40];
            
            for (int i = 1; i < File.Rows.Count; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    data[j] = File.Rows[i][j].ToString();
                }
               
                InsertStockBySum(data);
                
            }


        }
        private object ReadCSV(string filePath)
        {
            try
            {
                DataTable dtDataSource = new DataTable();
                string[] fileContent = File.ReadAllLines(filePath);
                if (fileContent.Count() > 0)
                {
                    //Create data table columns
                    string[] columns = fileContent[0].Split(',');
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        columns[i] = columns[i].Replace("@@@", ",");
                        columns[i] = columns[i].Replace("\"", "");
                        dtDataSource.Columns.Add(columns[i]);
                    }

                    //Add row data
                    for (int i = 1; i < fileContent.Count(); i++)
                    {
                        string[] rowData = fileContent[i].Split(',');
                        for (int j = 0; j < rowData.Length; j++)
                        {
                            rowData[j] = rowData[j].Replace("@@@", ",");
                            rowData[j] = rowData[j].Replace("\"", "");
                        }
                        dtDataSource.Rows.Add(rowData);
                    }
                }
                return dtDataSource;
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E1202";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
                return null;
            }
        }
        private void InsertStockBySum(string[] data)
        {
            string s_cmd = "INSERT INTO T07_StockBySumCond_Original Values ('" +
                data[0] + "', '" +
                data[1] + "','" +
                data[2] + "', '" +
                data[3] + "', '" +
                data[4] + "', '" +
                data[5] + "', '" +
                data[6] + "', '" +
                data[7] + "', '" +
                data[8] + "', '" +
                data[9] + "', '" +
                data[10] + "','" +
                data[11] + "', '" +
                data[12] + "', '" +
                data[13] + "', '" +
                data[14] + "', '" +
                data[15] + "', '" +
                data[16] + "', '" +
                data[17] + "' ,'" +
                data[18] + "', '" +
                data[19] + "', '" +
                data[20] + "', '" +
                data[21] + "', '" +
                data[22] + "', '" +
                data[23] + "', '" +
                data[24] + "', '" +
                data[25] + "', '" +
                data[26] + "', '" +
                data[27] + "', '" +
                data[28] + "', '" +
                data[29] + "', '" +
                data[30] + "', '" +
                data[31] + "', '" +
                data[32] + "', '" +
                data[33] + "', '" +
                data[34] + "', '" +
                data[35] + "', '" +
                data[36] + "', '" +
                data[37] + "', '" +
                data[38] + "', '" +
                data[39] + "' )";

            ComFunc.ConnectDatabase(s_cmd);
        }
       

        private void Process_Stocklist()
        {

            DialogResult result = MessageBox.Show("Do you import stock list?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {

                //Import with truncate
                DeleteData();
                ImportStockBySum();
                string s_cmd = "";
                s_cmd = ComFunc.Q01_Stock_List_with_FG();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q03_Stock_List_Parts();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q04_Stock_List_Parts_Temp();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q05_Stock_List_Parts_Packed();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q02_Location_Check();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportFG();
                }

                dataGridView1.Focus();
                MessageBox.Show("Import packing stock list!", "Document Support");
            }

        }

        private void setScreen()
        {
            try
            {
                string s_cmd;

                s_cmd = ComFunc.Q01_Stock_List_with_FG();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();

                }

                s_cmd = ComFunc.Q03_Stock_List_Parts();
                DataTable dt2 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt2)
                {
                    dataGridView2.DataSource = dt2;
                    dataGridView2.ClearSelection();
                    lbl_Cnt.Text = dt2.Rows.Count.ToString();

                }

                s_cmd = ComFunc.Q04_Stock_List_Parts_Temp();
                DataTable dt3 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt3)
                {
                    dataGridView3.DataSource = dt3;
                    dataGridView3.ClearSelection();
                    lbl_Cnt.Text = dt3.Rows.Count.ToString();
                }

                s_cmd = ComFunc.Q05_Stock_List_Parts_Packed();
                DataTable dt4 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt4)
                {
                    dataGridView4.DataSource = dt4;
                    dataGridView4.ClearSelection();
                    lbl_Cnt.Text = dt4.Rows.Count.ToString();
                }

                s_cmd = ComFunc.Q02_Location_Check();
                DataTable dt5 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt5)
                {
                    dataGridView5.DataSource = dt5;
                    dataGridView5.ClearSelection();
                    lbl_Cnt.Text = dt5.Rows.Count.ToString();
                }

            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3101";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            setScreen();
        }

        private void btn_FG_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "Q01_Stock List with FG", true);
        }

        private void btn_Part_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView2, "Q03_Stock List Parts", true);
        }

        private void btn_PartTemp_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView3, "Q05_Stock List Parts_Packed", true);
        }

        private void btn_PartPacked_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView4, "Q04_Stock List Parts_Temp", true);
        }

        private void btn_LocationCheck_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView5, "Q02_Location Check", true);
        }

        private void Btn_import_Click(object sender, EventArgs e)
        {
            Process_Stocklist();
            setScreen();
        }
    }
}
