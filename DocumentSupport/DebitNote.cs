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
using EO.Internal;

namespace DocumentSupport
{
    public partial class DebitNote : Form
    {

        public DebitNote()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E0301";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        string PathInBound = Application.StartupPath + @"\Import\InBoundActualDebit.xls";
        string PathOutBound = Application.StartupPath + @"\Import\OutBoundActualDebit.xls";

        string PathTemplateInBound = Application.StartupPath + @"\Template\QI02_InBoundActual_Daily_Summary.xls";
        string PathTemplateOutBound = Application.StartupPath + @"\Template\QO06_OutBound_DN_Data.xls";
        string PathTemplateInOut = Application.StartupPath + @"\Template\QR02_INOUT RECORD_TFZ.xls";
        string PathTemplateM3 = Application.StartupPath + @"\Template\QR03_OutBoundActual_M3_TFZ.xls";

        string PathOutInBound = Application.StartupPath + @"\Output\QI02_InBoundActual_Daily_Summary.xls";
        string PathOutOutBound = Application.StartupPath + @"\Output\QO06_OutBound_DN_Data.xls";
        string PathOutInOut = Application.StartupPath + @"\Output\QR02_INOUT RECORD_TFZ.xls";
        string PathOutM3 = Application.StartupPath + @"\Output\QR03_OutBoundActual_M3_TFZ.xls";

        private void Process_Inbound()
        {

            DialogResult result = MessageBox.Show("Do you import inbound?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                //Import with truncate
                ImportInbound();
                string s_cmd = "";

                s_cmd = ComFunc.Q01_InBoundActual_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI02_InBoundActual_Daily();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportInBound();
                }


                s_cmd = ComFunc.QI02_InBoundActual_Daily();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                }
                //Process_OutBound();
                dataGridView1.Focus();
                MessageBox.Show("Import inbound finished!", "Document Support");
            }

        }
        private void Process_OutBound()
        {
            DialogResult result = MessageBox.Show("Do you import Outbound?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {

                ImportOutbound();
                string s_cmd = "";
                s_cmd = ComFunc.QO01_OutBoundActual_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                s_cmd = ComFunc.QO02_OutBoundActual_Sum_FG();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO03_OutBoundActual_Sum_Parts();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO04_EXPORT_CUSTOMS();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO05_ExportEntry();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO06_OutBound_DN_Data(ut_from.Text, ut_from.Text);
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportOutBound();
                }

                string DateForm = "";
                string DateTo = "";
                if (Date_From.Value != null)
                {
                    DateTime? dateform = ComFunc.ConvertDate(Date_From.Value.ToString());
                    DateForm = dateform.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateForm = "2000-01-01";
                }

                if (Date_To.Value != null)
                {
                    DateTime? dateto = ComFunc.ConvertDate(Date_To.Value.ToString());
                    DateTo = dateto.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTo = DateTime.Now.ToString("yyyy-MM-dd");
                }

                s_cmd = ComFunc.QO06_OutBound_DN_Data(DateForm, DateTo); //from to
                DataTable dt1 = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt1)
                {
                    dataGridView2.DataSource = dt1;
                    dataGridView2.ClearSelection();
                }
                MessageBox.Show("Import outbound finished!", "Document Support");

            }
        }
        private void Process_InOut()
        {
            DialogResult result = MessageBox.Show("Do you import InOut?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                string s_cmd = "";
                string DateForm = "";
                string DateTo = "";
                if (Date_From.Value != null)
                {
                    DateTime? dateform = ComFunc.ConvertDate(Date_From.Value.ToString());
                    DateForm = dateform.Value.ToString("yyyy-MM-dd");
                    DateForm += " 00:00:00";
                }
                else
                {
                    DateForm = "2000-01-01 00:00:00";
                }

                if (Date_To.Value != null)
                {
                    DateTime? dateto = ComFunc.ConvertDate(Date_To.Value.ToString());
                    DateTo = dateto.Value.ToString("yyyy-MM-dd");
                    DateTo += " 23:59:59";
                }
                else
                {
                    DateTo = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTo += " 23:59:59";
                }

                s_cmd = ComFunc.QR01_InBoundActual_DB(DateForm, DateTo);
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportInOut();
                }
                s_cmd = ComFunc.QR03_OutBoundActual_M3_TFZ(DateForm, DateTo);
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportM3();
                }

                s_cmd = ComFunc.QR02_INOUT_RECORD_TFZ(DateForm, DateTo);
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView3.DataSource = dt;
                    dataGridView3.ClearSelection();
                }

                MessageBox.Show("Import inout finished!", "Document Support");
            }

        }

        private string InsertInbound(string[] data, string sql)
        {
            string main = "INSERT INTO TI02_InBoundActual_DB Values ";
            string sub = " ('" +
                data[0] + "', '" +
                data[1] + "', '" +
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
                data[39] + "', '" +
                data[40] + "', '" +
                data[41] + "', '" +
                data[42] + "', '" +
                data[43] + "', '" +
                data[44] + "', '" +
                data[45] + "', '" +
                data[46] + "', '" +
                data[47] + "', '" +
                data[48] + "', '" +
                data[49] + "', '" +
                data[50] + "', '" +
                data[51] + "', '" +
                data[52] + "', '" +
                data[53] + "', '" +
                data[54] + "', '" +
                data[55] + "', '" +
                data[56] + "', '" +
                data[57] + "', '" +
                data[58] + "', '" +
                data[59] + "' )";



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
        private string InsertOutbound(string[] data, string sql)
        {
            string main = "INSERT INTO TO02_OutBoundActual_DB Values ";
            string sub = " ('" +
                data[0] + "', '" +
                data[1] + "', '" +
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
                data[39] + "', '" +
                data[40] + "', '" +
                data[41] + "', '" +
                data[42] + "', '" +
                data[43] + "', '" +
                data[44] + "', '" +
                data[45] + "', '" +
                data[46] + "', '" +
                data[47] + "', '" +
                data[48] + "', '" +
                data[49] + "', '" +
                data[50] + "', '" +
                data[51] + "', '" +
                data[52] + "', '" +
                data[53] + "', '" +
                data[54] + "', '" +
                data[55] + "', '" +
                data[56] + "', '" +
                data[57] + "', '" +
                data[58] + "', '" +
                data[59] + "', '" +
                data[60] + "', '" +
                data[61] + "', '" +
                data[62] + "', '" +
                data[63] + "', '" +
                data[64] + "', '" +
                data[65] + "' )";

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

        private void GenerateReportInBound()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateInBound, PathOutInBound, true);
            XlsCreator1.OpenBook(PathOutInBound, "");

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
            System.Diagnostics.Process.Start(PathOutInBound);
        }
        private void GenerateReportOutBound()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateOutBound, PathOutOutBound, true);
            XlsCreator1.OpenBook(PathOutOutBound, "");

            string sql = ComFunc.QO06_OutBound_DN_Data();
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
            System.Diagnostics.Process.Start(PathOutOutBound);
        }
        private void GenerateReportInOut()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateInOut, PathOutInOut, true);
            XlsCreator1.OpenBook(PathOutInOut, "");

            string sql = ComFunc.QR02_INOUT_RECORD_TFZ(ut_from.Text, ut_from.Text);
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
            System.Diagnostics.Process.Start(PathOutInOut);
        }
        private void GenerateReportM3()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateM3, PathOutM3, true);
            XlsCreator1.OpenBook(PathOutM3, "");

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
            System.Diagnostics.Process.Start(PathOutM3);
        }

        private void ImportInbound()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            xlsCreator1.OpenBook(PathInBound, "");
            string sql = string.Empty;

            ///
            List<int> list = new List<int> { };
            string[] data = new string[60];

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 60; j++)
                {
                    data[j] = xlsCreator1.Pos(j, i).Value.ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }

                }

                if (count == 61)
                {
                    break;
                }
                else
                {
                    sql = InsertInbound(data, sql);
                }
            }

            ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
            //ResetDatagridErrorList();

        }
        private void ImportOutbound()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            string sql = string.Empty;
            xlsCreator1.OpenBook(PathOutBound, "");

            string[] data = new string[66];
            List<int> list = new List<int> { };

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 66; j++)
                {
                    data[j] = (xlsCreator1.Pos(j, i).Value).ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }

                }
                if (count == 67)
                {
                    break;
                }
                else
                {
                    sql = InsertOutbound(data, sql);
                }

            }

            ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
            //ResetDatagridErrorList();
        }

        private void ResetDatagrid()
        {
            ResetDatagridInbound();
            ResetDatagridOutbound();
            ResetDatagridDateChack();
        }
        private void ResetDatagridInbound()
        {
            string sql = ComFunc.QI02_InBoundActual_Daily();
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoGenerateColumns = false;
        }
        private void ResetDatagridOutbound()
        {
            string sql = ComFunc.QO06_OutBound_DN_Data(ut_from.Text, ut_from.Text);
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView2.DataSource = dt;
            dataGridView2.AutoGenerateColumns = false;
        }
        private void ResetDatagridDateChack()
        {
            string sql = ComFunc.QO08_Date_Check(ut_from.Text, ut_from.Text);
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView3.DataSource = dt;
            dataGridView3.AutoGenerateColumns = false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetDatagrid();
        }

        private void btn_PackingListFinal_Click(object sender, EventArgs e)
        {
            PackingListFinal f = new PackingListFinal();
            f.Show();
        }

        private void btn_DailySum_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "QI02_InBoundActual_Daily_Summary", true);
            
        }

        private void btn_DNData_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "QO06_OutBound_DN_Data", true);

        }

        private void btn_Transit_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "QO07_OutBound_DN_Transit", true);
            
        }

        private void bn_DateChk_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "QO08_Date Check", true);

        }

        private void btn_InOutActual_Click(object sender, EventArgs e)
        {
            InOutBoundActual f = new InOutBoundActual();
            f.Show();
        }

        private void Btn_import_Click(object sender, EventArgs e)
        {
            try
            {
                Process_Inbound();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error001!");
                return;
            }
        }

        private void Btn_importoutbound_Click(object sender, EventArgs e)
        {
            try
            {
                Process_OutBound();
                Process_InOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error001!");
                return;
            }
        }

        private void Clear_Inbound()
        {
            string s_cmd;

            s_cmd = ComFunc.QI03_InBoundActual_DB_Delete((DateTime)Date_To.Value);
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.Q01_InBoundActual_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI02_InBoundActual_Daily();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QR01_InBoundActual_DB(ut_from.Text, ut_from.Text);
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
        }

        private void Clear_Outbound()
        {
            string s_cmd;
            s_cmd = ComFunc.QO09_OutBoundActual_DB_Delete((DateTime)Date_To.Value);
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO01_OutBoundActual_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
            s_cmd = ComFunc.QO02_OutBoundActual_Sum_FG();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO03_OutBoundActual_Sum_Parts();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO04_EXPORT_CUSTOMS();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO05_ExportEntry();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO06_OutBound_DN_Data(ut_from.Text, ut_from.Text);
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QR03_OutBoundActual_M3_TFZ(ut_from.Text, ut_from.Text);
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
        }

        private void Delete_Out_Click(object sender, EventArgs e)
        {
            string s_cmd;
            if (true == chb_Inbound.Checked)
            {
                 Clear_Inbound();
            }

            if (true == chb_Outbound.Checked)
            {
                Clear_Outbound();
            }

            s_cmd = @"DELETE FROM ErrorList";
            ComFunc.ConnectDatabase(s_cmd);

            MessageBox.Show("Clear finished!");
        }
    }
}
