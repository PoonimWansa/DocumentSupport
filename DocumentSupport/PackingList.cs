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
    public partial class PackingList : Form
    {
        #region PATH
        string PathTemPackingD = Application.StartupPath + @"\Template\M01_PackingList_Draft.xls";
        string PathTemPackingS = Application.StartupPath + @"\Template\M01_Q05_PackingList_Draft_Sum.xls";
        string PathOutPackingD = Application.StartupPath + @"\Output\M01_PackingList_Draft.xls";
        string PathOutPackingS = Application.StartupPath + @"\Output\M01_Q05_PackingList_Draft_Sum.xls";

        string PathImportPickingList = Application.StartupPath + @"\Import\PickingList.xls";


        string PathTemPackingFinal = Application.StartupPath + @"\Template\M02_PackingList_FINAL.xls";
        string PathTemPackingSummary = Application.StartupPath + @"\Template\Q06_PackingList_Summary_TFZ.xls";
        string PathTemPackingINBD = Application.StartupPath + @"\Template\Q08_PackingList_INBD_ED_LINE_NO.xls";
        string PathOutPackingFinal = Application.StartupPath + @"\Output\M02_PackingList_FINAL.xls";
        string PathOutPackingSummary = Application.StartupPath + @"\Output\Q06_PackingList_Summary_TFZ.xls";
        string PathOutPackingINBD = Application.StartupPath + @"\Output\Q08_PackingList_INBD_ED_LINE_NO.xls";
        #endregion

        public PackingList()
        {
            try
            {
                InitializeComponent();
                ResetDatagrid();
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E0301";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        private void ResetDatagrid()
        {

            try
            {
                string s_cmd;

                s_cmd = ComFunc.Q05_PackingList_Draft();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();
                }

                s_cmd = ComFunc.Q05_PackingList_Draft_Sum();
                DataTable dt1 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt1)
                {
                    dataGridView2.DataSource = dt1;
                    dataGridView2.ClearSelection();
                    lbl_Cnt.Text = dt1.Rows.Count.ToString();
                }

                s_cmd = ComFunc.Q06_PackingList_FINAL();
                DataTable dt2 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt2)
                {
                    dataGridView4.DataSource = dt2;
                    dataGridView4.ClearSelection();
                    lbl_Cnt.Text = dt2.Rows.Count.ToString();
                }


            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3101";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }
        private void ResetDatagridPackingDraft()
        {
            string s_cmd = "";
            s_cmd = ComFunc.Q05_PackingList_Draft();
            DataTable dt = ComFunc.ConnectDatabase(s_cmd);
            dataGridView1.DataSource = dt;
        }
        private void ResetDatagridPackingDraftSum()
        {
            string s_cmd = "";
            s_cmd = ComFunc.Q05_PackingList_Draft_Sum();
            DataTable dt = ComFunc.ConnectDatabase(s_cmd);
            dataGridView2.DataSource = dt;

        }

        private void Process_Packing()
        {
            Process_PackingDraft();
            Process_PackingFinal();
        }

        private void Process_PackingDraft()
        {

            DialogResult result = MessageBox.Show("Do you import Packing Draft?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {

                DeleteData();
                ImportPacking();
                string s_cmd = "";
                s_cmd = ComFunc.Q01_InBoundActual_DB();//Insert from T01 to T06
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q02_PickingList(); //SELECT T02
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q03_ReadBarcode_FNL();//SELECT T04
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q04_PackingListData();//SELECT T06
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q05_PackingList_Draft(); //SELECT T04
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    dataGridView1.DataSource = ComFunc.ConnectDatabase(s_cmd);
                    GenerateReportDraft();
                }

                s_cmd = ComFunc.Q05_PackingList_Draft_Sum();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    dataGridView2.DataSource = ComFunc.ConnectDatabase(s_cmd);
                    GenerateReportDraftSum();
                    lbl_Cnt.Text = dataGridView2.Rows.Count.ToString();

                }

                dataGridView1.Focus();
                MessageBox.Show("Import packing draft finished!", "Document Support");
                return;
            }

        }
       
        private void Process_PackingFinal()
        {

            DialogResult result = MessageBox.Show("Do you import Packing List Final?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                string s_cmd = "";
                s_cmd = ComFunc.Q06_PackingList_FINAL();//Insert to TI03
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportFinal();
                }

                s_cmd = ComFunc.Q06_PackingList_INBOUND_ED_NO();//Copy TI05 to TI02
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q07_TIFFA_LINE_NO();//Process TI01 to TI03
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.Q08_PackingList_INBD_ED_LINE_NO();//Process TI03 to TI04
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportSummary();
                    GenerateReportINBD();
                }

                s_cmd = ComFunc.Q06_PackingList_FINAL();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView4.DataSource = dt;
                    dataGridView4.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();
                }

                dataGridView4.Focus();
                MessageBox.Show("Import packing list final finished!", "Document Support");
                return;
            }

        }

        private void DeleteData()
        {
            try
            {
                string sql = "DELETE FROM [T06_InBoundActual_DB]";
                ComFunc.ConnectDatabase(sql);

                sql = "DELETE FROM [T02_PickingList]";
                ComFunc.ConnectDatabase(sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void ImportPacking()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            xlsCreator1.OpenBook(PathImportPickingList, "");
            string sql = string.Empty;

            ///
            List<int> list = new List<int> { };
            string[] data = new string[41];

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 40; j++)
                {
                    data[j] = xlsCreator1.Pos(j, i).Value.ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }


                }

                if (count == 40)
                {
                    break;
                }
                else
                {
                    sql = InsertPackingDraft(data, sql);
                }
            }

            ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
        }
        private string InsertPackingDraft(string[] data, string sql)
        {
            string Table = "T02_PickingList";
            string main = @"INSERT INTO " + Table + " Values";

            string sub = "(";
            int ColumnsCnt = 40;
            for (int i = 0; i < ColumnsCnt; i++)
            {
                sub += "'" + data[i] + "'";
                if (i != ColumnsCnt - 1)
                    sub += ",";
            }
            sub += ")";


            if (sql == string.Empty)
            {
                sql = main + sub;
            }
            else
            {
                sql += ", " + sub;
            }

            return sql;
        }



        private void GenerateReportDraft()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemPackingD, PathOutPackingD, true);
            XlsCreator1.OpenBook(PathOutPackingD, "");

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
            System.Diagnostics.Process.Start(PathOutPackingD);
        }
        private void GenerateReportDraftSum()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemPackingS, PathOutPackingS, true);
            XlsCreator1.OpenBook(PathOutPackingS, "");

            string sql = ComFunc.Q05_PackingList_Draft_Sum();
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
            System.Diagnostics.Process.Start(PathOutPackingS);
        }
        private void GenerateReportFinal()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemPackingFinal, PathOutPackingFinal, true);
            XlsCreator1.OpenBook(PathOutPackingFinal, "");

            string sql = ComFunc.Q06_PackingList_FINAL();
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
            System.Diagnostics.Process.Start(PathOutPackingFinal);
        }
        private void GenerateReportSummary()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemPackingSummary, PathOutPackingSummary, true);
            XlsCreator1.OpenBook(PathOutPackingSummary, "");

            string sql = ComFunc.Q06_PackingList_Summary_TFZ();
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
            System.Diagnostics.Process.Start(PathOutPackingSummary);
        }
        private void GenerateReportINBD()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemPackingINBD, PathOutPackingINBD, true);
            XlsCreator1.OpenBook(PathOutPackingINBD, "");

            string sql = ComFunc.Q08_PackingList_INBD_ED_LINE_NO();
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
            System.Diagnostics.Process.Start(PathOutPackingINBD);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetDatagrid();
        }
        private void btn_Packing_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "M01_PackingList_Draft", true);
        }
        private void btn_PackingSum_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView2, "M01_Q05_PackingList_Draft_Sum", true);
        }
        private void btn_PackingListFinal_Click(object sender, EventArgs e)
        {
            PackingListFinal f = new PackingListFinal();
            f.Show();
        }
        private void btn_import_Click(object sender, EventArgs e)
        {

            try
            {
                Process_Packing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error001!");
                return;
            }

        }
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

    


       

    }
}
