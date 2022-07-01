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

namespace DocumentSupport
{
    public partial class TFZReport : Form
    {

        public TFZReport()
        {
            try
            {
                InitializeComponent();
                ResetDatagrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Document Support");
                return;
            }
        }

        #region PATH
        string PathInBound = Application.StartupPath + @"\Import\InBoundActual.xls";
        string PathMoveIn = Application.StartupPath + @"\Import\TIFFA_Data_MoveIn.xls";
        string PathOutBound = Application.StartupPath + @"\Import\OutBoundActual.xls";
        string PathMoveOut = Application.StartupPath + @"\Import\TIFFA_Data_MoveOut.xls";
        string PathTemplateDupIn = Application.StartupPath + @"\Template\TI04_InBound_Duplicate_CheckMaster_Template.xls";
        string PathTemplateDupOut = Application.StartupPath + @"\Template\TO04_OutBound_Duplicate_CheckMaster_Template.xls";
        string PathOutputDupIn = Application.StartupPath + @"\Output\TI04_InBound_Duplicate_CheckMaster.xls";
        string PathOutputDupOut = Application.StartupPath + @"\Output\TIO04_OutBound_Duplicate_CheckMaster.xls";
        string PathStockBySum = Application.StartupPath + @"\Import\StockBySumCond.csv";

        #endregion

        //Datagridview
        private void ResetDatagrid()
        {
            ResetDatagridInbound();
            ResetDatagridOutbound();
            ResetDatagridStockBySum();
            ResetDatagridErrorList();
        }
        private void ResetDatagridInbound()
        {
            string sql = @"select * from TI05_InBoundActual_DB_Final";
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoGenerateColumns = false;
        }
        private void ResetDatagridOutbound()
        {
            string sql = @"select * from TO05_OutBoundActual_DB_Final";
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView2.DataSource = dt;
            dataGridView2.AutoGenerateColumns = false;
        }
        private void ResetDatagridStockBySum()
        {
            string sql = ComFunc.QS03_StockList_FINAL();
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView3.DataSource = dt;
            dataGridView3.AutoGenerateColumns = false;
        }
        private void ResetDatagridErrorList()
        {
            string sql = "select * from dbo.ErrorList";
            DataTable dt = ComFunc.ConnectDatabase(sql);
            dataGridView4.DataSource = dt;
            dataGridView4.AutoGenerateColumns = false;
        }

        //Main process
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
                string s_cmd = ComFunc.Q01_Add_InBoundActual_DB();//Insert to TI03
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI01_InBoundActual_KeyCode();//Copy TI03 to TI01
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                //Import with truncate
                ImportMoveIn();//Insert to TI05


                s_cmd = ComFunc.QI02_CustomsData_MoveIn_KeyCode();//Copy TI05 to TI02
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI03_InBoundActual_LineNo();//Process TI01 to TI03
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI04_Duplicate_Check();//Process TI03 to TI04
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.TI04_InBound_Duplicate_CheckMaster(); // Export TI04
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportInBound();
                }


                s_cmd = ComFunc.QI05_InBoundActual_LineNo_Check();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI06_Details_of_Export_MoveIn_WMS_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI07_Details_of_EXP_MoveIn_TIFFA_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QI08_Details_of_Export_MoveIn_Final(); //
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                string sql = @"select * from TI05_InBoundActual_DB_Final";
                DataTable dt = ComFunc.ConnectDatabase(sql);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();

                }

                dataGridView1.Focus();
                MessageBox.Show("Import inbound finished!", "Document Support");
            }

        }
        private void Process_Outbound()
        {
            DialogResult result = MessageBox.Show("Do you import outbound?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {

                string s_cmd;
                //Import 
                ImportOutbound();
                s_cmd = ComFunc.Q02_Add_OutBoundActual_DB();//Insert
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO01_OutBoundActual_KeyCode();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                //Import with truncate

                ImportMoveOut();
                s_cmd = ComFunc.QO02_CustomsData_MoveOut_KeyCode();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO03_OutBoundActual_LineNo();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO04_Duplicate_Check();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.TO04_OutBound_Duplicate_CheckMaster();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    GenerateReportOutBound();
                }

                s_cmd = ComFunc.QO05_OutBoundActual_LineNo_Check();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO06_Details_of_Export_MoveOut_WMS_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO07_Details_of_EXP_MoveOut_TIFFA_DB();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QO08_Details_of_Export_MoveOut_Final();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                string sql = @"select * from TO05_OutBoundActual_DB_Final";
                DataTable dt = ComFunc.ConnectDatabase(sql);
                if (null != dt)
                {
                    dataGridView2.DataSource = dt;
                    dataGridView2.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();

                }
                dataGridView2.Focus();
                MessageBox.Show("Import outbound finished!", "Document Support");
            }
        }
        private void Process_StockList()
        {
            DialogResult result = MessageBox.Show("Do you import stock list?", "Document support",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {

                string s_cmd = "";
                s_cmd = ComFunc.QIO01_InOutRecord_Draft();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QIO02_InOutRecord_Draft();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QIO03_InOutRecord_FINAL();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                ImportStockBySum();
                s_cmd = ComFunc.QS01_StockList_Draft();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QS02_StockList_FOB_Draft();
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QS03_StockList_FINAL();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView3.DataSource = dt;
                    dataGridView3.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();
                }
                dataGridView3.Focus();
                MessageBox.Show("Import stock list finished!", "Document support");

            }

        }

        //Clear data
        private void Clear_Inbound()
        {
            string s_cmd;

            s_cmd = "delete from T01_InBoundActual_Original";
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = "delete from T05_CustomsData_MoveIn_Original";
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI01_InBoundActual_KeyCode();//Copy TI03 to TI01
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI02_CustomsData_MoveIn_KeyCode();//Copy TI05 to TI02
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI03_InBoundActual_LineNo();//Process TI01 to TI03
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI04_Duplicate_Check();//Process TI03 to TI04
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.TI04_InBound_Duplicate_CheckMaster(); // Export TI04
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI05_InBoundActual_LineNo_Check();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
            s_cmd = ComFunc.QI06_Details_of_Export_MoveIn_WMS_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
            s_cmd = ComFunc.QI07_Details_of_EXP_MoveIn_TIFFA_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QI08_Details_of_Export_MoveIn_Final(); //
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
        }
        private void Clear_Outbound()
        {
            string s_cmd;
            s_cmd = ComFunc.QO01_OutBoundActual_KeyCode();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
            s_cmd = ComFunc.QO02_CustomsData_MoveOut_KeyCode();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO03_OutBoundActual_LineNo();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO04_Duplicate_Check();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.TO04_OutBound_Duplicate_CheckMaster();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO05_OutBoundActual_LineNo_Check();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO06_Details_of_Export_MoveOut_WMS_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO07_Details_of_EXP_MoveOut_TIFFA_DB();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QO08_Details_of_Export_MoveOut_Final();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }
        }
        private void Clear_Stocklist()
        {
            string s_cmd;
            s_cmd = ComFunc.QIO01_InOutRecord_Draft();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QIO02_InOutRecord_Draft();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QIO03_InOutRecord_FINAL();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QS01_StockList_Draft();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QS02_StockList_FOB_Draft();
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                string error_msg = @"System Error E1205";
                ComFunc.WriteLogLocal(error_msg, "");
            }

            s_cmd = ComFunc.QS03_StockList_FINAL();
            DataTable dt = ComFunc.ConnectDatabase(s_cmd);
            if (null != dt)
            {
                dataGridView3.DataSource = dt;
                dataGridView3.ClearSelection();
                lbl_Cnt.Text = dt.Rows.Count.ToString();
            }
        }
        private void Clear_Errorlist()
        {
            string s_cmd;
            s_cmd = "select * from ErrorList";
            DataTable dt = ComFunc.ConnectDatabase(s_cmd);
            if (null != dt)
            {
                dataGridView4.DataSource = dt;
                dataGridView4.ClearSelection();
                lbl_Cnt.Text = dt.Rows.Count.ToString();
            }
        }

        //Import function
        private void ImportInbound()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            xlsCreator1.OpenBook(PathInBound, "");
            string sql = string.Empty;

            ///
            List<int> list = new List<int> { };
            string[] data = new string[62];

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 61; j++)
                {
                    data[j] = xlsCreator1.Pos(j, i).Value.ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }
                    //
                    if (j == 1)
                    {
                        list.Add(ComFunc.ConvertInt(data[j]));
                        var distinctBytes = new HashSet<int>(list);
                        bool allDifferent = distinctBytes.Count == list.Count;
                        if (!allDifferent)
                        {
                            list.RemoveAt(list.Count - 1);
                            InsertErrorList(data[j], i, "Inbound Actual");
                        }
                    }

                    //
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
            ResetDatagridErrorList();
        }
        private void ImportMoveIn()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            xlsCreator1.OpenBook(PathMoveIn, "");
            string sql = string.Empty;

            List<int> list = new List<int> { };
            string[] data = new string[18];

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 18; j++)
                {
                    data[j] = (xlsCreator1.Pos(j, i).Value).ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }

                    if (j == 4)
                    {
                        list.Add(ComFunc.ConvertInt(data[j]));
                        var distinctBytes = new HashSet<int>(list);
                        bool allDifferent = distinctBytes.Count == list.Count ;
                       
                        if (!allDifferent)
                        {
                            list.RemoveAt(list.Count - 1);
                            InsertErrorList(data[j], i, "Move In");
                        }
                       
                        
                    }
                    
                }

                    if (count == 18)
                {
                    break;
                }
                else
                {
                    sql = InsertMoveIn(data, sql);
                }

            }

            if(sql != "")
                ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
            ResetDatagridErrorList();
        }
        private void ImportOutbound()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            string sql = string.Empty;
            xlsCreator1.OpenBook(PathOutBound, "");

            string[] data = new string[67];
            List<int> list = new List<int> { };

            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 67; j++)
                {
                    data[j] = (xlsCreator1.Pos(j, i).Value).ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }

                    if (j == 1)
                    {
                        list.Add(ComFunc.ConvertInt(data[j]));
                        var distinctBytes = new HashSet<int>(list);
                        bool allDifferent = distinctBytes.Count == list.Count;
                        if (!allDifferent)
                        {
                            list.RemoveAt(list.Count - 1);
                            InsertErrorList(data[j], i, "Outbound Actual");
                        }
                    }
                }
                if (count == 67)
                {
                    break;
                }
                else
                {
                    sql = InsertOutbound(data,sql);
                }
                
            }
            if (sql != "")
                ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
            ResetDatagridErrorList();
        }
        private void ImportMoveOut()
        {
            ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
            string sql = string.Empty;
            xlsCreator1.OpenBook(PathMoveOut, "");

            string[] data = new string[18];
            List<int> list = new List<int> { };


            for (int i = 1; i < 999; i++)
            {
                int count = 0;
                for (int j = 0; j < 18; j++)
                {
                    data[j] = (xlsCreator1.Pos(j, i).Value).ToString();
                    if (data[j] == "")
                    {
                        count++;
                    }
                    if (j == 4)
                    {
                        list.Add(ComFunc.ConvertInt(data[j]));
                        var distinctBytes = new HashSet<int>(list);
                        bool allDifferent = distinctBytes.Count == list.Count;
                        if (!allDifferent)
                        {
                            list.RemoveAt(list.Count - 1);
                            InsertErrorList(data[j], i, "Move Out");
                        }
                    }
                }
                
                if (count == 18)
                {
                    break;
                }
                else
                {
                    sql = InsertMoveOut(data, sql);
                }
                
            }
            if (sql != "")
                ComFunc.ConnectDatabase(sql);
            xlsCreator1.CloseBook(true);
            ResetDatagridErrorList();
        }
        private void ImportStockBySum()
        {
           
            DataTable File = (DataTable)ReadCSV(PathStockBySum);
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


        //Insert after import
        private string InsertInbound(string[] data, string sql)
        {
            string main = "INSERT INTO T01_InBoundActual_Original Values ";
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
                data[60] + "' )";



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
        private string InsertMoveIn(string[] data, string sql)
        {
            string main = @"INSERT INTO  T05_CustomsData_MoveIn_Original Values";
            string sub = "('" + data[0] + "', '" + data[1] + "','" + ComFunc.ConvertDouble(data[2]).ToString() + "', " +
                "'" + data[3] + "', '" + data[4] + "', " +
                "'" + ComFunc.ConvertDouble(data[5]).ToString() + "', '" + ComFunc.ConvertDouble(data[6]).ToString() + "', '" + data[7] + "', '" + ComFunc.ConvertDouble(data[8]).ToString() + "', '" + ComFunc.ConvertDouble(data[9]).ToString() + "', " +
                "'" + data[10] + "','" + ComFunc.ConvertDouble(data[11]).ToString() + "', '" + ComFunc.ConvertDouble(data[12]).ToString() + "', '" + ComFunc.ConvertDouble(data[13]).ToString() + "', " +
                "'" + data[14] + "', '" + ComFunc.ConvertDouble(data[15]).ToString() + "', '" + ComFunc.ConvertDouble(data[16]).ToString() + "', '" + ComFunc.ConvertDouble(data[17]).ToString() + "' )";

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

            string main = @"INSERT INTO T02_OutBoundActual_Original 
        Values ";
            string sub = "('" + data[0] + "'" +
      ",'" + ComFunc.ConvertDouble(data[1]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[2]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[3]) + "'" +
      ",'" + data[4] + "'" +
      ",'" + ComFunc.ConvertDouble(data[5]) + "'" +
      ",'" + data[6] + "'" +
      ",'" + data[7] + "'" +
      ",'" + data[8] + "'" +
      ",'" + data[9] + "'" +
      ",'" + data[10] + "'" +
      ",'" + data[11] + "'" +
      ",'" + ComFunc.ConvertDouble(data[12]) + "'" +
      ",'" + data[13] + "'" +
      ",'" + data[14] + "'" +
      ",'" + data[15] + "'" +
      ",'" + data[16] + "'" +
      ",'" + data[17] + "'" +
      ",'" + data[18] + "'" +
      ",'" + data[19] + "'" +
      ",'" + data[20] + "'" +
      ",'" + data[21] + "'" +
      ",'" + data[22] + "'" +
      ",'" + data[23] + "'" +
      ",'" + data[24] + "'" +
      ",'" + data[25] + "'" +
      ",'" + data[26] + "'" +
      ",'" + ComFunc.ConvertDouble(data[27]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[28]) + "'" +
      ",'" + data[29] + "'" +
      ",'" + ComFunc.ConvertDouble(data[30]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[31]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[32]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[33]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[34]) + "'" +
      ",'" + data[35] + "'" +
      ",'" + ComFunc.ConvertDouble(data[36]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[37]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[38]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[39]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[40]) + "'" +
      ",'" + data[41] + "'" +
      ",'" + ComFunc.ConvertDouble(data[42]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[43]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[44]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[45]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[46]) + "'" +
      ",'" + data[47] + "'" +
      ",'" + ComFunc.ConvertDouble(data[48]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[49]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[50]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[51]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[52]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[53]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[54]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[55]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[56]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[57]) + "'" +
      ",'" + data[58] + "'" +
      ",'" + ComFunc.ConvertDouble(data[59]) + "'" +
      ",'" + ComFunc.ConvertDouble(data[60]) + "'" +
      ",'" + data[61] + "'" +
      ",'" + data[62] + "'" +
      ",'" + data[63] + "'" +
      ",'" + data[64] + "'" +
      ",'" + data[65] + "'" +
      ",'" + data[66] +"')";
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
        private string InsertMoveOut(string[] data, string sql)
        {
            string main = @"INSERT INTO  T06_CustomsData_MoveOut_Original Values ";
            string sub = "('" + data[0] + "', '" + data[1] + "','" + ComFunc.ConvertDouble(data[2]).ToString() + "', " +
               "'" + data[3] + "', '" + data[4] + "', " +
               "'" + ComFunc.ConvertDouble(data[5]).ToString() + "', '" + ComFunc.ConvertDouble(data[6]).ToString() + "', '" + data[7] + "', '" + ComFunc.ConvertDouble(data[8]).ToString() + "', '" + ComFunc.ConvertDouble(data[9]).ToString() + "', " +
               "'" + data[10] + "','" + ComFunc.ConvertDouble(data[11]).ToString() + "', '" + ComFunc.ConvertDouble(data[12]).ToString() + "', '" + ComFunc.ConvertDouble(data[13]).ToString() + "', " +
               "'" + data[14] + "', '" + ComFunc.ConvertDouble(data[15]).ToString() + "', '" + ComFunc.ConvertDouble(data[16]).ToString() + "', '" + ComFunc.ConvertDouble(data[17]).ToString() + "' )";
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
        private void InsertErrorList(string data, int row, string file)
        {
            string sql = @"insert into dbo.ErrorList values ('"+data+"',"+row+",'"+file+"')";
            ComFunc.ConnectDatabase(sql);
        }

        //Export to excel
        private void GenerateReportInBound()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateDupIn, PathOutputDupIn, true);
            XlsCreator1.OpenBook(PathOutputDupIn, "");

            string sql = ComFunc.TI04_InBound_Duplicate_CheckMaster();
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;
            for (int i=0;i<dt.Rows.Count;i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                }
                RowCnt++;
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathOutputDupIn);
        }
        private void GenerateReportOutBound()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateDupOut, PathOutputDupOut, true);
            XlsCreator1.OpenBook(PathOutputDupOut, "");

            string sql = ComFunc.TO04_OutBound_Duplicate_CheckMaster();
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                }
                RowCnt++;
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathOutputDupOut);
        }

        #region Button function
        private void btn_import_inbound_Click(object sender, EventArgs e)
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
        private void btn_import_Outbound_Click(object sender, EventArgs e)
        {
            try
            {
                Process_Outbound();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error001!");
                return;
            }
        }
        private void btn_stockList_Click(object sender, EventArgs e)
        {
            Process_StockList();
        }
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            string s_cmd;
            if (true == chb_Inbound.Checked)
            {
                s_cmd = ComFunc.Q01_Delete_InBoundActual_DB((DateTime)until.Value);
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                Clear_Inbound();
            }

            if (true == chb_Outbound.Checked)
            {
                s_cmd = ComFunc.Q02_Delete_OutBoundActual_DB((DateTime)until.Value);
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                Clear_Outbound();
            }

            s_cmd = @"delete from T07_StockBySumCond_Original";
            ComFunc.ConnectDatabase(s_cmd);
            Clear_Stocklist();
            ResetDatagrid();

            s_cmd = @"delete from ErrorList";
            ComFunc.ConnectDatabase(s_cmd);
            Clear_Errorlist();
            ResetDatagrid();
            MessageBox.Show("Clear finished!");

        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ResetDatagrid();
        }
        private void btn_Inbound_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "TI04_InBound_Duplicate_CheckMaster", true);
        }
        private void btn_Outbound_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView2, "TO04_OutBound_Duplicate_CheckMaster", true);
        }
        private void StockList_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView3, "QS03_StockList_FINAL", true);
        }
        private void btn_OutputReport_Click(object sender, EventArgs e)
        {
            TFZOutputRe frm = new TFZOutputRe();
            frm.Show();
        }
        #endregion
        
    }
}
