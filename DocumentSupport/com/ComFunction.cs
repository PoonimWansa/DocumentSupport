using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace ComFunction
{
    public class ComFunc
    {
        public static string s_CurrentPath = "";

        public static DataTable ConnectDatabase(string sCommand)

        {
            const string strConnection = "ConnectionString";

            SqlConnection conn = new SqlConnection();
            conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[strConnection].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(sCommand, conn);

            DataTable dataTable = new DataTable();

            try
            {
                // Fill the data table with select statement's query results:
                int recordsAffected = da.Fill(dataTable);

                if (recordsAffected > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        System.Console.WriteLine(dr[0]);
                    }
                }

                return dataTable;
            }
            catch (OleDbException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                ComFunc.WriteLogLocal("E901", e.ToString());

                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        //internal static string QR01_InBoundActual_DB(Form form, object to)
        //{
        //    throw new NotImplementedException();
        //}

        public static DateTime? ConvertDate(string str)
        {
            DateTime dt_out;
            if (true == DateTime.TryParse(str, out dt_out))
            {
                return dt_out;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertMoney(string str)
        {
            string r_str = "";
            double d = 0;
            double.TryParse(str, out d);
            r_str = ConvertMoney(d);
            return r_str;
        }

        public static string ConvertMoney(double d)
        {
            string r_str = "";

            System.Globalization.NumberFormatInfo numberFormatInfo = new System.Globalization.NumberFormatInfo();
            numberFormatInfo.NumberDecimalDigits = 2;
            r_str = d.ToString("N", numberFormatInfo);

            return r_str;
        }

        public static double ConvertDouble(string str)
        {
            double d;
            if (double.TryParse(str, out d))
            {
                return d;
            }
            else
            {
                return 0;
            }
        }

        public static int ConvertInt(string str)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }

        public static string ConvertStr(string str)
        {
            if (null != str)
            {
                return str.Trim();
            }
            else
            {
                return "";
            }
        }

        public static void WriteLogLocal(string err_cd, string str)
        {
            string s_file = ComFunc.s_CurrentPath + "/Log/" +
                            DateTime.Now.ToString("yyyyMMdd") + ".txt";

            str = DateTime.Now.ToString("yyyy/MM/dd") + " " +
                    DateTime.Now.ToString("HH:mm:ss") + " " +
                    err_cd + " : " +
                    str;
            StreamWriter writer = new StreamWriter(s_file, true);
            writer.WriteLine(str);
            writer.Close();
        }
        
        public static ExcelCreator.XlsCreator xlsCreator1 = new ExcelCreator.XlsCreator();
        public static void GenerateDatagridviewExcel(DataGridView grid, string filename, bool b_date)
        {
            try
            {
                // file name
                string s_DateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string _filename = filename;
                if (true == b_date)
                {
                    _filename = _filename + "_" + s_DateTime;
                }
                _filename = _filename + ".xls";

                // dialog setting
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = _filename;
                sfd.Filter = "EXCEL File(*.xls)|*.xls|All Files(*.*)|*.*";
                sfd.FilterIndex = 2;
                sfd.Title = "Import EXCEL file";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    xlsCreator1.CreateBook(sfd.FileName, 3, ExcelCreator.xlVersion.ver2003);

                    string _MASTER_ITEMVIEW = ConfigurationManager.AppSettings.Get("MASTER_ITEMVIEW");
                    string _MASTER_INVENTORY = ConfigurationManager.AppSettings.Get("MASTER_INVENTORY");
                    string _MASTER_ITEM = "";
                    if ("" != _MASTER_ITEMVIEW
                        && "PartsList" == filename)
                    {
                        _MASTER_ITEM = _MASTER_ITEMVIEW;
                    }
                    if ("" != _MASTER_INVENTORY
                        && "Inventory" == filename)
                    {
                        _MASTER_ITEM = _MASTER_INVENTORY;
                    }
                    string[] MASTER_ITEM = _MASTER_ITEM.Split(',');

                    int row = 0;
                    int col = 0;

                    // header.
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        string tmptype = grid.Columns[i].CellType.Name;
                        if (true == grid.Columns[i].Visible &&
                            "DataGridViewButtonCell" != grid.Columns[i].CellType.Name)
                        {
                            xlsCreator1.Pos(col, row).Str = grid.Columns[i].HeaderCell.Value.ToString();
                            col++;
                        }
                    }

                    col = 0;
                    row++;

                    // body.
                    foreach (DataGridViewRow dr in grid.Rows)
                    {
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            if (true == grid.Columns[i].Visible &&
                                "DataGridViewButtonCell" != grid.Columns[i].CellType.Name)
                            {
                                double d = 0;
                                if (double.TryParse(dr.Cells[i].Value.ToString(), out d))
                                {
                                    xlsCreator1.Pos(col, row).Value = d;
                                }
                                else
                                {
                                    xlsCreator1.Pos(col, row).Str = dr.Cells[i].Value.ToString();
                                }
                                col++;
                            }
                        }
                        col = 0;
                        row++;
                    }

                    xlsCreator1.CloseBook(true);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9028";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        public static void GenerateDatagridview(DataGridView grid, string filename, bool b_date)
        {
            GenerateDatagridviewExcel(grid, filename, b_date);

        }

        public static bool ImportDatagridviewEXCEL(DataGridView grid, string table)
        {
            try
            {
                bool b_ret = false;

                // dialog setting
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "EXCEL File(*.xls)|*.xls";
                ofd.FilterIndex = 1;
                ofd.Title = "Import EXCEL file";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    xlsCreator1.OpenBook(ofd.FileName, "");

                    int row = 0;
                    int col = 0;

                    // header.
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        if (true == grid.Columns[i].Visible &&
                                "DataGridViewButtonCell" != grid.Columns[i].CellType.Name)
                        {
                            string s_tmp1 = "";
                            string s_tmp2 = "";
                            s_tmp1 = xlsCreator1.Pos(col, row).Str;
                            s_tmp2 = grid.Columns[i].HeaderCell.Value.ToString();
                            if (s_tmp1 != s_tmp2)
                            {
                                xlsCreator1.CloseBook(true);
                                return false;
                            }
                            col++;
                        }
                    }

                    string s_cmd = "";

                    s_cmd = "DELETE FROM " + table;
                    if (null == ComFunc.ConnectDatabase(s_cmd))
                    {
                        ComFunc.WriteLogLocal("System Error E9033", "");
                        xlsCreator1.CloseBook(true);
                        return false;
                    }

                    b_ret = InsertMasterData(table, col);

                    xlsCreator1.CloseBook(true);

                    updateUpdateMaster(1);
                }
                return b_ret;
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9034";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
                return false;
            }
        }

        public static bool Import_Excel(DataGridView grid, string table, string path)
        {
            try
            {
                bool b_ret = false;
                xlsCreator1.OpenBook(path, "");

                int row = 0;
                int col = 0;

                // header.
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    if (true == grid.Columns[i].Visible &&
                            "DataGridViewButtonCell" != grid.Columns[i].CellType.Name)
                    {
                        string s_tmp1 = "";
                        string s_tmp2 = "";
                        s_tmp1 = xlsCreator1.Pos(col, row).Str;
                        s_tmp2 = grid.Columns[i].HeaderCell.Value.ToString();
                        if (s_tmp1 != s_tmp2)
                        {
                            xlsCreator1.CloseBook(true);
                            return false;
                        }
                        col++;
                    }
                }
                string s_cmd = "";


                s_cmd = "DELETE FROM " + table;
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    ComFunc.WriteLogLocal("System Error E9033", "");
                    xlsCreator1.CloseBook(true);
                    return false;
                }

                b_ret = InsertMasterData(table, col);
                

                if (true == b_ret)
                {
                    MessageBox.Show("ERROR");
                }

                xlsCreator1.CloseBook(true);

                updateUpdateMaster(1);



                return b_ret;
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9034";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
                return false;
            }
        }

        public static void updateUpdateMaster(int value)
        {
            try
            {
                string sql = @"UPDATE TB_M_SYSTEM SET [VALUE] = '" + value.ToString() + "' WHERE [NAME] = 'UP_MAS'";
                if (null == ComFunc.ConnectDatabase(sql))
                {
                    string error_msg = @"System Error E9037";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9038";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        public static bool InsertMasterData(string table, int cnt)
        {
            try
            {
                int row = 1;
                int col = 0;

                while (true)
                {
                    string s_cmd = "INSERT INTO " + table + " VALUES( ";
                    col = 0;

                    if ("" == xlsCreator1.Pos(col, row).Str.Trim())
                    {
                        break;
                    }

                    while (true)
                    {
                        string s_tmp = "";
                        s_tmp = xlsCreator1.Pos(col, row).Str.Trim();
                        s_tmp = s_tmp.Replace("'", "''");

                        if ("MSACCESS" == ConfigurationManager.AppSettings.Get("DBTYPE"))
                        {
                            s_cmd = s_cmd + "'" + s_tmp + "',";
                        }
                        else
                        {
                            if ("" != s_tmp && 0 == ComFunc.ConvertDouble(s_tmp))
                            {
                                s_cmd = s_cmd + "N'" + s_tmp + "',";
                            }
                            else
                            {
                                s_cmd = s_cmd + "'" + s_tmp + "',";
                            }
                        }
                        col++;
                        if (cnt <= col)
                        {
                            break;
                        }
                    }

                    s_cmd = s_cmd + ComFunc.DtNow() + "," + "'9999'," + ComFunc.DtNow() + "," + "'9999')";

                    if (null == ComFunc.ConnectDatabase(s_cmd))
                    {
                        ComFunc.WriteLogLocal("System Error E9029", "");
                        xlsCreator1.CloseBook(true);
                        return false;
                    }

                    row++;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9030";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
                return false;
            }
        }

        public static string DtNow()
        {
            string ret = "";
            if ("MSACCESS" == ConfigurationManager.AppSettings.Get("DBTYPE"))
            {
                ret = "Now()";
            }
            else
            {
                ret = "GETDATE()";
            }
            return ret;
        }

        public static bool TableDeleted(string tableName)
        {
            string s_cmd = "DELETE FROM " + tableName;
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                ComFunc.WriteLogLocal("System Error E9033", "");
                return false;
            }
            else
            {
                return true;
            }

        }

        public static bool TableTrucated(string tableName)
        {
            string s_cmd = "TRUNCATE TABLE " + tableName;
            if (null == ComFunc.ConnectDatabase(s_cmd))
            {
                ComFunc.WriteLogLocal("System Error E9033", "");
                return false;
            }
            else
            {
                return true;
            }

        }

        public static List<string> InsertItemMasterData(string table, int cnt)
        {
            try
            {
                List<string> sCommandList = new List<string>();

                int row = 1;
                int col = 0;

                //UpdateDTMaster();

                while (true)
                {
                    string s_cmd = "INSERT INTO " + table + " VALUES( ";
                    col = 0;

                    if ("" == xlsCreator1.Pos(col, row).Str.Trim())
                    {
                        break;
                    }

                    while (true)
                    {
                        string s_tmp = "";

                        switch (col)
                        {
                            case 0:
                                // ID.
                                s_tmp = xlsCreator1.Pos(col, row).Str.Trim().Replace("\n", "");
                                break;
                            default:
                                s_tmp = xlsCreator1.Pos(col, row).Str.Trim();
                                break;
                        }

                        s_tmp = s_tmp.Replace("'", "''");
                        s_tmp = ConvertForSQL(s_tmp);
                        s_cmd = s_cmd + s_tmp + ",";
                        col++;
                        if (cnt <= col)
                        {
                            break;
                        }
                    }

                    s_cmd = s_cmd + ComFunc.DtNow() + "," + "'9999'," + ComFunc.DtNow() + "," + "'9999');";

                    sCommandList.Add(s_cmd);

                    row++;
                }

                return sCommandList;
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E9032";
                ComFunc.WriteLogLocal(error_msg, ex.Message);
                return null;
            }
        }

        public static string ConvertForSQL(string str)
        {
            string r_str = "";

            if ("" == str)
            {
                r_str = "null";
            }
            else
            {
                if ("MSACCESS" == ConfigurationManager.AppSettings.Get("DBTYPE"))
                {
                    r_str = "'" + str + "'";
                }
                else
                {
                    r_str = "N'" + str + "'";
                }
            }

            return r_str;
        }

        #region Packing List SEAPS-T TFZ 
        //M01_PackingList_Draft
        public static string Q01_InBoundActual_DB()
        {
            string s_cmd = "INSERT INTO T06_InBoundActual_DB " +
                    "SELECT T01_InBoundActual.[PO NO] AS [SUP PO NO],  " +
                    "T01_InBoundActual.SUPLR,  " +
                    "T01_InBoundActual.[SUPLR NM], " +
                    "T01_InBoundActual.[ACTUAL DT] AS [IN ACT DT],  " +
                    "T01_InBoundActual.[PRT NO],  " +
                    "T01_InBoundActual.[CASE NO],  " +
                    "T01_InBoundActual.[PROD CAT],  " +
                    "T01_InBoundActual.[PROD GP],  " +
                    "T01_InBoundActual.[PROD GP NM],  " +
                    "T01_InBoundActual.LCTN,  " +
                    "T01_InBoundActual.BOI,  " +
                    "T01_InBoundActual.[LOT NO], " +
                    "T01_InBoundActual.[PJT CD],  " +
                    "T01_InBoundActual.[Consignee PO NO] AS [CON PO NO],  " +
                    "T01_InBoundActual.QTY4 AS QTY,  " +
                    "T01_InBoundActual.UNIT4 AS UNIT,  " +
                    "T01_InBoundActual.[WEIGTH4(kg)] AS [N/W],  " +
                    "T01_InBoundActual.[VOLUME4(m^3)] AS M3,  " +
                    "T01_InBoundActual.[TOTAL PCS],  " +
                    "T01_InBoundActual.ETD,  " +
                    "T01_InBoundActual.[Original Lot No],  " +
                    "T01_InBoundActual.[Inbound ED] AS [INBD ED NO]  " +
                    "FROM T01_InBoundActual; ";

            return s_cmd;
        }
        public static string Q01_Delete_InBoundActual_DB2(DateTime until)
        {
            string s_cmd;
            s_cmd = "DELETE FROM T06_InBoundActual_DB " +
                    "WHERE (((T06_InBoundActual_DB.[SUP PO NO])<='" + until.ToString("yyyyMMdd") + "')); ";
            return s_cmd;
        }
        public static string Q02_PickingList()
        {
           string s_cmd;
           s_cmd = "SELECT T02_PickingList.[PJT CD], " +
                    "T02_PickingList.[DELV NO] AS [INV NO],  "+
                    "T02_PickingList.LCTN,  "+
                    "T02_PickingList.[Consignee PO NO] AS [CON PO NO],  "+
                    "T02_PickingList.[PRT NO],  "+
                    "Count(T02_PickingList.[CASE NO]) AS CTN,  "+
                    "Sum(T02_PickingList.QTY4) AS QTY,  "+
                    "T02_PickingList.UNIT4 AS UNIT "+
                    "FROM T02_PickingList "+
                    "GROUP BY T02_PickingList.[PJT CD], T02_PickingList.[DELV NO], T02_PickingList.LCTN, T02_PickingList.[Consignee PO NO], T02_PickingList.[PRT NO], T02_PickingList.UNIT4 "+
                    "ORDER BY T02_PickingList.LCTN, T02_PickingList.[PRT NO]; ";
           return s_cmd;
        }
        public static string Q03_ReadBarcode_FNL()
        {
            string s_cmd;
            s_cmd = "SELECT T04_Shipping_Mark.Name AS [SHIP TO], T04_Shipping_Mark.INV AS [INV NO], T04_Shipping_Mark.PN AS [PLT NO], T03_ReadBarcode.[PLT NO] AS [PLT LOT NO], T03_ReadBarcode.[CASE NO] " +
                    "FROM T03_ReadBarcode LEFT JOIN T04_Shipping_Mark ON T03_ReadBarcode.[PLT NO] = T04_Shipping_Mark.LOT " +
                    "GROUP BY T04_Shipping_Mark.Name, T04_Shipping_Mark.INV, T04_Shipping_Mark.PN, T03_ReadBarcode.[PLT NO], T03_ReadBarcode.[CASE NO] "; //;
            return s_cmd;
        }
        public static string Q04_PackingListData()
        {
            string s_cmd;
            s_cmd = "SELECT T06_InBoundActual_DB.[SUP PO NO], T06_InBoundActual_DB.SUPLR, T06_InBoundActual_DB.[SUPLR NM], T06_InBoundActual_DB.[IN ACT DT], T06_InBoundActual_DB.[PRT NO], T06_InBoundActual_DB.[CASE NO], T06_InBoundActual_DB.[PROD CAT], T06_InBoundActual_DB.[PROD GP], T06_InBoundActual_DB.[PROD GP NM], T06_InBoundActual_DB.LCTN, T06_InBoundActual_DB.BOI, T06_InBoundActual_DB.[LOT NO], T06_InBoundActual_DB.[PJT CD], T06_InBoundActual_DB.[CON PO NO], T06_InBoundActual_DB.QTY, T06_InBoundActual_DB.UNIT, T06_InBoundActual_DB.[N/W], T06_InBoundActual_DB.M3, T06_InBoundActual_DB.[TOTAL PCS], T06_InBoundActual_DB.ETD, T06_InBoundActual_DB.[Original Lot No], T06_InBoundActual_DB.[INBD ED NO], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], Q03_ReadBarcode_FNL.[PLT LOT NO] "+
                    "FROM (" + ComFunc.Q03_ReadBarcode_FNL() + ") as Q03_ReadBarcode_FNL LEFT JOIN T06_InBoundActual_DB ON Q03_ReadBarcode_FNL.[CASE NO] = T06_InBoundActual_DB.[CASE NO] " +
                    "GROUP BY T06_InBoundActual_DB.[SUP PO NO], T06_InBoundActual_DB.SUPLR, T06_InBoundActual_DB.[SUPLR NM], T06_InBoundActual_DB.[IN ACT DT], T06_InBoundActual_DB.[PRT NO], T06_InBoundActual_DB.[CASE NO], T06_InBoundActual_DB.[PROD CAT], T06_InBoundActual_DB.[PROD GP], T06_InBoundActual_DB.[PROD GP NM], T06_InBoundActual_DB.LCTN, T06_InBoundActual_DB.BOI, T06_InBoundActual_DB.[LOT NO], T06_InBoundActual_DB.[PJT CD], T06_InBoundActual_DB.[CON PO NO], T06_InBoundActual_DB.QTY, T06_InBoundActual_DB.UNIT, T06_InBoundActual_DB.[N/W], T06_InBoundActual_DB.M3, T06_InBoundActual_DB.[TOTAL PCS], T06_InBoundActual_DB.ETD, T06_InBoundActual_DB.[Original Lot No], T06_InBoundActual_DB.[INBD ED NO], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], Q03_ReadBarcode_FNL.[PLT LOT NO]  ";
            return s_cmd;
        }      
        public static string Q05_PackingList_Draft()
        {
            string s_cmd;
            s_cmd = "SELECT Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN AS [FNL LCTN], Q04_PackingListData.LCTN AS [RCV LCTN], Count(Q04_PackingListData.[CASE NO]) AS CTN, Sum(Q04_PackingListData.QTY) AS QTY, Q04_PackingListData.UNIT, Q04_PackingListData.[CON PO NO], Q04_PackingListData.[PRT NO] "+
                    "FROM ("+Q04_PackingListData()+")" +
                    "as Q04_PackingListData LEFT JOIN T02_PickingList ON Q04_PackingListData.[CASE NO] = T02_PickingList.[CASE NO] "+
                    "GROUP BY Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN, Q04_PackingListData.LCTN, Q04_PackingListData.UNIT, Q04_PackingListData.[CON PO NO], Q04_PackingListData.[PRT NO] "+
                    "ORDER BY Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN, Q04_PackingListData.LCTN, Q04_PackingListData.[CON PO NO], Q04_PackingListData.[PRT NO]; ";
            return s_cmd;
        }
        public static string Q05_PackingList_Draft_Sum()
        {
            string s_cmd;
            s_cmd = "SELECT Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN AS [FNL LCTN], Q04_PackingListData.LCTN AS [RCV LCTN], Count(Q04_PackingListData.[CASE NO]) AS CTN, Sum(Q04_PackingListData.QTY) AS QTY, Q04_PackingListData.UNIT, Q04_PackingListData.[CON PO NO] "+
                    "FROM ("+Q04_PackingListData()+")"+
                    "as Q04_PackingListData LEFT JOIN T02_PickingList ON Q04_PackingListData.[CASE NO] = T02_PickingList.[CASE NO] " +
                    "GROUP BY Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN, Q04_PackingListData.LCTN, Q04_PackingListData.UNIT, Q04_PackingListData.[CON PO NO] "+
                    "ORDER BY Q04_PackingListData.[PJT CD], Q04_PackingListData.[INV NO], Q04_PackingListData.[PLT LOT NO], Q04_PackingListData.[PLT NO], T02_PickingList.LCTN, Q04_PackingListData.LCTN, Q04_PackingListData.[CON PO NO]; ";
            return s_cmd;
        }

        //M02_PackingList_FINAL
        public static string Q06_PackingList_FINAL()
        {
            string s_cmd;
            s_cmd = "SELECT T05_OutBoundActual.[ACTUAL DT], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO] AS [CON PO NO], T05_OutBoundActual.[LOT NO] AS [SUP PO NO], T05_OutBoundActual.[PRT NO], Count(T05_OutBoundActual.[CASE NO]) AS CTN, " +
                    "Sum(T05_OutBoundActual.QTY4) AS QTY,  " +
                    "Round(Sum([T07_ProcuctMaster].[N/W]/[T07_ProcuctMaster].[QTY/UNIT3]*[T05_OutBoundActual].[QTY4]),2) AS [N/W Amend],  " +
                    "Round(Sum([T07_ProcuctMaster].[G/W]-[T07_ProcuctMaster].[N/W]+[T07_ProcuctMaster].[N/W]/[T07_ProcuctMaster].[QTY/UNIT3]*[T05_OutBoundActual].[QTY4]),2) AS [G/W Amend],  " +
                    "Sum(T05_OutBoundActual.[VOLUME4(m^3)]) AS M3,  " +
                    "Q03_ReadBarcode_FNL.[PLT LOT NO], T05_OutBoundActual.LCTN,  " +
                    "Sum(T05_OutBoundActual.WEIGTH4) AS [N/W],  " +
                    "Sum(T05_OutBoundActual.[Gross Weight]) AS [G/W], T05_OutBoundActual.UNIT4 AS UNIT " +
                    "FROM (T05_OutBoundActual INNER JOIN  " +
                    "(" + Q03_ReadBarcode_FNL() +
                    ")" +
                    "as Q03_ReadBarcode_FNL ON T05_OutBoundActual.[CASE NO] = Q03_ReadBarcode_FNL.[CASE NO])  " +
                    "LEFT JOIN T07_ProcuctMaster ON T05_OutBoundActual.[PRT NO] = T07_ProcuctMaster.[PRT NO] " +
                    "GROUP BY T05_OutBoundActual.[ACTUAL DT], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[LOT NO], T05_OutBoundActual.[PRT NO], Q03_ReadBarcode_FNL.[PLT LOT NO], T05_OutBoundActual.LCTN, T05_OutBoundActual.UNIT4 " +
                    "ORDER BY Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[PRT NO]; ";


            return s_cmd;
        }
        public static string Q06_PackingList_INBOUND_ED_NO()
        {
            string s_cmd;
            s_cmd = "INSERT INTO T10_PackingList_INBOUND_ED_NO "+
                    "SELECT CONCAT([T05_OutBoundActual].[Inbound ED],'-',[T05_OutBoundActual].[LOT NO],'-',[T05_OutBoundActual].[PRT NO]) AS [TIFFA ID],  "+
                    "T05_OutBoundActual.[PJT CD], T05_OutBoundActual.[DELV NO] AS [INVOICE NO],  "+
                    "T05_OutBoundActual.[PO NO] AS [CON PO NO], T05_OutBoundActual.[LOT NO] AS [SUP PO NO],  "+
                    "T05_OutBoundActual.[Inbound ED], T05_OutBoundActual.[PRT NO],  "+
                    "Sum(T05_OutBoundActual.QTY4) AS QTY, T05_OutBoundActual.UNIT4 AS UNIT   "+
                    "FROM T05_OutBoundActual "+
                    "GROUP BY CONCAT([T05_OutBoundActual].[Inbound ED], '-',[T05_OutBoundActual].[LOT NO],'-',[T05_OutBoundActual].[PRT NO]),  "+
                    "T05_OutBoundActual.[PJT CD], T05_OutBoundActual.[DELV NO], T05_OutBoundActual.[PO NO],  "+
                    "T05_OutBoundActual.[LOT NO], T05_OutBoundActual.[Inbound ED], T05_OutBoundActual.[PRT NO], T05_OutBoundActual.UNIT4 "+
                    "ORDER BY T05_OutBoundActual.[DELV NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[LOT NO],  "+
                    "T05_OutBoundActual.[Inbound ED], T05_OutBoundActual.[PRT NO]; ";
            return s_cmd;
        }
        public static string Q06_PackingList_Summary_TFZ()
        {
            string s_cmd;
            s_cmd = "SELECT T05_OutBoundActual.[DELV NO] AS [INVOICE NO], T05_OutBoundActual.[PO NO] AS [CON PO NO], T05_OutBoundActual.[LOT NO] AS [SUP PO NO], 1 AS [Inbound ED], T05_OutBoundActual.[PRT NO], Sum(T05_OutBoundActual.QTY4) AS QTY, T05_OutBoundActual.UNIT4 AS UNIT "+
                    "FROM T05_OutBoundActual "+
                    "GROUP BY T05_OutBoundActual.[DELV NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[LOT NO], T05_OutBoundActual.[PRT NO], T05_OutBoundActual.UNIT4 "+
                    "ORDER BY T05_OutBoundActual.[PO NO], T05_OutBoundActual.[LOT NO], T05_OutBoundActual.[PRT NO]; ";
            return s_cmd;
        }
        public static string Q07_TIFFA_LINE_NO()
        {
            //INTO T09_InBound_LINE_NO
            string s_cmd;
            s_cmd = "INSERT INTO T09_InBound_LINE_NO " +
                    "SELECT CONCAT([T08_TIFFA_Data_InBound].[DECLRATION NO],'-',[T08_TIFFA_Data_InBound].[PO NO],'-',[T08_TIFFA_Data_InBound].[PART NO]) AS [TIFFA ID], T08_TIFFA_Data_InBound.[INVOICE NO], T08_TIFFA_Data_InBound.[PO NO], T08_TIFFA_Data_InBound.[PART NO], T08_TIFFA_Data_InBound.[DECLRATION NO], T08_TIFFA_Data_InBound.[LINE NO] " +
                    "FROM T08_TIFFA_Data_InBound " +
                    "GROUP BY CONCAT([T08_TIFFA_Data_InBound].[DECLRATION NO],'-',[T08_TIFFA_Data_InBound].[PO NO],'-',[T08_TIFFA_Data_InBound].[PART NO]), T08_TIFFA_Data_InBound.[INVOICE NO], T08_TIFFA_Data_InBound.[PO NO], T08_TIFFA_Data_InBound.[PART NO], T08_TIFFA_Data_InBound.[DECLRATION NO], T08_TIFFA_Data_InBound.[LINE NO] " +
                    "HAVING (((CONCAT([T08_TIFFA_Data_InBound].[DECLRATION NO],'-',[T08_TIFFA_Data_InBound].[PO NO],'-',[T08_TIFFA_Data_InBound].[PART NO])) NOT LIKE '%PALLET%')); ";
            return s_cmd;
        }
        public static string Q08_PackingList_INBD_ED_LINE_NO()
        {
            string s_cmd;
            s_cmd = "SELECT T10_PackingList_INBOUND_ED_NO.[INVOICE NO], T10_PackingList_INBOUND_ED_NO.[CON PO NO], T10_PackingList_INBOUND_ED_NO.[SUP PO NO], T10_PackingList_INBOUND_ED_NO.[PRT NO], Sum(T10_PackingList_INBOUND_ED_NO.QTY) AS QTY, T10_PackingList_INBOUND_ED_NO.UNIT, T10_PackingList_INBOUND_ED_NO.[Inbound ED], T09_InBound_LINE_NO.[LINE NO], T09_InBound_LINE_NO.[INVOICE NO] AS [INBD INV NO] " +
                    "FROM T10_PackingList_INBOUND_ED_NO LEFT JOIN T09_InBound_LINE_NO ON T10_PackingList_INBOUND_ED_NO.[TIFFA ID] = T09_InBound_LINE_NO.[TIFFA ID] " +
                    "GROUP BY T10_PackingList_INBOUND_ED_NO.[INVOICE NO], T10_PackingList_INBOUND_ED_NO.[CON PO NO], T10_PackingList_INBOUND_ED_NO.[SUP PO NO], T10_PackingList_INBOUND_ED_NO.[PRT NO], T10_PackingList_INBOUND_ED_NO.UNIT, T10_PackingList_INBOUND_ED_NO.[Inbound ED], T09_InBound_LINE_NO.[LINE NO], T09_InBound_LINE_NO.[INVOICE NO] " +
                    "ORDER BY T10_PackingList_INBOUND_ED_NO.[CON PO NO], T10_PackingList_INBOUND_ED_NO.[PRT NO]; ";
            return s_cmd;
        }
        public static string ErrorlistPacking()
        {
            string s_cmd;
            s_cmd = "SELECT* from ErrorListPacking";
            return s_cmd;
        }
        //
        public static string T05_PackingList_Draft()
        {
            string s_cmd;
            s_cmd = "SELECT * FROM T05_PackingList_Draft";
            return s_cmd;
        }
        public static string T05_PackingList_Draft_Sum()
        {
            string s_cmd;
            s_cmd = "SELECT * FROM T05_PackingList_Draft_Sum";
            return s_cmd;
        }
        public static string T05_PackingList_FINAL()
        {
            string s_cmd;
            s_cmd = "SELECT T05_OutBoundActual.[ACTUAL DT], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO] AS [CON PO NO], T05_OutBoundActual.[LOT NO] AS [SUP PO NO], T05_OutBoundActual.[PRT NO], Count(T05_OutBoundActual.[CASE NO]) AS CTN, " +
                    "Sum(T05_OutBoundActual.QTY4) AS QTY,  " +
                    "Round(Sum([T07_ProcuctMaster].[N/W]/[T07_ProcuctMaster].[QTY/UNIT3]*[T05_OutBoundActual].[QTY4]),2) AS [N/W Amend],  " +
                    "Round(Sum([T07_ProcuctMaster].[G/W]-[T07_ProcuctMaster].[N/W]+[T07_ProcuctMaster].[N/W]/[T07_ProcuctMaster].[QTY/UNIT3]*[T05_OutBoundActual].[QTY4]),2) AS [G/W Amend],  " +
                    "Sum(T05_OutBoundActual.[VOLUME4(m^3)]) AS M3,  " +
                    "Q03_ReadBarcode_FNL.[PLT LOT NO], T05_OutBoundActual.LCTN,  " +
                    "Sum(T05_OutBoundActual.WEIGTH4) AS [N/W],  " +
                    "Sum(T05_OutBoundActual.[Gross Weight]) AS [G/W], T05_OutBoundActual.UNIT4 AS UNIT " +
                    "FROM (T05_OutBoundActual INNER JOIN  " +
                    "(" + Q03_ReadBarcode_FNL() +
                    ")" +
                    "as Q03_ReadBarcode_FNL ON T05_OutBoundActual.[CASE NO] = Q03_ReadBarcode_FNL.[CASE NO])  " +
                    "LEFT JOIN T07_ProcuctMaster ON T05_OutBoundActual.[PRT NO] = T07_ProcuctMaster.[PRT NO] " +
                    "GROUP BY T05_OutBoundActual.[ACTUAL DT], Q03_ReadBarcode_FNL.[INV NO], Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[LOT NO], T05_OutBoundActual.[PRT NO], Q03_ReadBarcode_FNL.[PLT LOT NO], T05_OutBoundActual.LCTN, T05_OutBoundActual.UNIT4 " +
                    "ORDER BY Q03_ReadBarcode_FNL.[PLT NO], T05_OutBoundActual.[PO NO], T05_OutBoundActual.[PRT NO]; ";


            return s_cmd;
        }
        #endregion

        #region DN SEAPS-T TFZ
        //M001_IMPORT InBoundActual DATA
        public static string QI01_InBoundActual_DB()
        {
            string s_cmd;
            s_cmd = "INSERT INTO TI02_InBoundActual_DB ( [INBD NO], [ROW NO], [DETAIL NO], [INBD CAT], [INBD CAT NM], [PO DT], [PO NO], [PLT NO], SUPLR, [SUPLR NM], REMARK, [ACTUAL DT], [PRT NO], [PRT NM1], [PRT NM2], [CASE NO], [PROD CAT], [PROD CAT NM], [PROD GP], [PROD GP NM], LCTN, BOI, [BOI NM], [LOT NO], [PJT CD], [PJT NM], [Consignee PO NO], QTY1, UNIT1, [WEIGTH1(kg)], [VOLUME1(m^3)], [INBD CHG1], [STOR CHG1], QTY2, UNIT2, [WEIGTH2(kg)], [VOLUME2(m^3)], [INBD CHG2], [STOR CHG2], QTY3, UNIT3, [WEIGTH3(kg)], [VOLUME3(m^3)], [INBD CHG3], [STOR CHG3], QTY4, UNIT4, [WEIGTH4(kg)], [VOLUME4(m^3)], [INBD CHG4], [STOR CHG4], [TOTAL PCS], [QTY1(PLAN)], [QTY2(PLAN)], [QTY3(PLAN)], [QTY4(PLAN)], [TOTAL PCS(PLAN)], ETD, [Original Lot No], [Inbound ED] ) "+
                    "SELECT TI01_InBoundActual_Original.[INBD NO], TI01_InBoundActual_Original.[ROW NO], TI01_InBoundActual_Original.[DETAIL NO], TI01_InBoundActual_Original.[INBD CAT], TI01_InBoundActual_Original.[INBD CAT NM], TI01_InBoundActual_Original.[PO DT], TI01_InBoundActual_Original.[PO NO], TI01_InBoundActual_Original.[PLT NO], TI01_InBoundActual_Original.SUPLR, TI01_InBoundActual_Original.[SUPLR NM], TI01_InBoundActual_Original.REMARK, TI01_InBoundActual_Original.[ACTUAL DT], TI01_InBoundActual_Original.[PRT NO], TI01_InBoundActual_Original.[PRT NM1], TI01_InBoundActual_Original.[PRT NM2], TI01_InBoundActual_Original.[CASE NO], TI01_InBoundActual_Original.[PROD CAT], TI01_InBoundActual_Original.[PROD CAT NM], TI01_InBoundActual_Original.[PROD GP], TI01_InBoundActual_Original.[PROD GP NM], TI01_InBoundActual_Original.LCTN, TI01_InBoundActual_Original.BOI, TI01_InBoundActual_Original.[BOI NM], TI01_InBoundActual_Original.[LOT NO], TI01_InBoundActual_Original.[PJT CD], TI01_InBoundActual_Original.[PJT NM], TI01_InBoundActual_Original.[Consignee PO NO], TI01_InBoundActual_Original.QTY1, TI01_InBoundActual_Original.UNIT1, TI01_InBoundActual_Original.[WEIGTH1(kg)], TI01_InBoundActual_Original.[VOLUME1(m^3)], TI01_InBoundActual_Original.[INBD CHG1], TI01_InBoundActual_Original.[STOR CHG1], TI01_InBoundActual_Original.QTY2, TI01_InBoundActual_Original.UNIT2, TI01_InBoundActual_Original.[WEIGTH2(kg)], TI01_InBoundActual_Original.[VOLUME2(m^3)], TI01_InBoundActual_Original.[INBD CHG2], TI01_InBoundActual_Original.[STOR CHG2], TI01_InBoundActual_Original.QTY3, TI01_InBoundActual_Original.UNIT3, TI01_InBoundActual_Original.[WEIGTH3(kg)], TI01_InBoundActual_Original.[VOLUME3(m^3)], TI01_InBoundActual_Original.[INBD CHG3], TI01_InBoundActual_Original.[STOR CHG3], TI01_InBoundActual_Original.QTY4, TI01_InBoundActual_Original.UNIT4, TI01_InBoundActual_Original.[WEIGTH4(kg)], TI01_InBoundActual_Original.[VOLUME4(m^3)], TI01_InBoundActual_Original.[INBD CHG4], TI01_InBoundActual_Original.[STOR CHG4], TI01_InBoundActual_Original.[TOTAL PCS], TI01_InBoundActual_Original.[QTY1(PLAN)], TI01_InBoundActual_Original.[QTY2(PLAN)], TI01_InBoundActual_Original.[QTY3(PLAN)], TI01_InBoundActual_Original.[QTY4(PLAN)], TI01_InBoundActual_Original.[TOTAL PCS(PLAN)], TI01_InBoundActual_Original.ETD, TI01_InBoundActual_Original.[Original Lot No], TI01_InBoundActual_Original.[Inbound ED] "+
                    "FROM TI01_InBoundActual_Original;";
            return s_cmd;
        }

        //M002_IMPORT OutBoundActual DATA
        public static string QO01_OutBoundActual_DB()
        {
            string s_cmd;
            s_cmd = "INSERT INTO TO02_OutBoundActual_DB ( [OUTBD NO], [ROW NO], [DETAIL NO], [OUTBD CAT], [OUTBD CAT NM], [PO DT], [PO NO], [PLT NO], [DELV CD], [DELV NM], [DELV NO], REMARK, [ACTUAL DT], [PRT NO], [PRT NM1], [PRT NM2], [CASE NO], [PROD CAT], [PROD CAT NM], [PROD GP], [PROD GP NM], LCTN, BOI, [BOI NM], [LOT NO], [PJT CD], [PJT NM], [INBD DT], QTY1, UNIT1, WEIGTH1, [VOLUME1(m^3)], [OUTBD CHG1], [STOR CHG1], QTY2, UNIT2, WEIGTH2, [VOLUME2(m^3)], [OUTBD CHG2], [STOR CHG2], QTY3, UNIT3, WEIGTH3, [VOLUME3(m^3)], [OUTBD CHG3], [STOR CHG3], QTY4, UNIT4, WEIGTH4, [VOLUME4(m^3)], [OUTBD CHG4], [STOR CHG4], [TOTAL PCS], [QTY1(PLAN)], [QTY2(PLAN)], [QTY3(PLAN)], [QTY4(PLAN)], [TOTAL PCS(PLAN)], [ITEM SECTION], [QTY/UNIT(3)], [Gross Weight], [Box Size], COO, [Original Lot No], [Inbound ED], [Outbound ED] ) "+
                    "SELECT TO01_OutBoundActual_Original.[OUTBD NO], TO01_OutBoundActual_Original.[ROW NO], TO01_OutBoundActual_Original.[DETAIL NO], TO01_OutBoundActual_Original.[OUTBD CAT], TO01_OutBoundActual_Original.[OUTBD CAT NM], TO01_OutBoundActual_Original.[PO DT], TO01_OutBoundActual_Original.[PO NO], TO01_OutBoundActual_Original.[PLT NO], TO01_OutBoundActual_Original.[DELV CD], TO01_OutBoundActual_Original.[DELV NM], TO01_OutBoundActual_Original.[DELV NO], TO01_OutBoundActual_Original.REMARK, TO01_OutBoundActual_Original.[ACTUAL DT], TO01_OutBoundActual_Original.[PRT NO], TO01_OutBoundActual_Original.[PRT NM1], TO01_OutBoundActual_Original.[PRT NM2], TO01_OutBoundActual_Original.[CASE NO], TO01_OutBoundActual_Original.[PROD CAT], TO01_OutBoundActual_Original.[PROD CAT NM], TO01_OutBoundActual_Original.[PROD GP], TO01_OutBoundActual_Original.[PROD GP NM], TO01_OutBoundActual_Original.LCTN, TO01_OutBoundActual_Original.BOI, TO01_OutBoundActual_Original.[BOI NM], TO01_OutBoundActual_Original.[LOT NO], TO01_OutBoundActual_Original.[PJT CD], TO01_OutBoundActual_Original.[PJT NM], TO01_OutBoundActual_Original.[INBD DT], TO01_OutBoundActual_Original.QTY1, TO01_OutBoundActual_Original.UNIT1, TO01_OutBoundActual_Original.WEIGTH1, TO01_OutBoundActual_Original.[VOLUME1(m^3)], TO01_OutBoundActual_Original.[OUTBD CHG1], TO01_OutBoundActual_Original.[STOR CHG1], TO01_OutBoundActual_Original.QTY2, TO01_OutBoundActual_Original.UNIT2, TO01_OutBoundActual_Original.WEIGTH2, TO01_OutBoundActual_Original.[VOLUME2(m^3)], TO01_OutBoundActual_Original.[OUTBD CHG2], TO01_OutBoundActual_Original.[STOR CHG2], TO01_OutBoundActual_Original.QTY3, TO01_OutBoundActual_Original.UNIT3, TO01_OutBoundActual_Original.WEIGTH3, TO01_OutBoundActual_Original.[VOLUME3(m^3)], TO01_OutBoundActual_Original.[OUTBD CHG3], TO01_OutBoundActual_Original.[STOR CHG3], TO01_OutBoundActual_Original.QTY4, TO01_OutBoundActual_Original.UNIT4, TO01_OutBoundActual_Original.WEIGTH4, TO01_OutBoundActual_Original.[VOLUME4(m^3)], TO01_OutBoundActual_Original.[OUTBD CHG4], TO01_OutBoundActual_Original.[STOR CHG4], TO01_OutBoundActual_Original.[TOTAL PCS], TO01_OutBoundActual_Original.[QTY1(PLAN)], TO01_OutBoundActual_Original.[QTY2(PLAN)], TO01_OutBoundActual_Original.[QTY3(PLAN)], TO01_OutBoundActual_Original.[QTY4(PLAN)], TO01_OutBoundActual_Original.[TOTAL PCS(PLAN)], TO01_OutBoundActual_Original.[ITEM SECTION], TO01_OutBoundActual_Original.[QTY/UNIT(3)], TO01_OutBoundActual_Original.[Gross Weight], TO01_OutBoundActual_Original.[Box Size], TO01_OutBoundActual_Original.COO, TO01_OutBoundActual_Original.[Original Lot No], TO01_OutBoundActual_Original.[Inbound ED], TO01_OutBoundActual_Original.[Outbound ED] "+
                    "FROM TO01_OutBoundActual_Original; ";

            return s_cmd;
        }

        //M003_EXPORT InBound Transit Data
        public static string QI02_InBoundActual_Daily()
        {
            string s_cmd;
            s_cmd = "SELECT TI02_InBoundActual_DB.[ACTUAL DT], Count(TI02_InBoundActual_DB.[CASE NO]) AS [TOTAL CTN] "+
                    "FROM TI02_InBoundActual_DB "+
                    "GROUP BY TI02_InBoundActual_DB.[ACTUAL DT]";
            return s_cmd;
        }

        //M004_EXPORT OutBound DN Transit Data
        public static string QO02_OutBoundActual_Sum_FG()
        {
            string s_cmd;
            s_cmd = "SELECT TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO] AS [INV NO], TO02_OutBoundActual_DB.[PROD CAT], TO02_OutBoundActual_DB.[PRT NO], Sum(TO02_OutBoundActual_DB.QTY4) AS QTY "+
                    "FROM TO02_OutBoundActual_DB "+
                    "GROUP BY TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO], TO02_OutBoundActual_DB.[PROD CAT], TO02_OutBoundActual_DB.[PRT NO] "+
                    "HAVING (((TO02_OutBoundActual_DB.[PROD CAT])='FG EXP')) "+
                    "ORDER BY TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO]; ";

            return s_cmd;
        }
        public static string QO03_OutBoundActual_Sum_Parts()
        {
            string s_cmd;
            s_cmd = "SELECT TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO] AS [INV NO], Count(TO02_OutBoundActual_DB.[CASE NO]) AS CTN, TO02_OutBoundActual_DB.[PROD CAT] "+
                    "FROM TO02_OutBoundActual_DB "+
                    "GROUP BY TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO], TO02_OutBoundActual_DB.[PROD CAT] "+
                    "HAVING (((TO02_OutBoundActual_DB.[PROD CAT])<>'FG EXP')) "+
                    "ORDER BY TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO]; ";
            return s_cmd;
        }
        public static string QO04_EXPORT_CUSTOMS()
        {
            string s_cmd;
            s_cmd = "SELECT TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO] AS [INV NO], CONCAT([TO02_OutBoundActual_DB].[PO NO],'-',[TO02_OutBoundActual_DB].[PRT NO]) AS [KEY CD] "+
                    "FROM TO02_OutBoundActual_DB "+
                    "GROUP BY TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO], CONCAT([TO02_OutBoundActual_DB].[PO NO],'-',[TO02_OutBoundActual_DB].[PRT NO]), TO02_OutBoundActual_DB.[PROD CAT] "+
                    "HAVING (((TO02_OutBoundActual_DB.[PROD CAT])<>'FG EXP')) "+
                    "ORDER BY TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[DELV NO]; ";
            return s_cmd;
        }
        public static string QO05_ExportEntry()
        {
            string s_cmd;
            s_cmd = "SELECT [TO05_EXPORT CUSTOMS].[ACTUAL DT], [TO05_EXPORT CUSTOMS].[PJT CD], [TO05_EXPORT CUSTOMS].[INV NO], Count([TO05_EXPORT CUSTOMS].[KEY CD]) AS Entry "+
                    "FROM [TO05_EXPORT CUSTOMS] "+
                    "GROUP BY [TO05_EXPORT CUSTOMS].[ACTUAL DT], [TO05_EXPORT CUSTOMS].[PJT CD], [TO05_EXPORT CUSTOMS].[INV NO] "+
                    "ORDER BY [TO05_EXPORT CUSTOMS].[ACTUAL DT], [TO05_EXPORT CUSTOMS].[PJT CD], [TO05_EXPORT CUSTOMS].[INV NO]; ";
            return s_cmd;
        }
        public static string QO06_OutBound_DN_Data(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], Sum(TO04_OutBoundActual_Sum_PARTS.CTN) AS [Picking QTY], TO03_OutBoundActual_Sum_FG.QTY AS [Packing QTY], [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT] AS [ACT DT FG], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT] AS [ACT DT PRT] " +
                    "FROM (TO03_OutBoundActual_Sum_FG LEFT JOIN TO04_OutBoundActual_Sum_PARTS ON TO03_OutBoundActual_Sum_FG.[INV NO] = TO04_OutBoundActual_Sum_PARTS.[INV NO]) LEFT JOIN [TO06_Export Entry] ON TO03_OutBoundActual_Sum_FG.[INV NO] = [TO06_Export Entry].[INV NO] " +
                    "GROUP BY TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], TO03_OutBoundActual_Sum_FG.QTY, [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] " +
                    "HAVING (((TO03_OutBoundActual_Sum_FG.[ACTUAL DT]) Between '"+From+"' And '"+To+"')) " +
                    "ORDER BY TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] ";
            return s_cmd;
        }
        public static string QO06_OutBound_DN_Data()
        {
            string s_cmd;
            s_cmd = "SELECT TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], Sum(TO04_OutBoundActual_Sum_PARTS.CTN) AS [Picking QTY], TO03_OutBoundActual_Sum_FG.QTY AS [Packing QTY], [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT] AS [ACT DT FG], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT] AS [ACT DT PRT] " +
                    "FROM (TO03_OutBoundActual_Sum_FG LEFT JOIN TO04_OutBoundActual_Sum_PARTS ON TO03_OutBoundActual_Sum_FG.[INV NO] = TO04_OutBoundActual_Sum_PARTS.[INV NO]) LEFT JOIN [TO06_Export Entry] ON TO03_OutBoundActual_Sum_FG.[INV NO] = [TO06_Export Entry].[INV NO] " +
                    "GROUP BY TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], TO03_OutBoundActual_Sum_FG.QTY, [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] " +
                    "ORDER BY TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] ";
            return s_cmd;
        }

        public static string QO07_OutBound_DN_Transit(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT QO06_OutBound_DN_Data.[ACT DT FG], Sum(QO06_OutBound_DN_Data.[PICKING QTY]) AS CTN " +
                    "FROM (SELECT TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], Sum(TO04_OutBoundActual_Sum_PARTS.CTN) AS [Picking QTY], TO03_OutBoundActual_Sum_FG.QTY AS [Packing QTY], [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT] AS [ACT DT FG], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT] AS [ACT DT PRT] " +
                    "FROM (TO03_OutBoundActual_Sum_FG LEFT JOIN TO04_OutBoundActual_Sum_PARTS ON TO03_OutBoundActual_Sum_FG.[INV NO] = TO04_OutBoundActual_Sum_PARTS.[INV NO]) LEFT JOIN [TO06_Export Entry] ON TO03_OutBoundActual_Sum_FG.[INV NO] = [TO06_Export Entry].[INV NO] " +
                    "GROUP BY TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], TO03_OutBoundActual_Sum_FG.QTY, [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] " +
                    //"HAVING (((TO03_OutBoundActual_Sum_FG.[ACTUAL DT]) Between '"+From+"' And '"+To+"')) " +
                    ") AS QO06_OutBound_DN_Data " +
                    "GROUP BY QO06_OutBound_DN_Data.[ACT DT FG]; ";

            return s_cmd;
        }
        public static string QO08_Date_Check(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT QO06_OutBound_DN_Data.[INV NO], Count(QO06_OutBound_DN_Data.[INV NO]) AS [Date Check] " +
                    "FROM (SELECT TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], Sum(TO04_OutBoundActual_Sum_PARTS.CTN) AS [Picking QTY], TO03_OutBoundActual_Sum_FG.QTY AS [Packing QTY], [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT] AS [ACT DT FG], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT] AS [ACT DT PRT] " +
                    "FROM (TO03_OutBoundActual_Sum_FG LEFT JOIN TO04_OutBoundActual_Sum_PARTS ON TO03_OutBoundActual_Sum_FG.[INV NO] = TO04_OutBoundActual_Sum_PARTS.[INV NO]) LEFT JOIN [TO06_Export Entry] ON TO03_OutBoundActual_Sum_FG.[INV NO] = [TO06_Export Entry].[INV NO] " +
                    "GROUP BY TO03_OutBoundActual_Sum_FG.[PJT CD], TO03_OutBoundActual_Sum_FG.[INV NO], TO03_OutBoundActual_Sum_FG.[PRT NO], TO03_OutBoundActual_Sum_FG.QTY, [TO06_Export Entry].Entry, TO03_OutBoundActual_Sum_FG.[ACTUAL DT], TO04_OutBoundActual_Sum_PARTS.[ACTUAL DT], TO03_OutBoundActual_Sum_FG.[PJT CD] " +
                    //"HAVING (((TO03_OutBoundActual_Sum_FG.[ACTUAL DT]) Between '"+From+"' And '"+To+"')) " +
                    ") AS QO06_OutBound_DN_Data " +
                    "GROUP BY QO06_OutBound_DN_Data.[INV NO] ";
                    //"HAVING (((Count(QO06_OutBound_DN_Data.[INV NO]))>1)); ";

            return s_cmd;
        }

        //M05_InOut Record
        public static string QR01_InBoundActual_DB(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT TI02_InBoundActual_DB.[INBD NO], TI02_InBoundActual_DB.[ROW NO], TI02_InBoundActual_DB.[DETAIL NO], TI02_InBoundActual_DB.[INBD CAT], "+
                    "TI02_InBoundActual_DB.[INBD CAT NM], TI02_InBoundActual_DB.[PO DT], TI02_InBoundActual_DB.[PO NO], TI02_InBoundActual_DB.[PLT NO], TI02_InBoundActual_DB.SUPLR, TI02_InBoundActual_DB.[SUPLR NM], TI02_InBoundActual_DB.REMARK, TI02_InBoundActual_DB.[ACTUAL DT], [TI02_InBoundActual_DB].[ACTUAL DT],[TI02_InBoundActual_DB].[ACTUAL DT],[TI02_InBoundActual_DB].[ACTUAL DT] AS [INBD DT], TI02_InBoundActual_DB.[PRT NO], TI02_InBoundActual_DB.[PRT NM1], TI02_InBoundActual_DB.[PRT NM2], TI02_InBoundActual_DB.[CASE NO], TI02_InBoundActual_DB.[PROD CAT], TI02_InBoundActual_DB.[PROD CAT NM], TI02_InBoundActual_DB.[PROD GP], TI02_InBoundActual_DB.[PROD GP NM], TI02_InBoundActual_DB.LCTN, TI02_InBoundActual_DB.BOI, TI02_InBoundActual_DB.[BOI NM], TI02_InBoundActual_DB.[LOT NO], TI02_InBoundActual_DB.[PJT CD], TI02_InBoundActual_DB.[PJT NM], TI02_InBoundActual_DB.[Consignee PO NO], TI02_InBoundActual_DB.QTY1, TI02_InBoundActual_DB.UNIT1, TI02_InBoundActual_DB.[WEIGTH1(kg)], TI02_InBoundActual_DB.[VOLUME1(m^3)], TI02_InBoundActual_DB.[INBD CHG1], TI02_InBoundActual_DB.[STOR CHG1], TI02_InBoundActual_DB.QTY2, TI02_InBoundActual_DB.UNIT2, TI02_InBoundActual_DB.[WEIGTH2(kg)], TI02_InBoundActual_DB.[VOLUME2(m^3)], TI02_InBoundActual_DB.[INBD CHG2], TI02_InBoundActual_DB.[STOR CHG2], TI02_InBoundActual_DB.QTY3, TI02_InBoundActual_DB.UNIT3, TI02_InBoundActual_DB.[WEIGTH3(kg)], TI02_InBoundActual_DB.[VOLUME3(m^3)], TI02_InBoundActual_DB.[INBD CHG3], TI02_InBoundActual_DB.[STOR CHG3], TI02_InBoundActual_DB.QTY4, TI02_InBoundActual_DB.UNIT4, TI02_InBoundActual_DB.[WEIGTH4(kg)], TI02_InBoundActual_DB.[VOLUME4(m^3)], TI02_InBoundActual_DB.[INBD CHG4], TI02_InBoundActual_DB.[STOR CHG4], TI02_InBoundActual_DB.[TOTAL PCS], TI02_InBoundActual_DB.[QTY1(PLAN)], TI02_InBoundActual_DB.[QTY2(PLAN)], TI02_InBoundActual_DB.[QTY3(PLAN)], TI02_InBoundActual_DB.[QTY4(PLAN)], TI02_InBoundActual_DB.[TOTAL PCS(PLAN)], TI02_InBoundActual_DB.ETD, TI02_InBoundActual_DB.[Original Lot No], TI02_InBoundActual_DB.[Inbound ED] "+
                    "FROM TI02_InBoundActual_DB "+
                    "WHERE (((TI02_InBoundActual_DB.[ACTUAL DT]) Between '"+From+"' And '"+To+"') AND ((TI02_InBoundActual_DB.BOI) NOT LIKE 'FG')); ";
            return s_cmd;
        }
        public static string QR01_OutBoundActual_DB(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT TO02_OutBoundActual_DB.[OUTBD NO], TO02_OutBoundActual_DB.[ROW NO], TO02_OutBoundActual_DB.[DETAIL NO], TO02_OutBoundActual_DB.[OUTBD CAT], TO02_OutBoundActual_DB.[OUTBD CAT NM], TO02_OutBoundActual_DB.[PO DT], TO02_OutBoundActual_DB.[PO NO], TO02_OutBoundActual_DB.[PLT NO], TO02_OutBoundActual_DB.[DELV CD], TO02_OutBoundActual_DB.[DELV NM], TO02_OutBoundActual_DB.[DELV NO], TO02_OutBoundActual_DB.REMARK, TO02_OutBoundActual_DB.[ACTUAL DT], " +
                    "[TO02_OutBoundActual_DB].[ACTUAL DT],[TO02_OutBoundActual_DB].[ACTUAL DT],[TO02_OutBoundActual_DB].[ACTUAL DT] AS [OUTBD DT], TO02_OutBoundActual_DB.[PRT NO], TO02_OutBoundActual_DB.[PRT NM1], TO02_OutBoundActual_DB.[PRT NM2], TO02_OutBoundActual_DB.[CASE NO], TO02_OutBoundActual_DB.[PROD CAT], TO02_OutBoundActual_DB.[PROD CAT NM], TO02_OutBoundActual_DB.[PROD GP], TO02_OutBoundActual_DB.[PROD GP NM], TO02_OutBoundActual_DB.LCTN, TO02_OutBoundActual_DB.BOI, TO02_OutBoundActual_DB.[BOI NM], TO02_OutBoundActual_DB.[LOT NO], TO02_OutBoundActual_DB.[PJT CD], TO02_OutBoundActual_DB.[PJT NM], TO02_OutBoundActual_DB.[INBD DT], TO02_OutBoundActual_DB.QTY1, TO02_OutBoundActual_DB.UNIT1, TO02_OutBoundActual_DB.WEIGTH1, TO02_OutBoundActual_DB.[VOLUME1(m^3)], TO02_OutBoundActual_DB.[OUTBD CHG1], TO02_OutBoundActual_DB.[STOR CHG1], TO02_OutBoundActual_DB.QTY2, TO02_OutBoundActual_DB.UNIT2, TO02_OutBoundActual_DB.WEIGTH2, TO02_OutBoundActual_DB.[VOLUME2(m^3)], TO02_OutBoundActual_DB.[OUTBD CHG2], TO02_OutBoundActual_DB.[STOR CHG2], TO02_OutBoundActual_DB.QTY3, TO02_OutBoundActual_DB.UNIT3, TO02_OutBoundActual_DB.WEIGTH3, TO02_OutBoundActual_DB.[VOLUME3(m^3)], TO02_OutBoundActual_DB.[OUTBD CHG3], TO02_OutBoundActual_DB.[STOR CHG3], TO02_OutBoundActual_DB.QTY4, TO02_OutBoundActual_DB.UNIT4, TO02_OutBoundActual_DB.WEIGTH4, TO02_OutBoundActual_DB.[VOLUME4(m^3)], TO02_OutBoundActual_DB.[OUTBD CHG4], TO02_OutBoundActual_DB.[STOR CHG4], TO02_OutBoundActual_DB.[TOTAL PCS], TO02_OutBoundActual_DB.[QTY1(PLAN)], TO02_OutBoundActual_DB.[QTY2(PLAN)], TO02_OutBoundActual_DB.[QTY3(PLAN)], TO02_OutBoundActual_DB.[QTY4(PLAN)], TO02_OutBoundActual_DB.[TOTAL PCS(PLAN)], TO02_OutBoundActual_DB.[ITEM SECTION], TO02_OutBoundActual_DB.[QTY/UNIT(3)], TO02_OutBoundActual_DB.[Gross Weight], TO02_OutBoundActual_DB.[Box Size], TO02_OutBoundActual_DB.COO, TO02_OutBoundActual_DB.[Original Lot No], TO02_OutBoundActual_DB.[Inbound ED], TO02_OutBoundActual_DB.[Outbound ED] " +
                    "FROM TO02_OutBoundActual_DB " +
                    "WHERE (((TO02_OutBoundActual_DB.[ACTUAL DT]) Between '' And '') AND ((TO02_OutBoundActual_DB.BOI) NOT LIKE 'FG')); ";
            return s_cmd;
        }
        public static string QR02_INOUT_RECORD_TFZ(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT TR01_InBoundActual_DB.[PROD CAT], TR01_InBoundActual_DB.[PO NO] AS [SUP PO NO], TR01_InBoundActual_DB.[Consignee PO NO] AS [CON PO NO], TR02_OutBoundActual_DB.[DELV NO] AS [INV NO], TR01_InBoundActual_DB.[PRT NO], TR01_InBoundActual_DB.[PJT CD], "+
                    "Sum(TR01_InBoundActual_DB.QTY4) AS [INBD QTY], Sum(TR02_OutBoundActual_DB.QTY4) AS [OUTBD QTY], TR01_InBoundActual_DB.[INBD DT], TR02_OutBoundActual_DB.[OUTBD DT] "+
                    "FROM TR01_InBoundActual_DB LEFT JOIN TR02_OutBoundActual_DB ON TR01_InBoundActual_DB.[CASE NO] = TR02_OutBoundActual_DB.[CASE NO] "+
                    "GROUP BY TR01_InBoundActual_DB.[PROD CAT], TR01_InBoundActual_DB.[PO NO], TR01_InBoundActual_DB.[Consignee PO NO], TR02_OutBoundActual_DB.[DELV NO], TR01_InBoundActual_DB.[PRT NO], TR01_InBoundActual_DB.[PJT CD], TR01_InBoundActual_DB.[INBD DT], TR02_OutBoundActual_DB.[OUTBD DT] "+
                    "HAVING (((TR01_InBoundActual_DB.[PROD CAT]) != 'FG EXP') AND ((TR02_OutBoundActual_DB.[OUTBD DT]) Between '"+From+"' And '"+To+"')) OR (((TR02_OutBoundActual_DB.[OUTBD DT]) Is Null)) "+
                    "ORDER BY TR01_InBoundActual_DB.[INBD DT]; ";
            return s_cmd;
        }

        //M06_OutBoundActual M3
        public static string QR03_OutBoundActual_M3_TFZ(string From, string To)
        {
            string s_cmd;
            s_cmd = "SELECT [TO02_OutBoundActual_DB].[ACTUAL DT] AS [Month], TO02_OutBoundActual_DB.[ACTUAL DT], "+
                    "Sum(TO02_OutBoundActual_DB.[VOLUME4(m^3)]) AS M3 "+
                    "FROM TO02_OutBoundActual_DB "+
                    "GROUP BY [TO02_OutBoundActual_DB].[ACTUAL DT], TO02_OutBoundActual_DB.[ACTUAL DT], TO02_OutBoundActual_DB.BOI "+
                    "HAVING (((TO02_OutBoundActual_DB.[ACTUAL DT]) Between '"+From+"' And '"+To+"') AND ((TO02_OutBoundActual_DB.BOI)!='FG')); ";
            return s_cmd;
        }

        //M07_Delete InBoundActula DB
        public static string QI03_InBoundActual_DB_Delete(DateTime until)
        {
            string s_cmd;
            s_cmd = "DELETE FROM TI02_InBoundActual_DB "+
                    "WHERE (((TI02_InBoundActual_DB.[ACTUAL DT])<= '"+until+"')); ";

            return s_cmd;
        }

        //M08_Delete OutBoundActual DB
        public static string QO09_OutBoundActual_DB_Delete(DateTime until)
        {
            string s_cmd;
            s_cmd = "DELETE FROM TO02_OutBoundActual_DB " +
                    "WHERE (((TO02_OutBoundActual_DB.[ACTUAL DT])<='"+until+"')); ";
            return s_cmd;
        }
        #endregion

        #region Stock List
        public static string Q01_Stock_List_with_FG()
        {
            string s_cmd;
            s_cmd = "SELECT StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO] AS [SUP PO NO], StockBySumCond.[Consignee PO NO] AS [CON PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI,  " +
                    "Count(StockBySumCond.[CASE NO]) AS CTN, Sum(StockBySumCond.[ACTUAL QTY4]) AS [ACT QTY], StockBySumCond.UNIT4 AS UNIT, Sum(StockBySumCond.QTY4) AS QTY  " +
                    "FROM StockBySumCond  " +
                    "GROUP BY StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO], StockBySumCond.[Consignee PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, StockBySumCond.UNIT4  " +
                    "ORDER BY StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[PRT NO];  ";

            return s_cmd;
        }
        public static string Q02_Location_Check()
        {
            string s_cmd;
            s_cmd = "SELECT StockBySumCond.LCTN "+
                    "FROM StockBySumCond "+
                    "GROUP BY StockBySumCond.LCTN ";
            return s_cmd;
        }
        public static string Q03_Stock_List_Parts()
        {
            string s_cmd;
            s_cmd = "SELECT StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO] AS [SUP PO NO], StockBySumCond.[Consignee PO NO] AS [CON PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, Count(StockBySumCond.[CASE NO]) AS CTN, Sum(StockBySumCond.[ACTUAL QTY4]) AS [ACT QTY], StockBySumCond.UNIT4 AS UNIT, Sum(StockBySumCond.QTY4) AS QTY " +
                    "FROM StockBySumCond " +
                    "GROUP BY StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO], StockBySumCond.[Consignee PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, StockBySumCond.UNIT4 " +
                    "HAVING (((StockBySumCond.BOI)<> 'FG')) ";
            return s_cmd;
        }
        public static string Q04_Stock_List_Parts_Temp()
        {
            string s_cmd;
            s_cmd = "SELECT StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO] AS [SUP PO NO], StockBySumCond.[Consignee PO NO] AS [CON PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, Count(StockBySumCond.[CASE NO]) AS CTN, Sum(StockBySumCond.[ACTUAL QTY4]) AS [ACT QTY], StockBySumCond.UNIT4 AS UNIT, Sum(StockBySumCond.QTY4) AS QTY " +
                    "FROM StockBySumCond " +
                    "GROUP BY StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO], StockBySumCond.[Consignee PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, StockBySumCond.UNIT4, SUBSTRING([StockBySumCond].[LCTN],2,1) " +
                    "HAVING (((StockBySumCond.BOI)<>'FG') AND ((SUBSTRING([StockBySumCond].[LCTN],2,1))<>'-')) ";
            return s_cmd;
        }
        public static string Q05_Stock_List_Parts_Packed()
        {
            string s_cmd;
            s_cmd = "SELECT StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO] AS [SUP PO NO], StockBySumCond.[Consignee PO NO] AS [CON PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, Count(StockBySumCond.[CASE NO]) AS CTN, Sum(StockBySumCond.[ACTUAL QTY4]) AS [ACT QTY], StockBySumCond.UNIT4 AS UNIT, Sum(StockBySumCond.QTY4) AS QTY " +
                    "FROM StockBySumCond " +
                    "GROUP BY StockBySumCond.LCTN, StockBySumCond.[PJT CD], StockBySumCond.[LOT NO], StockBySumCond.[Consignee PO NO], StockBySumCond.[PRT NO], StockBySumCond.BOI, StockBySumCond.UNIT4, SUBSTRING([StockBySumCond].[LCTN],2,1) ";
                    //"HAVING (((StockBySumCond.BOI)<>'FG') AND ((SUBSTRING([StockBySumCond].[LCTN],2,1))='-')) ";

            return s_cmd;
        }
        #endregion

        #region TFZ Report
        public static string Q01_Add_InBoundActual_DB()
        {
            string s_cmd;
            s_cmd = "INSERT INTO T03_InBoundActual_DB ( [INBD NO], [ROW NO], [DETAIL NO], [PO DT], [SUP PO NO], SUPLR, [INBD DT], [PRT NO], [CASE NO], [PROD CAT], LCTN, BOI, [LOT NO], [PJT CD], [CON PO NO], QTY, UNIT, NW, M3, ETD, [Original Lot No], [Inbound ED], [PRT NO 2] ) " +
                            "SELECT T01_InBoundActual_Original.[INBD NO], T01_InBoundActual_Original.[ROW NO], T01_InBoundActual_Original.[DETAIL NO], T01_InBoundActual_Original.[PO DT], T01_InBoundActual_Original.[PO NO] AS [SUP PO NO], T01_InBoundActual_Original.SUPLR, T01_InBoundActual_Original.[ACTUAL DT] AS [INBD DT], T01_InBoundActual_Original.[PRT NO], T01_InBoundActual_Original.[CASE NO], T01_InBoundActual_Original.[PROD CAT], T01_InBoundActual_Original.LCTN, T01_InBoundActual_Original.BOI, T01_InBoundActual_Original.[LOT NO], T01_InBoundActual_Original.[PJT CD], T01_InBoundActual_Original.[Consignee PO NO] AS [CON PO NO], T01_InBoundActual_Original.QTY4 AS QTY, T01_InBoundActual_Original.UNIT4 AS UNIT, T01_InBoundActual_Original.[WEIGTH4(kg)] AS NW, T01_InBoundActual_Original.[VOLUME4(m^3)] AS M3, T01_InBoundActual_Original.ETD, T01_InBoundActual_Original.[Original Lot No], T01_InBoundActual_Original.[Inbound ED], T01_InBoundActual_Original.[PRT NO 2] " +
                            "FROM T01_InBoundActual_Original; ";
            return s_cmd;
        }
        public static string Q01_Delete_InBoundActual_DB(DateTime until)
        {
            string s_cmd;
            s_cmd = "DELETE FROM T03_InBoundActual_DB "+
                    "WHERE (((T03_InBoundActual_DB.[INBD DT])<='"+until.ToString("yyyyMMdd")+"')); ";
            return s_cmd;
        }
        public static string Q02_Add_OutBoundActual_DB()
        {
            string s_cmd;
            s_cmd = "INSERT INTO T04_OutBoundActual_DB ( [OUTBD NO], [ROW NO], [DETAIL NO], [PO DT], [CON PO NO], [INV NO], [OUTBD DT], [PRT NO], [CASE NO], [PROD CAT], LCTN, BOI, [SUP PO NO], [PJT CD], [INBD DT], QTY, UNIT, NW, M3, [ITEM SECTION], [QTY/UNIT], GW, [Box Size], COO, [Original Lot No], [Inbound ED], [Outbound ED], [PRT NO 2] ) " +
                    "SELECT T02_OutBoundActual_Original.[OUTBD NO], T02_OutBoundActual_Original.[ROW NO], T02_OutBoundActual_Original.[DETAIL NO], T02_OutBoundActual_Original.[PO DT], T02_OutBoundActual_Original.[PO NO] AS [CON PO NO], T02_OutBoundActual_Original.[DELV NO] AS [INV NO], T02_OutBoundActual_Original.[ACTUAL DT] AS [OUTBD DT], T02_OutBoundActual_Original.[PRT NO], T02_OutBoundActual_Original.[CASE NO], T02_OutBoundActual_Original.[PROD CAT], T02_OutBoundActual_Original.LCTN, T02_OutBoundActual_Original.BOI, T02_OutBoundActual_Original.[LOT NO] AS [SUP PO NO], T02_OutBoundActual_Original.[PJT CD], T02_OutBoundActual_Original.[INBD DT], T02_OutBoundActual_Original.QTY4 AS QTY, T02_OutBoundActual_Original.UNIT4 AS UNIT, T02_OutBoundActual_Original.WEIGTH4 AS NW, T02_OutBoundActual_Original.[VOLUME4(m^3)] AS M3, T02_OutBoundActual_Original.[ITEM SECTION], T02_OutBoundActual_Original.[QTY/UNIT(3)] AS [QTY/UNIT], T02_OutBoundActual_Original.[Gross Weight] AS GW, T02_OutBoundActual_Original.[Box Size], T02_OutBoundActual_Original.COO, T02_OutBoundActual_Original.[Original Lot No], T02_OutBoundActual_Original.[Inbound ED], T02_OutBoundActual_Original.[Outbound ED], T02_OutBoundActual_Original.[PRT NO 2] " +
                    "FROM T02_OutBoundActual_Original " +
                    "WHERE (((T02_OutBoundActual_Original.BOI)<>'FG')); ";
            return s_cmd;
        }
        public static string Q02_Delete_OutBoundActual_DB(DateTime until)
        {
            string s_cmd;
            s_cmd = "DELETE FROM T04_OutBoundActual_DB "+
                    "WHERE (((T04_OutBoundActual_DB.[OUTBD DT])<='"+until.ToString("yyyyMMdd")+"'));";
            return s_cmd;
        }
        public static string QI01_InBoundActual_KeyCode()
        {
            string s_cmd = "";
            if (TableTrucated("TI01_InBoundActual_KeyCode"))
            {
                s_cmd = "INSERT INTO TI01_InBoundActual_KeyCode ([KeyCode],[INBD NO],[ROW NO],[DETAIL NO],[PO DT],[SUP PO NO],[SUPLR],[INBD DT],[PRT NO],[CASE NO],[PROD CAT],[LCTN],[BOI],[LOT NO],[PJT CD],[CON PO NO],[QTY],[UNIT],[NW],[M3],[ETD],[Original Lot No],[Inbound ED]) " +
                        "SELECT CONCAT([T03_InBoundActual_DB].[Inbound ED],'-',[T03_InBoundActual_DB].[SUP PO NO],'-',[T03_InBoundActual_DB].[PRT NO 2]) AS KeyCode, " +
                        "T03_InBoundActual_DB.[INBD NO], [T03_InBoundActual_DB].[ROW NO] AS [ROW NO], [T03_InBoundActual_DB].[DETAIL NO] AS [DETAIL NO],  " +
                        "T03_InBoundActual_DB.[PO DT], T03_InBoundActual_DB.[SUP PO NO], T03_InBoundActual_DB.SUPLR, T03_InBoundActual_DB.[INBD DT], T03_InBoundActual_DB.[PRT NO], T03_InBoundActual_DB.[CASE NO], T03_InBoundActual_DB.[PROD CAT], T03_InBoundActual_DB.LCTN, T03_InBoundActual_DB.BOI, T03_InBoundActual_DB.[LOT NO], T03_InBoundActual_DB.[PJT CD], T03_InBoundActual_DB.[CON PO NO], T03_InBoundActual_DB.QTY, T03_InBoundActual_DB.UNIT, T03_InBoundActual_DB.NW, T03_InBoundActual_DB.M3, T03_InBoundActual_DB.ETD, T03_InBoundActual_DB.[Original Lot No], T03_InBoundActual_DB.[Inbound ED]  " +
                    /*INTO TI01_InBoundActual_KeyCode*/
                        "FROM T03_InBoundActual_DB; ";
            }
            return s_cmd;
            
        }
        public static string QI02_CustomsData_MoveIn_KeyCode()
        {
            string s_cmd = "";
            if (TableTrucated("TI02_CustomsData_MoveIn_KeyCode"))
            {
                s_cmd = "INSERT INTO TI02_CustomsData_MoveIn_KeyCode ([KeyCode],[DECLRATION NO],[LINE NO],[COMMODITY DESCRIPTION IN ENGLISH],[PO NO],[EXPORT DATE],[SUBMIT DATE],[INVOICE NO#],[INVOICE DATE],[QUANTITY (UNIT)],[UNIT],[PART NO],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS]) "+
                        "SELECT CONCAT([T05_CustomsData_MoveIn_Original].[DECLRATION NO],'-',[T05_CustomsData_MoveIn_Original].[PO NO],'-',[T05_CustomsData_MoveIn_Original].[PART NO]) AS KeyCode, " +
                        "T05_CustomsData_MoveIn_Original.[DECLRATION NO], T05_CustomsData_MoveIn_Original.[LINE NO], T05_CustomsData_MoveIn_Original.[COMMODITY DESCRIPTION IN ENGLISH], T05_CustomsData_MoveIn_Original.[PO NO], T05_CustomsData_MoveIn_Original.[EXPORT DATE], T05_CustomsData_MoveIn_Original.[SUBMIT DATE], T05_CustomsData_MoveIn_Original.[INVOICE NO#], T05_CustomsData_MoveIn_Original.[INVOICE DATE], T05_CustomsData_MoveIn_Original.[QUANTITY (UNIT)], T05_CustomsData_MoveIn_Original.UNIT, T05_CustomsData_MoveIn_Original.[PART NO], T05_CustomsData_MoveIn_Original.[UNIT PRICE], T05_CustomsData_MoveIn_Original.[FOB PRICE (FOREIGN CURRENCY], T05_CustomsData_MoveIn_Original.Currency, T05_CustomsData_MoveIn_Original.[EX RATE], T05_CustomsData_MoveIn_Original.[PRICE FOB (THAI BAHT)], T05_CustomsData_MoveIn_Original.STATUS  " +
                    /*INTO TI02_CustomsData_MoveIn_KeyCode*/
                        "FROM T05_CustomsData_MoveIn_Original " +
                        "WHERE (((T05_CustomsData_MoveIn_Original.[COMMODITY DESCRIPTION IN ENGLISH]) NOT LIKE '%PALLET%')); ";
            }
            return s_cmd;
        }
        public static string QI03_InBoundActual_LineNo()
        {
            string s_cmd ="";
            if (TableTrucated("TI03_InBoundActual_LineNo"))
            {
                s_cmd = "INSERT INTO TI03_InBoundActual_LineNo ([DECLRATION NO],[LINE NO],[INBD NO],[ROW NO],[DETAIL NO],[PO DT],[SUP PO NO],[SUPLR],[INBD DT],[PRT NO],[CASE NO],[PROD CAT],[LCTN],[BOI],[LOT NO],[PJT CD],[CON PO NO],[QTY],[UNIT],[NW],[M3],[ETD],[Original Lot No],[Inbound ED],[COMMODITY DESCRIPTION IN ENGLISH],[INVOICE NO#],[QUANTITY (UNIT)],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS],[KeyCode]) "+
                        "SELECT TI02_CustomsData_MoveIn_KeyCode.[DECLRATION NO], TI02_CustomsData_MoveIn_KeyCode.[LINE NO], TI01_InBoundActual_KeyCode.[INBD NO], TI01_InBoundActual_KeyCode.[ROW NO], TI01_InBoundActual_KeyCode.[DETAIL NO], TI01_InBoundActual_KeyCode.[PO DT], TI01_InBoundActual_KeyCode.[SUP PO NO], TI01_InBoundActual_KeyCode.SUPLR, TI01_InBoundActual_KeyCode.[INBD DT], TI01_InBoundActual_KeyCode.[PRT NO], TI01_InBoundActual_KeyCode.[CASE NO], TI01_InBoundActual_KeyCode.[PROD CAT], TI01_InBoundActual_KeyCode.LCTN, TI01_InBoundActual_KeyCode.BOI, TI01_InBoundActual_KeyCode.[LOT NO], TI01_InBoundActual_KeyCode.[PJT CD], TI01_InBoundActual_KeyCode.[CON PO NO], TI01_InBoundActual_KeyCode.QTY, TI01_InBoundActual_KeyCode.UNIT, TI01_InBoundActual_KeyCode.NW, TI01_InBoundActual_KeyCode.M3, TI01_InBoundActual_KeyCode.ETD, TI01_InBoundActual_KeyCode.[Original Lot No], TI01_InBoundActual_KeyCode.[Inbound ED], TI02_CustomsData_MoveIn_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TI02_CustomsData_MoveIn_KeyCode.[INVOICE NO#], TI02_CustomsData_MoveIn_KeyCode.[QUANTITY (UNIT)], TI02_CustomsData_MoveIn_KeyCode.[UNIT PRICE], TI02_CustomsData_MoveIn_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TI02_CustomsData_MoveIn_KeyCode.Currency, TI02_CustomsData_MoveIn_KeyCode.[EX RATE], TI02_CustomsData_MoveIn_KeyCode.[PRICE FOB (THAI BAHT)], TI02_CustomsData_MoveIn_KeyCode.STATUS, TI01_InBoundActual_KeyCode.KeyCode " +
                        /*INTO TI03_InBoundActual_LineNo*/
                        "FROM TI01_InBoundActual_KeyCode LEFT JOIN TI02_CustomsData_MoveIn_KeyCode ON TI01_InBoundActual_KeyCode.KeyCode = TI02_CustomsData_MoveIn_KeyCode.KeyCode " +
                        "GROUP BY TI02_CustomsData_MoveIn_KeyCode.[DECLRATION NO], TI02_CustomsData_MoveIn_KeyCode.[LINE NO], TI01_InBoundActual_KeyCode.[INBD NO], TI01_InBoundActual_KeyCode.[ROW NO], TI01_InBoundActual_KeyCode.[DETAIL NO], TI01_InBoundActual_KeyCode.[PO DT], TI01_InBoundActual_KeyCode.[SUP PO NO], TI01_InBoundActual_KeyCode.SUPLR, TI01_InBoundActual_KeyCode.[INBD DT], TI01_InBoundActual_KeyCode.[PRT NO], TI01_InBoundActual_KeyCode.[CASE NO], TI01_InBoundActual_KeyCode.[PROD CAT], TI01_InBoundActual_KeyCode.LCTN, TI01_InBoundActual_KeyCode.BOI, TI01_InBoundActual_KeyCode.[LOT NO], TI01_InBoundActual_KeyCode.[PJT CD], TI01_InBoundActual_KeyCode.[CON PO NO], TI01_InBoundActual_KeyCode.QTY, TI01_InBoundActual_KeyCode.UNIT, TI01_InBoundActual_KeyCode.NW, TI01_InBoundActual_KeyCode.M3, TI01_InBoundActual_KeyCode.ETD, TI01_InBoundActual_KeyCode.[Original Lot No], TI01_InBoundActual_KeyCode.[Inbound ED], TI02_CustomsData_MoveIn_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TI02_CustomsData_MoveIn_KeyCode.[INVOICE NO#], TI02_CustomsData_MoveIn_KeyCode.[QUANTITY (UNIT)], TI02_CustomsData_MoveIn_KeyCode.[UNIT PRICE], TI02_CustomsData_MoveIn_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TI02_CustomsData_MoveIn_KeyCode.Currency, TI02_CustomsData_MoveIn_KeyCode.[EX RATE], TI02_CustomsData_MoveIn_KeyCode.[PRICE FOB (THAI BAHT)], TI02_CustomsData_MoveIn_KeyCode.STATUS, TI01_InBoundActual_KeyCode.KeyCode, TI01_InBoundActual_KeyCode.[INBD DT] " ;
            }
            return s_cmd;
        }
        public static string QI04_Duplicate_Check()
        {
            string s_cmd = "";
            if (TableTrucated("[TI04_InBound_Duplicate_CheckMaster]"))
            {
                s_cmd = "INSERT INTO TI04_InBound_Duplicate_CheckMaster ([DECLRATION NO],[INBD DT],[INVOICE NO#],[SUP PO NO],[PRT NO],[CASE NO],[DATA CHECK],[KeyCode]) " +
                        "SELECT TI03_InBoundActual_LineNo.[DECLRATION NO], TI03_InBoundActual_LineNo.[INBD DT], TI03_InBoundActual_LineNo.[INVOICE NO#], TI03_InBoundActual_LineNo.[SUP PO NO], TI03_InBoundActual_LineNo.[PRT NO], TI03_InBoundActual_LineNo.[CASE NO], Count(TI03_InBoundActual_LineNo.[CASE NO]) AS [DATA CHECK], TI03_InBoundActual_LineNo.KeyCode " +
                    /*INTO TI04_InBound_Duplicate_CheckMaster*/
                        "FROM TI03_InBoundActual_LineNo " +
                        "GROUP BY TI03_InBoundActual_LineNo.[DECLRATION NO], TI03_InBoundActual_LineNo.[INBD DT], TI03_InBoundActual_LineNo.[INVOICE NO#], TI03_InBoundActual_LineNo.[SUP PO NO], TI03_InBoundActual_LineNo.[PRT NO], TI03_InBoundActual_LineNo.[CASE NO], TI03_InBoundActual_LineNo.KeyCode " +
                        "HAVING (((Count(TI03_InBoundActual_LineNo.[CASE NO]))>1)) " +
                        "ORDER BY TI03_InBoundActual_LineNo.[INBD DT]; ";
            }
            return s_cmd;
        }
        public static string QI05_InBoundActual_LineNo_Check()
        {
            string s_cmd= "";
            if (TableTrucated("[TI05_InBoundActual_DB_Final]"))
            {
                s_cmd = "INSERT INTO TI05_InBoundActual_DB_Final ([DATA CHECK],[KeyCode3],[CTN],[DECLRATION NO],[LINE NO],[INBD NO],[ROW NO],[DETAIL NO],[PO DT],[SUP PO NO],[SUPLR],[INBD DT],[PRT NO],[CASE NO],[PROD CAT],[LCTN],[BOI],[LOT NO],[PJT CD],[CON PO NO],[QTY],[UNIT],[NW],[M3],[ETD],[Original Lot No],[Inbound ED],[COMMODITY DESCRIPTION IN ENGLISH],[INVOICE NO#],[QUANTITY (UNIT)],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS],[KeyCode]) " +
                        "SELECT TI04_InBound_Duplicate_CheckMaster.[DATA CHECK], CONCAT([TI03_InBoundActual_LineNo].[QUANTITY (UNIT)] ,'-', [TI03_InBoundActual_LineNo].[KeyCode]) AS KeyCode3, " +
                        "[TI03_InBoundActual_LineNo].[QUANTITY (UNIT)]/[TI03_InBoundActual_LineNo].[QTY] AS CTN, TI03_InBoundActual_LineNo.[DECLRATION NO], TI03_InBoundActual_LineNo.[LINE NO], TI03_InBoundActual_LineNo.[INBD NO], TI03_InBoundActual_LineNo.[ROW NO], TI03_InBoundActual_LineNo.[DETAIL NO], TI03_InBoundActual_LineNo.[PO DT], TI03_InBoundActual_LineNo.[SUP PO NO], TI03_InBoundActual_LineNo.SUPLR, TI03_InBoundActual_LineNo.[INBD DT], TI03_InBoundActual_LineNo.[PRT NO], TI03_InBoundActual_LineNo.[CASE NO], TI03_InBoundActual_LineNo.[PROD CAT], TI03_InBoundActual_LineNo.LCTN, TI03_InBoundActual_LineNo.BOI, TI03_InBoundActual_LineNo.[LOT NO], TI03_InBoundActual_LineNo.[PJT CD], TI03_InBoundActual_LineNo.[CON PO NO], TI03_InBoundActual_LineNo.QTY, TI03_InBoundActual_LineNo.UNIT, TI03_InBoundActual_LineNo.NW, TI03_InBoundActual_LineNo.M3, TI03_InBoundActual_LineNo.ETD, TI03_InBoundActual_LineNo.[Original Lot No], TI03_InBoundActual_LineNo.[Inbound ED], TI03_InBoundActual_LineNo.[COMMODITY DESCRIPTION IN ENGLISH], TI03_InBoundActual_LineNo.[INVOICE NO#], TI03_InBoundActual_LineNo.[QUANTITY (UNIT)], TI03_InBoundActual_LineNo.[UNIT PRICE], TI03_InBoundActual_LineNo.[FOB PRICE (FOREIGN CURRENCY], TI03_InBoundActual_LineNo.Currency, TI03_InBoundActual_LineNo.[EX RATE], TI03_InBoundActual_LineNo.[PRICE FOB (THAI BAHT)], TI03_InBoundActual_LineNo.STATUS, TI03_InBoundActual_LineNo.KeyCode  " +
                    /*INTO TI05_InBoundActual_DB_Final*/
                        "FROM TI03_InBoundActual_LineNo LEFT JOIN TI04_InBound_Duplicate_CheckMaster ON TI03_InBoundActual_LineNo.KeyCode = TI04_InBound_Duplicate_CheckMaster.KeyCode " +
                        "GROUP BY TI04_InBound_Duplicate_CheckMaster.[DATA CHECK], CONCAT([TI03_InBoundActual_LineNo].[QUANTITY (UNIT)] ,'-', [TI03_InBoundActual_LineNo].[KeyCode]), [TI03_InBoundActual_LineNo].[QUANTITY (UNIT)]/[TI03_InBoundActual_LineNo].[QTY], TI03_InBoundActual_LineNo.[DECLRATION NO], TI03_InBoundActual_LineNo.[LINE NO], TI03_InBoundActual_LineNo.[INBD NO], TI03_InBoundActual_LineNo.[ROW NO], TI03_InBoundActual_LineNo.[DETAIL NO], TI03_InBoundActual_LineNo.[PO DT], TI03_InBoundActual_LineNo.[SUP PO NO], TI03_InBoundActual_LineNo.SUPLR, TI03_InBoundActual_LineNo.[INBD DT], TI03_InBoundActual_LineNo.[PRT NO], TI03_InBoundActual_LineNo.[CASE NO], TI03_InBoundActual_LineNo.[PROD CAT], TI03_InBoundActual_LineNo.LCTN, TI03_InBoundActual_LineNo.BOI, TI03_InBoundActual_LineNo.[LOT NO], TI03_InBoundActual_LineNo.[PJT CD], TI03_InBoundActual_LineNo.[CON PO NO], TI03_InBoundActual_LineNo.QTY, TI03_InBoundActual_LineNo.UNIT, TI03_InBoundActual_LineNo.NW, TI03_InBoundActual_LineNo.M3, TI03_InBoundActual_LineNo.ETD, TI03_InBoundActual_LineNo.[Original Lot No], TI03_InBoundActual_LineNo.[Inbound ED], TI03_InBoundActual_LineNo.[COMMODITY DESCRIPTION IN ENGLISH], TI03_InBoundActual_LineNo.[INVOICE NO#], TI03_InBoundActual_LineNo.[QUANTITY (UNIT)], TI03_InBoundActual_LineNo.[UNIT PRICE], TI03_InBoundActual_LineNo.[FOB PRICE (FOREIGN CURRENCY], TI03_InBoundActual_LineNo.Currency, TI03_InBoundActual_LineNo.[EX RATE], TI03_InBoundActual_LineNo.[PRICE FOB (THAI BAHT)], TI03_InBoundActual_LineNo.STATUS, TI03_InBoundActual_LineNo.KeyCode, TI03_InBoundActual_LineNo.[INBD DT]; ";
            }
            return s_cmd;
        }
        public static string QI06_Details_of_Export_MoveIn_WMS_DB()
        {
            string s_cmd= "";
            if (TableTrucated("[TI06_Details of Export MoveIn_WMS_DB]"))
            {
                s_cmd = "INSERT INTO [TI06_Details of Export MoveIn_WMS_DB] ([KeyCode4],[INBD DT],[Inbound ED],[DECLRATION NO],[LINE NO],[PJT CD],[SUP PO NO],[PRT NO],[QTY],[UNIT]) " +
                        "SELECT CONCAT([TI05_InBoundActual_DB_Final].[DECLRATION NO],'-',[TI05_InBoundActual_DB_Final].[LINE NO]) AS KeyCode4, TI05_InBoundActual_DB_Final.[INBD DT], TI05_InBoundActual_DB_Final.[Inbound ED], TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TI05_InBoundActual_DB_Final.[PJT CD], TI05_InBoundActual_DB_Final.[SUP PO NO], TI05_InBoundActual_DB_Final.[PRT NO], Sum(TI05_InBoundActual_DB_Final.QTY) AS QTY, TI05_InBoundActual_DB_Final.UNIT  " +
                    /*INTO [TI06_Details of Export MoveIn_WMS_DB]*/
                        "FROM TI05_InBoundActual_DB_Final " +
                        "GROUP BY CONCAT([TI05_InBoundActual_DB_Final].[DECLRATION NO] ,'-', [TI05_InBoundActual_DB_Final].[LINE NO]), TI05_InBoundActual_DB_Final.[INBD DT], TI05_InBoundActual_DB_Final.[Inbound ED], TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TI05_InBoundActual_DB_Final.[PJT CD], TI05_InBoundActual_DB_Final.[SUP PO NO], TI05_InBoundActual_DB_Final.[PRT NO], TI05_InBoundActual_DB_Final.UNIT; ";
            }
            return s_cmd;
        }
        public static string QI07_Details_of_EXP_MoveIn_TIFFA_DB()
        {
            string s_cmd= "";
            if (TableTrucated("[TI07_Details of EXP MoveIn_TIFFA_DB]"))
            {
                s_cmd = "INSERT INTO [TI07_Details of EXP MoveIn_TIFFA_DB] ([KeyCode4],[DECLRATION NO],[LINE NO],[COMMODITY DESCRIPTION IN ENGLISH],[PO NO],[EXPORT DATE],[SUBMIT DATE],[INVOICE NO#],[INVOICE DATE],[QUANTITY (UNIT)],[PART NO],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS]) " +
                        "SELECT CONCAT([TI02_CustomsData_MoveIn_KeyCode].[DECLRATION NO] ,'-',[TI02_CustomsData_MoveIn_KeyCode].[LINE NO]) AS KeyCode4, " +
                        "TI02_CustomsData_MoveIn_KeyCode.[DECLRATION NO], TI02_CustomsData_MoveIn_KeyCode.[LINE NO], TI02_CustomsData_MoveIn_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TI02_CustomsData_MoveIn_KeyCode.[PO NO], TI02_CustomsData_MoveIn_KeyCode.[EXPORT DATE], TI02_CustomsData_MoveIn_KeyCode.[SUBMIT DATE], TI02_CustomsData_MoveIn_KeyCode.[INVOICE NO#], TI02_CustomsData_MoveIn_KeyCode.[INVOICE DATE], TI02_CustomsData_MoveIn_KeyCode.[QUANTITY (UNIT)], TI02_CustomsData_MoveIn_KeyCode.[PART NO], TI02_CustomsData_MoveIn_KeyCode.[UNIT PRICE], TI02_CustomsData_MoveIn_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TI02_CustomsData_MoveIn_KeyCode.Currency, TI02_CustomsData_MoveIn_KeyCode.[EX RATE], TI02_CustomsData_MoveIn_KeyCode.[PRICE FOB (THAI BAHT)], TI02_CustomsData_MoveIn_KeyCode.STATUS  " +
                    /*[TI07_Details of EXP MoveIn_TIFFA_DB]*/
                        "FROM TI02_CustomsData_MoveIn_KeyCode " +
                        "GROUP BY CONCAT([TI02_CustomsData_MoveIn_KeyCode].[DECLRATION NO] ,'-', [TI02_CustomsData_MoveIn_KeyCode].[LINE NO]), TI02_CustomsData_MoveIn_KeyCode.[DECLRATION NO], TI02_CustomsData_MoveIn_KeyCode.[LINE NO], TI02_CustomsData_MoveIn_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TI02_CustomsData_MoveIn_KeyCode.[PO NO], TI02_CustomsData_MoveIn_KeyCode.[EXPORT DATE], TI02_CustomsData_MoveIn_KeyCode.[SUBMIT DATE], TI02_CustomsData_MoveIn_KeyCode.[INVOICE NO#], TI02_CustomsData_MoveIn_KeyCode.[INVOICE DATE], TI02_CustomsData_MoveIn_KeyCode.[QUANTITY (UNIT)], TI02_CustomsData_MoveIn_KeyCode.[PART NO], TI02_CustomsData_MoveIn_KeyCode.[UNIT PRICE], TI02_CustomsData_MoveIn_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TI02_CustomsData_MoveIn_KeyCode.Currency, TI02_CustomsData_MoveIn_KeyCode.[EX RATE], TI02_CustomsData_MoveIn_KeyCode.[PRICE FOB (THAI BAHT)], TI02_CustomsData_MoveIn_KeyCode.STATUS ";
            }

            return s_cmd;
        }
        public static string QI08_Details_of_Export_MoveIn_Final()
        {
            string s_cmd= "";
            if (TableTrucated("[TI08_Details of Export MoveIn_Final]"))
            {
                s_cmd = "INSERT INTO [TI08_Details of Export MoveIn_Final] ([INBD DT],[Inbound ED],[DECLRATION NO],[LINE NO],[COMMODITY],[PJT CD],[SUP PO NO],[PRT NO],[QTY],[UNIT],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS]) " +
                        "SELECT [TI06_Details of Export MoveIn_WMS_DB].[INBD DT], [TI06_Details of Export MoveIn_WMS_DB].[Inbound ED], [TI06_Details of Export MoveIn_WMS_DB].[DECLRATION NO], [TI06_Details of Export MoveIn_WMS_DB].[LINE NO], [TI07_Details of EXP MoveIn_TIFFA_DB].[COMMODITY DESCRIPTION IN ENGLISH] AS COMMODITY, [TI06_Details of Export MoveIn_WMS_DB].[PJT CD], [TI06_Details of Export MoveIn_WMS_DB].[SUP PO NO], [TI06_Details of Export MoveIn_WMS_DB].[PRT NO], [TI06_Details of Export MoveIn_WMS_DB].QTY, [TI06_Details of Export MoveIn_WMS_DB].UNIT, [TI07_Details of EXP MoveIn_TIFFA_DB].[UNIT PRICE], [TI07_Details of EXP MoveIn_TIFFA_DB].[FOB PRICE (FOREIGN CURRENCY], [TI07_Details of EXP MoveIn_TIFFA_DB].Currency, [TI07_Details of EXP MoveIn_TIFFA_DB].[EX RATE], [TI07_Details of EXP MoveIn_TIFFA_DB].[PRICE FOB (THAI BAHT)], [TI07_Details of EXP MoveIn_TIFFA_DB].STATUS " +
                    /*[TI08_Details of Export MoveIn_Final]*/
                        "FROM [TI06_Details of Export MoveIn_WMS_DB] LEFT JOIN [TI07_Details of EXP MoveIn_TIFFA_DB] ON [TI06_Details of Export MoveIn_WMS_DB].KeyCode4 = [TI07_Details of EXP MoveIn_TIFFA_DB].KeyCode4 " +
                        "GROUP BY [TI06_Details of Export MoveIn_WMS_DB].[INBD DT], [TI06_Details of Export MoveIn_WMS_DB].[Inbound ED], [TI06_Details of Export MoveIn_WMS_DB].[DECLRATION NO], [TI06_Details of Export MoveIn_WMS_DB].[LINE NO], [TI07_Details of EXP MoveIn_TIFFA_DB].[COMMODITY DESCRIPTION IN ENGLISH], [TI06_Details of Export MoveIn_WMS_DB].[PJT CD], [TI06_Details of Export MoveIn_WMS_DB].[SUP PO NO], [TI06_Details of Export MoveIn_WMS_DB].[PRT NO], [TI06_Details of Export MoveIn_WMS_DB].QTY, [TI06_Details of Export MoveIn_WMS_DB].UNIT, [TI07_Details of EXP MoveIn_TIFFA_DB].[UNIT PRICE], [TI07_Details of EXP MoveIn_TIFFA_DB].[FOB PRICE (FOREIGN CURRENCY], [TI07_Details of EXP MoveIn_TIFFA_DB].Currency, [TI07_Details of EXP MoveIn_TIFFA_DB].[EX RATE], [TI07_Details of EXP MoveIn_TIFFA_DB].[PRICE FOB (THAI BAHT)], [TI07_Details of EXP MoveIn_TIFFA_DB].STATUS; ";
            }
            return s_cmd;
        }
        public static string QIO01_InOutRecord_Draft()
        {
            string s_cmd ="";
            if (TableTrucated("TIO01_InOutRecord_DB"))
            {
                s_cmd = "INSERT INTO TIO01_InOutRecord_DB([MI DECLRATION NO],[MI LINE NO],[INBD DT],[MI INV NO],[SUP PO NO],[COMMODITY],[PRT NO],[CASE NO],[MI QTY],[MI UNIT],[MI UNIT PRICE],[MI CURRENCY],[MI EX RATE],[MI STATUS],[MO DECLRATION NO],[MO LINE NO],[OUTBD DT],[MO INV NO],[CON PO NO],[MO QTY],[MO UNIT],[MO UNIT PRICE],[MO CURRENCY],[MO EX RATE],[MO STATUS] ) "+
                        "SELECT TI05_InBoundActual_DB_Final.[DECLRATION NO] AS [MI DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO] AS [MI LINE NO], TI05_InBoundActual_DB_Final.[INBD DT], TI05_InBoundActual_DB_Final.[INVOICE NO#] AS [MI INV NO], TI05_InBoundActual_DB_Final.[SUP PO NO], TI05_InBoundActual_DB_Final.[COMMODITY DESCRIPTION IN ENGLISH] AS COMMODITY, TI05_InBoundActual_DB_Final.[PRT NO], TI05_InBoundActual_DB_Final.[CASE NO], TI05_InBoundActual_DB_Final.QTY AS [MI QTY], TI05_InBoundActual_DB_Final.UNIT AS [MI UNIT], TI05_InBoundActual_DB_Final.[UNIT PRICE] AS [MI UNIT PRICE], TI05_InBoundActual_DB_Final.Currency AS [MI CURRENCY], TI05_InBoundActual_DB_Final.[EX RATE] AS [MI EX RATE], TI05_InBoundActual_DB_Final.STATUS AS [MI STATUS], TO05_OutBoundActual_DB_Final.[DECLRATION NO] AS [MO DECLRATION NO], TO05_OutBoundActual_DB_Final.[LINE NO] AS [MO LINE NO], TO05_OutBoundActual_DB_Final.[OUTBD DT], TO05_OutBoundActual_DB_Final.[INV NO] AS [MO INV NO], TO05_OutBoundActual_DB_Final.[CON PO NO], TO05_OutBoundActual_DB_Final.QTY AS [MO QTY], TO05_OutBoundActual_DB_Final.UNIT AS [MO UNIT], TO05_OutBoundActual_DB_Final.[UNIT PRICE] AS [MO UNIT PRICE], TO05_OutBoundActual_DB_Final.Currency AS [MO CURRENCY], TO05_OutBoundActual_DB_Final.[EX RATE] AS [MO EX RATE], TO05_OutBoundActual_DB_Final.STATUS AS [MO STATUS] " +
                    /*INTO TIO01_InOutRecord_DB*/
                        "FROM TI05_InBoundActual_DB_Final LEFT JOIN TO05_OutBoundActual_DB_Final ON TI05_InBoundActual_DB_Final.[CASE NO] = TO05_OutBoundActual_DB_Final.[CASE NO] " +
                        "GROUP BY TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TI05_InBoundActual_DB_Final.[INBD DT], TI05_InBoundActual_DB_Final.[INVOICE NO#], TI05_InBoundActual_DB_Final.[SUP PO NO], TI05_InBoundActual_DB_Final.[COMMODITY DESCRIPTION IN ENGLISH], TI05_InBoundActual_DB_Final.[PRT NO], TI05_InBoundActual_DB_Final.[CASE NO], TI05_InBoundActual_DB_Final.QTY, TI05_InBoundActual_DB_Final.UNIT, TI05_InBoundActual_DB_Final.[UNIT PRICE], TI05_InBoundActual_DB_Final.Currency, TI05_InBoundActual_DB_Final.[EX RATE], TI05_InBoundActual_DB_Final.STATUS, TO05_OutBoundActual_DB_Final.[DECLRATION NO], TO05_OutBoundActual_DB_Final.[LINE NO], TO05_OutBoundActual_DB_Final.[OUTBD DT], TO05_OutBoundActual_DB_Final.[INV NO], TO05_OutBoundActual_DB_Final.[CON PO NO], TO05_OutBoundActual_DB_Final.QTY, TO05_OutBoundActual_DB_Final.UNIT, TO05_OutBoundActual_DB_Final.[UNIT PRICE], TO05_OutBoundActual_DB_Final.Currency, TO05_OutBoundActual_DB_Final.[EX RATE], TO05_OutBoundActual_DB_Final.STATUS, TI05_InBoundActual_DB_Final.[INBD DT] " +
                        "ORDER BY TI05_InBoundActual_DB_Final.[INBD DT], TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TO05_OutBoundActual_DB_Final.[OUTBD DT]; ";
            }
            return s_cmd;
        }
        public static string QIO02_InOutRecord_Draft()
        {
            string s_cmd = "";
            if (TableTrucated("TIO02_InOutRecord_Draft"))
            {
                s_cmd = "INSERT INTO TIO02_InOutRecord_Draft([MI DECLRATION NO],[MI LINE NO],[INBD DT],[MI INV NO],[SUP PO NO],[COMMODITY],[PRT NO],[MI QTY],[MI UNIT],[MI UNIT PRICE],[MI CURRENCY],[MI EX RATE],[MI STATUS],[MO DECLRATION NO],[MO LINE NO],[OUTBD DT],[MO INV NO],[CON PO NO],[MO QTY],[MO UNIT],[MO UNIT PRICE],[MO CURRENCY],[MO EX RATE],[MO STATUS] ) "+
                        "SELECT TIO01_InOutRecord_DB.[MI DECLRATION NO], TIO01_InOutRecord_DB.[MI LINE NO], TIO01_InOutRecord_DB.[INBD DT], TIO01_InOutRecord_DB.[MI INV NO], TIO01_InOutRecord_DB.[SUP PO NO], TIO01_InOutRecord_DB.COMMODITY, TIO01_InOutRecord_DB.[PRT NO], Sum(TIO01_InOutRecord_DB.[MI QTY]) AS [MI QTY], TIO01_InOutRecord_DB.[MI UNIT], TIO01_InOutRecord_DB.[MI UNIT PRICE], TIO01_InOutRecord_DB.[MI CURRENCY], TIO01_InOutRecord_DB.[MI EX RATE], TIO01_InOutRecord_DB.[MI STATUS], TIO01_InOutRecord_DB.[MO DECLRATION NO], TIO01_InOutRecord_DB.[MO LINE NO], TIO01_InOutRecord_DB.[OUTBD DT], TIO01_InOutRecord_DB.[MO INV NO], TIO01_InOutRecord_DB.[CON PO NO], Sum(TIO01_InOutRecord_DB.[MO QTY]) AS [MO QTY], TIO01_InOutRecord_DB.[MO UNIT], TIO01_InOutRecord_DB.[MO UNIT PRICE], TIO01_InOutRecord_DB.[MO CURRENCY], TIO01_InOutRecord_DB.[MO EX RATE], TIO01_InOutRecord_DB.[MO STATUS] " +
                    /*INTO TIO02_InOutRecord_Draft*/
                        "FROM TIO01_InOutRecord_DB " +
                        "GROUP BY TIO01_InOutRecord_DB.[MI DECLRATION NO], TIO01_InOutRecord_DB.[MI LINE NO], TIO01_InOutRecord_DB.[INBD DT], TIO01_InOutRecord_DB.[MI INV NO], TIO01_InOutRecord_DB.[SUP PO NO], TIO01_InOutRecord_DB.COMMODITY, TIO01_InOutRecord_DB.[PRT NO], TIO01_InOutRecord_DB.[MI UNIT], TIO01_InOutRecord_DB.[MI UNIT PRICE], TIO01_InOutRecord_DB.[MI CURRENCY], TIO01_InOutRecord_DB.[MI EX RATE], TIO01_InOutRecord_DB.[MI STATUS], TIO01_InOutRecord_DB.[MO DECLRATION NO], TIO01_InOutRecord_DB.[MO LINE NO], TIO01_InOutRecord_DB.[OUTBD DT], TIO01_InOutRecord_DB.[MO INV NO], TIO01_InOutRecord_DB.[CON PO NO], TIO01_InOutRecord_DB.[MO UNIT], TIO01_InOutRecord_DB.[MO UNIT PRICE], TIO01_InOutRecord_DB.[MO CURRENCY], TIO01_InOutRecord_DB.[MO EX RATE], TIO01_InOutRecord_DB.[MO STATUS], TIO01_InOutRecord_DB.[INBD DT] " +
                        "ORDER BY TIO01_InOutRecord_DB.[INBD DT], TIO01_InOutRecord_DB.[MI DECLRATION NO], TIO01_InOutRecord_DB.[MI LINE NO], TIO01_InOutRecord_DB.[MO DECLRATION NO], TIO01_InOutRecord_DB.[MO LINE NO]; ";
            }
            return s_cmd;
        }
        public static string QIO03_InOutRecord_FINAL()
        {
            string s_cmd ="";
            if (TableTrucated("TIO03_InOutRecord_FINAL"))
            {
                s_cmd = "INSERT INTO [TIO03_InOutRecord_FINAL]([MI DECLRATION NO],[MI LINE NO],[INBD DT],[MI INV NO],[SUP PO NO],[COMMODITY],[PRT NO],[MI QTY],[MI UNIT],[MI UNIT PRICE],[MI FOB(FOREIGN)],[MI CURRENCY],[MI EX RATE],[MI FOB(THB)],[MI STATUS],[MO DECLRATION NO],[MO LINE NO] ,[OUTBD DT],[MO INV NO],[CON PO NO],[MO QTY],[MO UNIT],[MO UNIT PRICE],[MO FOB(FOREIGN)],[MO CURRENCY],[MO EX RATE],[MO FOB(THB)],[MO STATUS],[BALANCE])" +
                        "SELECT TIO02_InOutRecord_Draft.[MI DECLRATION NO], TIO02_InOutRecord_Draft.[MI LINE NO], TIO02_InOutRecord_Draft.[INBD DT], TIO02_InOutRecord_Draft.[MI INV NO], TIO02_InOutRecord_Draft.[SUP PO NO], TIO02_InOutRecord_Draft.COMMODITY, TIO02_InOutRecord_Draft.[PRT NO], TIO02_InOutRecord_Draft.[MI QTY], TIO02_InOutRecord_Draft.[MI UNIT], TIO02_InOutRecord_Draft.[MI UNIT PRICE], Round([TIO02_InOutRecord_Draft].[MI QTY]*[TIO02_InOutRecord_Draft].[MI UNIT PRICE],2) AS [MI FOB(FOREIGN)], TIO02_InOutRecord_Draft.[MI CURRENCY], TIO02_InOutRecord_Draft.[MI EX RATE], Round([TIO02_InOutRecord_Draft].[MI QTY]*[TIO02_InOutRecord_Draft].[MI UNIT PRICE]*[TIO02_InOutRecord_Draft].[MI EX RATE],2) AS [MI FOB(THB)], TIO02_InOutRecord_Draft.[MI STATUS], TIO02_InOutRecord_Draft.[MO DECLRATION NO], TIO02_InOutRecord_Draft.[MO LINE NO], TIO02_InOutRecord_Draft.[OUTBD DT], TIO02_InOutRecord_Draft.[MO INV NO], TIO02_InOutRecord_Draft.[CON PO NO], TIO02_InOutRecord_Draft.[MO QTY], TIO02_InOutRecord_Draft.[MO UNIT], TIO02_InOutRecord_Draft.[MO UNIT PRICE], Round([TIO02_InOutRecord_Draft].[MO QTY]*[TIO02_InOutRecord_Draft].[MO UNIT PRICE],2) AS [MO FOB(FOREIGN)], TIO02_InOutRecord_Draft.[MO CURRENCY], TIO02_InOutRecord_Draft.[MO EX RATE], Round([TIO02_InOutRecord_Draft].[MO QTY]*[TIO02_InOutRecord_Draft].[MO UNIT PRICE]*[TIO02_InOutRecord_Draft].[MO EX RATE],2) AS [MO FOB(THB)], TIO02_InOutRecord_Draft.[MO STATUS], [TIO02_InOutRecord_Draft].[MI QTY]-[TIO02_InOutRecord_Draft].[MO QTY] AS BALANCE " +
                    /*INTO TIO03_InOutRecord_FINAL*/
                        "FROM TIO02_InOutRecord_Draft " +
                        "GROUP BY TIO02_InOutRecord_Draft.[MI DECLRATION NO], TIO02_InOutRecord_Draft.[MI LINE NO], TIO02_InOutRecord_Draft.[INBD DT], TIO02_InOutRecord_Draft.[MI INV NO], TIO02_InOutRecord_Draft.[SUP PO NO], TIO02_InOutRecord_Draft.COMMODITY, TIO02_InOutRecord_Draft.[PRT NO], TIO02_InOutRecord_Draft.[MI QTY], TIO02_InOutRecord_Draft.[MI UNIT], TIO02_InOutRecord_Draft.[MI UNIT PRICE], Round([TIO02_InOutRecord_Draft].[MI QTY]*[TIO02_InOutRecord_Draft].[MI UNIT PRICE],2), TIO02_InOutRecord_Draft.[MI CURRENCY], TIO02_InOutRecord_Draft.[MI EX RATE], Round([TIO02_InOutRecord_Draft].[MI QTY]*[TIO02_InOutRecord_Draft].[MI UNIT PRICE]*[TIO02_InOutRecord_Draft].[MI EX RATE],2), TIO02_InOutRecord_Draft.[MI STATUS], TIO02_InOutRecord_Draft.[MO DECLRATION NO], TIO02_InOutRecord_Draft.[MO LINE NO], TIO02_InOutRecord_Draft.[OUTBD DT], TIO02_InOutRecord_Draft.[MO INV NO], TIO02_InOutRecord_Draft.[CON PO NO], TIO02_InOutRecord_Draft.[MO QTY], TIO02_InOutRecord_Draft.[MO UNIT], TIO02_InOutRecord_Draft.[MO UNIT PRICE], Round([TIO02_InOutRecord_Draft].[MO QTY]*[TIO02_InOutRecord_Draft].[MO UNIT PRICE],2), TIO02_InOutRecord_Draft.[MO CURRENCY], TIO02_InOutRecord_Draft.[MO EX RATE], Round([TIO02_InOutRecord_Draft].[MO QTY]*[TIO02_InOutRecord_Draft].[MO UNIT PRICE]*[TIO02_InOutRecord_Draft].[MO EX RATE],2), TIO02_InOutRecord_Draft.[MO STATUS], [TIO02_InOutRecord_Draft].[MI QTY]-[TIO02_InOutRecord_Draft].[MO QTY], TIO02_InOutRecord_Draft.[INBD DT] " +
                        "ORDER BY TIO02_InOutRecord_Draft.[INBD DT], TIO02_InOutRecord_Draft.[MI DECLRATION NO], TIO02_InOutRecord_Draft.[MI LINE NO]; ";
            }
            return s_cmd;
        }
        public static string QO01_OutBoundActual_KeyCode() 
        {
            string s_cmd="";
            if (TableTrucated("TO01_OutBoundActual_KeyCode"))
            {
                s_cmd = "INSERT INTO TO01_OutBoundActual_KeyCode " +
                        "SELECT CONCAT([T04_OutBoundActual_DB].[Outbound ED] ,'-', [T04_OutBoundActual_DB].[CON PO NO] ,'-', [T04_OutBoundActual_DB].[PRT NO 2]) AS KeyCode, T04_OutBoundActual_DB.[OUTBD NO], [T04_OutBoundActual_DB].[ROW NO] AS [ROW NO], [T04_OutBoundActual_DB].[DETAIL NO] AS [DETAIL NO], T04_OutBoundActual_DB.[PO DT], T04_OutBoundActual_DB.[CON PO NO], T04_OutBoundActual_DB.[INV NO], T04_OutBoundActual_DB.[OUTBD DT], T04_OutBoundActual_DB.[PRT NO], T04_OutBoundActual_DB.[CASE NO], T04_OutBoundActual_DB.[PROD CAT], T04_OutBoundActual_DB.LCTN, T04_OutBoundActual_DB.BOI, T04_OutBoundActual_DB.[SUP PO NO], T04_OutBoundActual_DB.[PJT CD], T04_OutBoundActual_DB.[INBD DT], T04_OutBoundActual_DB.QTY, T04_OutBoundActual_DB.UNIT, T04_OutBoundActual_DB.NW, T04_OutBoundActual_DB.M3, T04_OutBoundActual_DB.[ITEM SECTION], T04_OutBoundActual_DB.[QTY/UNIT], T04_OutBoundActual_DB.GW, T04_OutBoundActual_DB.[Box Size], T04_OutBoundActual_DB.COO, T04_OutBoundActual_DB.[Original Lot No], T04_OutBoundActual_DB.[Inbound ED], T04_OutBoundActual_DB.[Outbound ED] " +
                    /*INTO TO01_OutBoundActual_KeyCode*/
                        "FROM T04_OutBoundActual_DB; ";
            }
            return s_cmd;
        }
        public static string QO02_CustomsData_MoveOut_KeyCode()
        {
            string s_cmd = "";
            if (TableTrucated("TO02_CustomsData_MoveOut_KeyCode"))
            {
                s_cmd = "INSERT INTO TO02_CustomsData_MoveOut_KeyCode " +
                        "SELECT CONCAT([T06_CustomsData_MoveOut_Original].[DECLRATION NO] ,'-', [T06_CustomsData_MoveOut_Original].[PO NO] ,'-', [T06_CustomsData_MoveOut_Original].[PART NO]) AS KeyCode, T06_CustomsData_MoveOut_Original.[DECLRATION NO], T06_CustomsData_MoveOut_Original.[LINE NO], T06_CustomsData_MoveOut_Original.[COMMODITY DESCRIPTION IN ENGLISH], T06_CustomsData_MoveOut_Original.[PO NO], T06_CustomsData_MoveOut_Original.[EXPORT DATE], T06_CustomsData_MoveOut_Original.[SUBMIT DATE], T06_CustomsData_MoveOut_Original.[INVOICE NO#], T06_CustomsData_MoveOut_Original.[INVOICE DATE], T06_CustomsData_MoveOut_Original.[QUANTITY (UNIT)], T06_CustomsData_MoveOut_Original.UNIT, T06_CustomsData_MoveOut_Original.[PART NO], T06_CustomsData_MoveOut_Original.[UNIT PRICE], T06_CustomsData_MoveOut_Original.[FOB PRICE (FOREIGN CURRENCY], T06_CustomsData_MoveOut_Original.Currency, T06_CustomsData_MoveOut_Original.[EX RATE], T06_CustomsData_MoveOut_Original.[PRICE FOB (THAI BAHT)], T06_CustomsData_MoveOut_Original.STATUS " +
                    /*INTO TO02_CustomsData_MoveOut_KeyCode*/
                        "FROM T06_CustomsData_MoveOut_Original " +
                        "WHERE (((T06_CustomsData_MoveOut_Original.[COMMODITY DESCRIPTION IN ENGLISH]) NOT LIKE '%PALLET%')); ";
            }
            return s_cmd;
        }
        public static string QO03_OutBoundActual_LineNo()
        {
            string s_cmd = "";
            if (TableTrucated("TO03_OutBoundActual_LineNo"))
            {
                s_cmd = "INSERT INTO TO03_OutBoundActual_LineNo "+
                        "SELECT TO02_CustomsData_MoveOut_KeyCode.[DECLRATION NO], TO02_CustomsData_MoveOut_KeyCode.[LINE NO], TO01_OutBoundActual_KeyCode.[OUTBD NO], TO01_OutBoundActual_KeyCode.[ROW NO], TO01_OutBoundActual_KeyCode.[DETAIL NO], TO01_OutBoundActual_KeyCode.[PO DT], TO01_OutBoundActual_KeyCode.[CON PO NO], TO01_OutBoundActual_KeyCode.[INV NO], TO01_OutBoundActual_KeyCode.[OUTBD DT], TO01_OutBoundActual_KeyCode.[PRT NO], TO01_OutBoundActual_KeyCode.[CASE NO], TO01_OutBoundActual_KeyCode.[PROD CAT], TO01_OutBoundActual_KeyCode.LCTN, TO01_OutBoundActual_KeyCode.BOI, TO01_OutBoundActual_KeyCode.[SUP PO NO], TO01_OutBoundActual_KeyCode.[PJT CD], TO01_OutBoundActual_KeyCode.[INBD DT], TO01_OutBoundActual_KeyCode.QTY, TO01_OutBoundActual_KeyCode.UNIT, TO01_OutBoundActual_KeyCode.NW, TO01_OutBoundActual_KeyCode.M3, TO01_OutBoundActual_KeyCode.[ITEM SECTION], TO01_OutBoundActual_KeyCode.[QTY/UNIT], TO01_OutBoundActual_KeyCode.GW, TO01_OutBoundActual_KeyCode.[Box Size], TO01_OutBoundActual_KeyCode.COO, TO01_OutBoundActual_KeyCode.[Original Lot No], TO01_OutBoundActual_KeyCode.[Inbound ED], TO01_OutBoundActual_KeyCode.[Outbound ED], TO02_CustomsData_MoveOut_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TO02_CustomsData_MoveOut_KeyCode.[INVOICE NO#], TO02_CustomsData_MoveOut_KeyCode.[QUANTITY (UNIT)], TO02_CustomsData_MoveOut_KeyCode.[UNIT PRICE], TO02_CustomsData_MoveOut_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TO02_CustomsData_MoveOut_KeyCode.Currency, TO02_CustomsData_MoveOut_KeyCode.[EX RATE], TO02_CustomsData_MoveOut_KeyCode.[PRICE FOB (THAI BAHT)], TO02_CustomsData_MoveOut_KeyCode.STATUS, TO01_OutBoundActual_KeyCode.KeyCode " +
                    /*INTO TO03_OutBoundActual_LineNo*/
                        "FROM TO01_OutBoundActual_KeyCode LEFT JOIN TO02_CustomsData_MoveOut_KeyCode ON TO01_OutBoundActual_KeyCode.KeyCode = TO02_CustomsData_MoveOut_KeyCode.KeyCode " +
                        "GROUP BY TO02_CustomsData_MoveOut_KeyCode.[DECLRATION NO], TO02_CustomsData_MoveOut_KeyCode.[LINE NO], TO01_OutBoundActual_KeyCode.[OUTBD NO], TO01_OutBoundActual_KeyCode.[ROW NO], TO01_OutBoundActual_KeyCode.[DETAIL NO], TO01_OutBoundActual_KeyCode.[PO DT], TO01_OutBoundActual_KeyCode.[CON PO NO], TO01_OutBoundActual_KeyCode.[INV NO], TO01_OutBoundActual_KeyCode.[OUTBD DT], TO01_OutBoundActual_KeyCode.[PRT NO], TO01_OutBoundActual_KeyCode.[CASE NO], TO01_OutBoundActual_KeyCode.[PROD CAT], TO01_OutBoundActual_KeyCode.LCTN, TO01_OutBoundActual_KeyCode.BOI, TO01_OutBoundActual_KeyCode.[SUP PO NO], TO01_OutBoundActual_KeyCode.[PJT CD], TO01_OutBoundActual_KeyCode.[INBD DT], TO01_OutBoundActual_KeyCode.QTY, TO01_OutBoundActual_KeyCode.UNIT, TO01_OutBoundActual_KeyCode.NW, TO01_OutBoundActual_KeyCode.M3, TO01_OutBoundActual_KeyCode.[ITEM SECTION], TO01_OutBoundActual_KeyCode.[QTY/UNIT], TO01_OutBoundActual_KeyCode.GW, TO01_OutBoundActual_KeyCode.[Box Size], TO01_OutBoundActual_KeyCode.COO, TO01_OutBoundActual_KeyCode.[Original Lot No], TO01_OutBoundActual_KeyCode.[Inbound ED], TO01_OutBoundActual_KeyCode.[Outbound ED], TO02_CustomsData_MoveOut_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TO02_CustomsData_MoveOut_KeyCode.[INVOICE NO#], TO02_CustomsData_MoveOut_KeyCode.[QUANTITY (UNIT)], TO02_CustomsData_MoveOut_KeyCode.[UNIT PRICE], TO02_CustomsData_MoveOut_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TO02_CustomsData_MoveOut_KeyCode.Currency, TO02_CustomsData_MoveOut_KeyCode.[EX RATE], TO02_CustomsData_MoveOut_KeyCode.[PRICE FOB (THAI BAHT)], TO02_CustomsData_MoveOut_KeyCode.STATUS, TO01_OutBoundActual_KeyCode.KeyCode, TO01_OutBoundActual_KeyCode.[OUTBD DT] ";
            }
            return s_cmd;
        }
        public static string QO04_Duplicate_Check()
        {
            string s_cmd ="";
            if (TableTrucated("TO04_OutBound_Duplicate_CheckMaster"))
            {
                s_cmd = "INSERT INTO TO04_OutBound_Duplicate_CheckMaster " +
                        "SELECT TO03_OutBoundActual_LineNo.[DECLRATION NO], TO03_OutBoundActual_LineNo.[OUTBD DT], TO03_OutBoundActual_LineNo.[INV NO], TO03_OutBoundActual_LineNo.[CON PO NO], TO03_OutBoundActual_LineNo.[PRT NO], TO03_OutBoundActual_LineNo.[CASE NO], Count(TO03_OutBoundActual_LineNo.[CASE NO]) AS [DATA CHECK], TO03_OutBoundActual_LineNo.KeyCode " +
                    /*INTO TO04_OutBound_Duplicate_CheckMaster*/
                        "FROM TO03_OutBoundActual_LineNo " +
                        "GROUP BY TO03_OutBoundActual_LineNo.[DECLRATION NO], TO03_OutBoundActual_LineNo.[OUTBD DT], TO03_OutBoundActual_LineNo.[INV NO], TO03_OutBoundActual_LineNo.[CON PO NO], TO03_OutBoundActual_LineNo.[PRT NO], TO03_OutBoundActual_LineNo.[CASE NO], TO03_OutBoundActual_LineNo.KeyCode " +
                        "HAVING (((Count(TO03_OutBoundActual_LineNo.[CASE NO]))>1)); ";
            }
            return s_cmd;
        }
        public static string QO05_OutBoundActual_LineNo_Check()
        {
            string s_cmd ="";
            if (TableTrucated("TO05_OutBoundActual_DB_Final"))
            {
                s_cmd = "INSERT INTO TO05_OutBoundActual_DB_Final ([KeyCode3],[CTN],[DECLRATION NO],[LINE NO],[OUTBD NO],[ROW NO],[DETAIL NO],[PO DT],[CON PO NO],[INV NO],[OUTBD DT],[PRT NO],[CASE NO],[PROD CAT],[LCTN],[BOI],[SUP PO NO],[PJT CD],[INBD DT],[QTY],[UNIT],[NW],[M3],[ITEM SECTION],[QTY/UNIT],[GW],[Box Size],[COO],[Original Lot No],[Inbound ED],[Outbound ED],[COMMODITY DESCRIPTION IN ENGLISH],[INVOICE NO#],[QUANTITY (UNIT)],[UNIT PRICE],[FOB PRICE (FOREIGN CURRENCY],[Currency],[EX RATE],[PRICE FOB (THAI BAHT)],[STATUS],[KeyCode]) " +
                        "SELECT CONCAT(TO04_OutBound_Duplicate_CheckMaster.[DATA CHECK], [TO03_OutBoundActual_LineNo].[QUANTITY (UNIT)] ,'-', [TO03_OutBoundActual_LineNo].[KeyCode]) AS KeyCode3, " +
                        "[TO03_OutBoundActual_LineNo].[QUANTITY (UNIT)]/[TO03_OutBoundActual_LineNo].[QTY] AS CTN, TO03_OutBoundActual_LineNo.[DECLRATION NO], TO03_OutBoundActual_LineNo.[LINE NO], TO03_OutBoundActual_LineNo.[OUTBD NO], TO03_OutBoundActual_LineNo.[ROW NO], TO03_OutBoundActual_LineNo.[DETAIL NO], TO03_OutBoundActual_LineNo.[PO DT], TO03_OutBoundActual_LineNo.[CON PO NO], TO03_OutBoundActual_LineNo.[INV NO], TO03_OutBoundActual_LineNo.[OUTBD DT], TO03_OutBoundActual_LineNo.[PRT NO], TO03_OutBoundActual_LineNo.[CASE NO], TO03_OutBoundActual_LineNo.[PROD CAT], TO03_OutBoundActual_LineNo.LCTN, TO03_OutBoundActual_LineNo.BOI, TO03_OutBoundActual_LineNo.[SUP PO NO], TO03_OutBoundActual_LineNo.[PJT CD], TO03_OutBoundActual_LineNo.[INBD DT], TO03_OutBoundActual_LineNo.QTY, TO03_OutBoundActual_LineNo.UNIT, TO03_OutBoundActual_LineNo.NW, TO03_OutBoundActual_LineNo.M3, TO03_OutBoundActual_LineNo.[ITEM SECTION], TO03_OutBoundActual_LineNo.[QTY/UNIT], TO03_OutBoundActual_LineNo.GW, TO03_OutBoundActual_LineNo.[Box Size], TO03_OutBoundActual_LineNo.COO, TO03_OutBoundActual_LineNo.[Original Lot No], TO03_OutBoundActual_LineNo.[Inbound ED], TO03_OutBoundActual_LineNo.[Outbound ED], TO03_OutBoundActual_LineNo.[COMMODITY DESCRIPTION IN ENGLISH], TO03_OutBoundActual_LineNo.[INVOICE NO#], TO03_OutBoundActual_LineNo.[QUANTITY (UNIT)], TO03_OutBoundActual_LineNo.[UNIT PRICE], TO03_OutBoundActual_LineNo.[FOB PRICE (FOREIGN CURRENCY], TO03_OutBoundActual_LineNo.Currency, TO03_OutBoundActual_LineNo.[EX RATE], TO03_OutBoundActual_LineNo.[PRICE FOB (THAI BAHT)], TO03_OutBoundActual_LineNo.STATUS, TO03_OutBoundActual_LineNo.KeyCode  " +
                    /*INTO TO05_OutBoundActual_DB_Final*/
                        "FROM TO03_OutBoundActual_LineNo LEFT JOIN TO04_OutBound_Duplicate_CheckMaster ON TO03_OutBoundActual_LineNo.KeyCode = TO04_OutBound_Duplicate_CheckMaster.KeyCode " +
                        "GROUP BY TO04_OutBound_Duplicate_CheckMaster.[DATA CHECK], CONCAT([TO03_OutBoundActual_LineNo].[QUANTITY (UNIT)],'-',[TO03_OutBoundActual_LineNo].[KeyCode]), [TO03_OutBoundActual_LineNo].[QUANTITY (UNIT)]/[TO03_OutBoundActual_LineNo].[QTY], TO03_OutBoundActual_LineNo.[DECLRATION NO], TO03_OutBoundActual_LineNo.[LINE NO], TO03_OutBoundActual_LineNo.[OUTBD NO], TO03_OutBoundActual_LineNo.[ROW NO], TO03_OutBoundActual_LineNo.[DETAIL NO], TO03_OutBoundActual_LineNo.[PO DT], TO03_OutBoundActual_LineNo.[CON PO NO], TO03_OutBoundActual_LineNo.[INV NO], TO03_OutBoundActual_LineNo.[OUTBD DT], TO03_OutBoundActual_LineNo.[PRT NO], TO03_OutBoundActual_LineNo.[CASE NO], TO03_OutBoundActual_LineNo.[PROD CAT], TO03_OutBoundActual_LineNo.LCTN, TO03_OutBoundActual_LineNo.BOI, TO03_OutBoundActual_LineNo.[SUP PO NO], TO03_OutBoundActual_LineNo.[PJT CD], TO03_OutBoundActual_LineNo.[INBD DT], TO03_OutBoundActual_LineNo.QTY, TO03_OutBoundActual_LineNo.UNIT, TO03_OutBoundActual_LineNo.NW, TO03_OutBoundActual_LineNo.M3, TO03_OutBoundActual_LineNo.[ITEM SECTION], TO03_OutBoundActual_LineNo.[QTY/UNIT], TO03_OutBoundActual_LineNo.GW, TO03_OutBoundActual_LineNo.[Box Size], TO03_OutBoundActual_LineNo.COO, TO03_OutBoundActual_LineNo.[Original Lot No], TO03_OutBoundActual_LineNo.[Inbound ED], TO03_OutBoundActual_LineNo.[Outbound ED], TO03_OutBoundActual_LineNo.[COMMODITY DESCRIPTION IN ENGLISH], TO03_OutBoundActual_LineNo.[INVOICE NO#], TO03_OutBoundActual_LineNo.[QUANTITY (UNIT)], TO03_OutBoundActual_LineNo.[UNIT PRICE], TO03_OutBoundActual_LineNo.[FOB PRICE (FOREIGN CURRENCY], TO03_OutBoundActual_LineNo.Currency, TO03_OutBoundActual_LineNo.[EX RATE], TO03_OutBoundActual_LineNo.[PRICE FOB (THAI BAHT)], TO03_OutBoundActual_LineNo.STATUS, TO03_OutBoundActual_LineNo.KeyCode, TO03_OutBoundActual_LineNo.[OUTBD DT]; ";
            }
            return s_cmd;
        }
        public static string QO06_Details_of_Export_MoveOut_WMS_DB()
        {
            string s_cmd ="";
            if (TableTrucated("[TO06_Details of Export MoveOut_WMS_DB]"))
            {
                s_cmd = "INSERT INTO [TO06_Details of Export MoveOut_WMS_DB] " +
                        "SELECT CONCAT([TO05_OutBoundActual_DB_Final].[DECLRATION NO],'-',[TO05_OutBoundActual_DB_Final].[LINE NO]) AS KeyCode4, " +
                        "TO05_OutBoundActual_DB_Final.[OUTBD DT], TO05_OutBoundActual_DB_Final.[Outbound ED], TO05_OutBoundActual_DB_Final.[DECLRATION NO], TO05_OutBoundActual_DB_Final.[LINE NO], TO05_OutBoundActual_DB_Final.[PJT CD], TO05_OutBoundActual_DB_Final.[INV NO], TO05_OutBoundActual_DB_Final.[CON PO NO], TO05_OutBoundActual_DB_Final.[PRT NO], Sum(TO05_OutBoundActual_DB_Final.QTY) AS QTY, TO05_OutBoundActual_DB_Final.UNIT  " +
                    /*INTO [TO06_Details of Export MoveOut_WMS_DB]*/
                        "FROM TO05_OutBoundActual_DB_Final " +
                        "GROUP BY CONCAT([TO05_OutBoundActual_DB_Final].[DECLRATION NO],'-',[TO05_OutBoundActual_DB_Final].[LINE NO]), TO05_OutBoundActual_DB_Final.[OUTBD DT], TO05_OutBoundActual_DB_Final.[Outbound ED], TO05_OutBoundActual_DB_Final.[DECLRATION NO], TO05_OutBoundActual_DB_Final.[LINE NO], TO05_OutBoundActual_DB_Final.[PJT CD], TO05_OutBoundActual_DB_Final.[INV NO], TO05_OutBoundActual_DB_Final.[CON PO NO], TO05_OutBoundActual_DB_Final.[PRT NO], TO05_OutBoundActual_DB_Final.UNIT; ";
            }
            return s_cmd;
        }
        public static string QO07_Details_of_EXP_MoveOut_TIFFA_DB()
        {
            string s_cmd ="";
            if (TableTrucated("[TO07_Details of EXP MoveOut_TIFFA_DB]"))
            {
                s_cmd = "INSERT INTO [TO07_Details of EXP MoveOut_TIFFA_DB]" +
                        "SELECT CONCAT([TO02_CustomsData_MoveOut_KeyCode].[DECLRATION NO] ,'-', [TO02_CustomsData_MoveOut_KeyCode].[LINE NO]) AS KeyCode4, TO02_CustomsData_MoveOut_KeyCode.[DECLRATION NO], TO02_CustomsData_MoveOut_KeyCode.[LINE NO], TO02_CustomsData_MoveOut_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TO02_CustomsData_MoveOut_KeyCode.[PO NO], TO02_CustomsData_MoveOut_KeyCode.[EXPORT DATE], TO02_CustomsData_MoveOut_KeyCode.[SUBMIT DATE], TO02_CustomsData_MoveOut_KeyCode.[INVOICE NO#], TO02_CustomsData_MoveOut_KeyCode.[INVOICE DATE], TO02_CustomsData_MoveOut_KeyCode.[QUANTITY (UNIT)], TO02_CustomsData_MoveOut_KeyCode.[PART NO], TO02_CustomsData_MoveOut_KeyCode.[UNIT PRICE], TO02_CustomsData_MoveOut_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TO02_CustomsData_MoveOut_KeyCode.Currency, TO02_CustomsData_MoveOut_KeyCode.[EX RATE], TO02_CustomsData_MoveOut_KeyCode.[PRICE FOB (THAI BAHT)], TO02_CustomsData_MoveOut_KeyCode.STATUS " +
                    /*INTO [TO07_Details of EXP MoveOut_TIFFA_DB]*/
                        "FROM TO02_CustomsData_MoveOut_KeyCode " +
                        "GROUP BY CONCAT([TO02_CustomsData_MoveOut_KeyCode].[DECLRATION NO],'-',[TO02_CustomsData_MoveOut_KeyCode].[LINE NO]), TO02_CustomsData_MoveOut_KeyCode.[DECLRATION NO], TO02_CustomsData_MoveOut_KeyCode.[LINE NO], TO02_CustomsData_MoveOut_KeyCode.[COMMODITY DESCRIPTION IN ENGLISH], TO02_CustomsData_MoveOut_KeyCode.[PO NO], TO02_CustomsData_MoveOut_KeyCode.[EXPORT DATE], TO02_CustomsData_MoveOut_KeyCode.[SUBMIT DATE], TO02_CustomsData_MoveOut_KeyCode.[INVOICE NO#], TO02_CustomsData_MoveOut_KeyCode.[INVOICE DATE], TO02_CustomsData_MoveOut_KeyCode.[QUANTITY (UNIT)], TO02_CustomsData_MoveOut_KeyCode.[PART NO], TO02_CustomsData_MoveOut_KeyCode.[UNIT PRICE], TO02_CustomsData_MoveOut_KeyCode.[FOB PRICE (FOREIGN CURRENCY], TO02_CustomsData_MoveOut_KeyCode.Currency, TO02_CustomsData_MoveOut_KeyCode.[EX RATE], TO02_CustomsData_MoveOut_KeyCode.[PRICE FOB (THAI BAHT)], TO02_CustomsData_MoveOut_KeyCode.STATUS; ";
            }
            return s_cmd;
        }
        public static string QO08_Details_of_Export_MoveOut_Final()
        {
            string s_cmd ="";
            if (TableTrucated("[TO08_Details of Export MoveOut_Final]"))
            {
                s_cmd = "INSERT INTO [TO08_Details of Export MoveOut_Final] " +
                        "SELECT [TO06_Details of Export MoveOut_WMS_DB].[OUTBD DT], [TO06_Details of Export MoveOut_WMS_DB].[Outbound ED], [TO06_Details of Export MoveOut_WMS_DB].[DECLRATION NO], [TO06_Details of Export MoveOut_WMS_DB].[LINE NO], [TO06_Details of Export MoveOut_WMS_DB].[INV NO], [TO06_Details of Export MoveOut_WMS_DB].[PJT CD], [TO06_Details of Export MoveOut_WMS_DB].[CON PO NO], [TO06_Details of Export MoveOut_WMS_DB].[PRT NO], [TO06_Details of Export MoveOut_WMS_DB].QTY, [TO06_Details of Export MoveOut_WMS_DB].UNIT, [TO07_Details of EXP MoveOut_TIFFA_DB].[UNIT PRICE], [TO07_Details of EXP MoveOut_TIFFA_DB].[FOB PRICE (FOREIGN CURRENCY], [TO07_Details of EXP MoveOut_TIFFA_DB].Currency, [TO07_Details of EXP MoveOut_TIFFA_DB].[EX RATE], [TO07_Details of EXP MoveOut_TIFFA_DB].[PRICE FOB (THAI BAHT)], [TO07_Details of EXP MoveOut_TIFFA_DB].STATUS " +
                    /*INTO [TO08_Details of Export MoveOut_Final]*/
                        "FROM [TO06_Details of Export MoveOut_WMS_DB] LEFT JOIN [TO07_Details of EXP MoveOut_TIFFA_DB] ON [TO06_Details of Export MoveOut_WMS_DB].KeyCode4 = [TO07_Details of EXP MoveOut_TIFFA_DB].KeyCode4 " +
                        "GROUP BY [TO06_Details of Export MoveOut_WMS_DB].[OUTBD DT], [TO06_Details of Export MoveOut_WMS_DB].[Outbound ED], [TO06_Details of Export MoveOut_WMS_DB].[DECLRATION NO], [TO06_Details of Export MoveOut_WMS_DB].[LINE NO], [TO06_Details of Export MoveOut_WMS_DB].[INV NO], [TO06_Details of Export MoveOut_WMS_DB].[PJT CD], [TO06_Details of Export MoveOut_WMS_DB].[CON PO NO], [TO06_Details of Export MoveOut_WMS_DB].[PRT NO], [TO06_Details of Export MoveOut_WMS_DB].QTY, [TO06_Details of Export MoveOut_WMS_DB].UNIT, [TO07_Details of EXP MoveOut_TIFFA_DB].[UNIT PRICE], [TO07_Details of EXP MoveOut_TIFFA_DB].[FOB PRICE (FOREIGN CURRENCY], [TO07_Details of EXP MoveOut_TIFFA_DB].Currency, [TO07_Details of EXP MoveOut_TIFFA_DB].[EX RATE], [TO07_Details of EXP MoveOut_TIFFA_DB].[PRICE FOB (THAI BAHT)], [TO07_Details of EXP MoveOut_TIFFA_DB].STATUS; ";
            }
            return s_cmd;
        }
        //public static string QR01_Details_of_Export_MoveIn_Report(DateTime from,DateTime to)
        //{
        //    string s_cmd;
        //    s_cmd = "SELECT [TI08_Details of Export MoveIn_Final].[INBD DT], [TI08_Details of Export MoveIn_Final].[Inbound ED], [TI08_Details of Export MoveIn_Final].[DECLRATION NO], [TI08_Details of Export MoveIn_Final].[LINE NO], [TI08_Details of Export MoveIn_Final].COMMODITY, [TI08_Details of Export MoveIn_Final].[PJT CD], [TI08_Details of Export MoveIn_Final].[SUP PO NO], [TI08_Details of Export MoveIn_Final].[PRT NO], [TI08_Details of Export MoveIn_Final].QTY, [TI08_Details of Export MoveIn_Final].UNIT, [TI08_Details of Export MoveIn_Final].[UNIT PRICE], [TI08_Details of Export MoveIn_Final].[FOB PRICE (FOREIGN CURRENCY], [TI08_Details of Export MoveIn_Final].Currency, [TI08_Details of Export MoveIn_Final].[EX RATE], [TI08_Details of Export MoveIn_Final].[PRICE FOB (THAI BAHT)], [TI08_Details of Export MoveIn_Final].STATUS " +
        //            "FROM [TI08_Details of Export MoveIn_Final] "+
        //            "WHERE ((([TI08_Details of Export MoveIn_Final].[INBD DT]) Between '"+from.ToString("yyyyMMdd")+"' And '"+to.ToString("yyyyMMdd")+"')); ";
        //    return s_cmd;
        //}
        public static string QR01_Details_of_Export_MoveIn_Report(DateTime dateFrom, DateTime dateTo)
        {
            string s_cmd;
            s_cmd = "SELECT [TI08_Details of Export MoveIn_Final].[INBD DT], [TI08_Details of Export MoveIn_Final].[Inbound ED], [TI08_Details of Export MoveIn_Final].[DECLRATION NO], [TI08_Details of Export MoveIn_Final].[LINE NO], [TI08_Details of Export MoveIn_Final].COMMODITY, [TI08_Details of Export MoveIn_Final].[PJT CD], [TI08_Details of Export MoveIn_Final].[SUP PO NO], [TI08_Details of Export MoveIn_Final].[PRT NO], [TI08_Details of Export MoveIn_Final].QTY, [TI08_Details of Export MoveIn_Final].UNIT, [TI08_Details of Export MoveIn_Final].[UNIT PRICE], [TI08_Details of Export MoveIn_Final].[FOB PRICE (FOREIGN CURRENCY], [TI08_Details of Export MoveIn_Final].Currency, [TI08_Details of Export MoveIn_Final].[EX RATE], [TI08_Details of Export MoveIn_Final].[PRICE FOB (THAI BAHT)], [TI08_Details of Export MoveIn_Final].STATUS " +
                    "FROM [TI08_Details of Export MoveIn_Final] "+
                   "WHERE ((([TI08_Details of Export MoveIn_Final].[INBD DT]) Between '" + dateFrom.ToString("yyyyMMdd") + "' And '" + dateTo.ToString("yyyyMMdd") + "')); ";
            return s_cmd;
        }
        public static string QR02_Details_of_Export_MoveOut_Report(DateTime dateFrom, DateTime dateTo)
        {
            string s_cmd;
            s_cmd = "SELECT [TO08_Details of Export MoveOut_Final].[OUTBD DT], [TO08_Details of Export MoveOut_Final].[Outbound ED], [TO08_Details of Export MoveOut_Final].[DECLRATION NO], [TO08_Details of Export MoveOut_Final].[LINE NO], [TO08_Details of Export MoveOut_Final].[INV NO], [TO08_Details of Export MoveOut_Final].[PJT CD], [TO08_Details of Export MoveOut_Final].[CON PO NO], [TO08_Details of Export MoveOut_Final].[PRT NO], [TO08_Details of Export MoveOut_Final].QTY, [TO08_Details of Export MoveOut_Final].UNIT, [TO08_Details of Export MoveOut_Final].[UNIT PRICE], [TO08_Details of Export MoveOut_Final].[FOB PRICE (FOREIGN CURRENCY], [TO08_Details of Export MoveOut_Final].Currency, [TO08_Details of Export MoveOut_Final].[EX RATE], [TO08_Details of Export MoveOut_Final].[PRICE FOB (THAI BAHT)], [TO08_Details of Export MoveOut_Final].STATUS " +
                    "FROM [TO08_Details of Export MoveOut_Final] "+
                    "WHERE ((([TO08_Details of Export MoveOut_Final].[OUTBD DT]) Between '"+ dateFrom.ToString("yyyyMMdd")+"' And '"+ dateTo.ToString("yyyyMMdd")+"')); ";
            return s_cmd;
        }
        public static string QR03_InOutReport(DateTime dateFrom, DateTime dateTo)
        {
            string s_cmd;
            s_cmd = "SELECT TIO03_InOutRecord_FINAL.[MI DECLRATION NO], TIO03_InOutRecord_FINAL.[MI LINE NO], TIO03_InOutRecord_FINAL.[INBD DT], TIO03_InOutRecord_FINAL.[MI INV NO], TIO03_InOutRecord_FINAL.[SUP PO NO], TIO03_InOutRecord_FINAL.COMMODITY, TIO03_InOutRecord_FINAL.[PRT NO], TIO03_InOutRecord_FINAL.[MI QTY], TIO03_InOutRecord_FINAL.[MI UNIT], TIO03_InOutRecord_FINAL.[MI UNIT PRICE], TIO03_InOutRecord_FINAL.[MI FOB(FOREIGN)], TIO03_InOutRecord_FINAL.[MI CURRENCY], TIO03_InOutRecord_FINAL.[MI EX RATE], TIO03_InOutRecord_FINAL.[MI FOB(THB)], TIO03_InOutRecord_FINAL.[MI STATUS], TIO03_InOutRecord_FINAL.[MO DECLRATION NO], TIO03_InOutRecord_FINAL.[MO LINE NO], TIO03_InOutRecord_FINAL.[OUTBD DT], TIO03_InOutRecord_FINAL.[MO INV NO], TIO03_InOutRecord_FINAL.[CON PO NO], TIO03_InOutRecord_FINAL.[MO QTY], TIO03_InOutRecord_FINAL.[MO UNIT], TIO03_InOutRecord_FINAL.[MO UNIT PRICE], TIO03_InOutRecord_FINAL.[MO FOB(FOREIGN)], TIO03_InOutRecord_FINAL.[MO CURRENCY], TIO03_InOutRecord_FINAL.[MO EX RATE], TIO03_InOutRecord_FINAL.[MO FOB(THB)], TIO03_InOutRecord_FINAL.[MO STATUS], TIO03_InOutRecord_FINAL.BALANCE " +
                    "FROM TIO03_InOutRecord_FINAL " +
                    "GROUP BY TIO03_InOutRecord_FINAL.[MI DECLRATION NO], TIO03_InOutRecord_FINAL.[MI LINE NO], TIO03_InOutRecord_FINAL.[INBD DT], TIO03_InOutRecord_FINAL.[MI INV NO], TIO03_InOutRecord_FINAL.[SUP PO NO], TIO03_InOutRecord_FINAL.COMMODITY, TIO03_InOutRecord_FINAL.[PRT NO], TIO03_InOutRecord_FINAL.[MI QTY], TIO03_InOutRecord_FINAL.[MI UNIT], TIO03_InOutRecord_FINAL.[MI UNIT PRICE], TIO03_InOutRecord_FINAL.[MI FOB(FOREIGN)], TIO03_InOutRecord_FINAL.[MI CURRENCY], TIO03_InOutRecord_FINAL.[MI EX RATE], TIO03_InOutRecord_FINAL.[MI FOB(THB)], TIO03_InOutRecord_FINAL.[MI STATUS], TIO03_InOutRecord_FINAL.[MO DECLRATION NO], TIO03_InOutRecord_FINAL.[MO LINE NO], TIO03_InOutRecord_FINAL.[OUTBD DT], TIO03_InOutRecord_FINAL.[MO INV NO], TIO03_InOutRecord_FINAL.[CON PO NO], TIO03_InOutRecord_FINAL.[MO QTY], TIO03_InOutRecord_FINAL.[MO UNIT], TIO03_InOutRecord_FINAL.[MO UNIT PRICE], TIO03_InOutRecord_FINAL.[MO FOB(FOREIGN)], TIO03_InOutRecord_FINAL.[MO CURRENCY], TIO03_InOutRecord_FINAL.[MO EX RATE], TIO03_InOutRecord_FINAL.[MO FOB(THB)], TIO03_InOutRecord_FINAL.[MO STATUS], TIO03_InOutRecord_FINAL.BALANCE "+
                    "HAVING (((TIO03_InOutRecord_FINAL.[INBD DT]) Between '"+ dateFrom.ToString("yyyyMMdd")+ "' And '" + dateTo.ToString("yyyyMMdd") + "')) OR (((TIO03_InOutRecord_FINAL.[OUTBD DT]) Between '" + dateFrom.ToString("yyyyMMdd") + "' And '"+ dateTo.ToString("yyyyMMdd") + "')) ";

            return s_cmd;
        }
        public static string QS01_StockList_Draft()
        {
            string s_cmd ="";
            if (TableTrucated("TS01_StockList_Draft "))
            {
                s_cmd = "INSERT INTO TS01_StockList_Draft " +
                        "SELECT T07_StockBySumCond_Original.[ACTUAL DT] AS [INBD DT], T07_StockBySumCond_Original.[Inbound ED], TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TI05_InBoundActual_DB_Final.[INVOICE NO#], T07_StockBySumCond_Original.[PJT CD], T07_StockBySumCond_Original.[LOT NO] AS [SUP PO NO], T07_StockBySumCond_Original.[・ｿ_PRT NO_] AS [PRT NO], T07_StockBySumCond_Original.[CASE NO], T07_StockBySumCond_Original.LCTN, T07_StockBySumCond_Original.QTY4 AS QTY, T07_StockBySumCond_Original.[ACTUAL QTY4] AS [ACT QTY], T07_StockBySumCond_Original.UNIT4 AS UNIT, TI05_InBoundActual_DB_Final.[UNIT PRICE], TI05_InBoundActual_DB_Final.Currency, TI05_InBoundActual_DB_Final.[EX RATE], TI05_InBoundActual_DB_Final.STATUS, TI05_InBoundActual_DB_Final.[FOB PRICE (FOREIGN CURRENCY], TI05_InBoundActual_DB_Final.[PRICE FOB (THAI BAHT)] " +
                    /*INTO TS01_StockList_Draft*/
                        "FROM T07_StockBySumCond_Original LEFT JOIN TI05_InBoundActual_DB_Final ON T07_StockBySumCond_Original.[CASE NO] = TI05_InBoundActual_DB_Final.[CASE NO] " +
                        "GROUP BY T07_StockBySumCond_Original.[ACTUAL DT], T07_StockBySumCond_Original.[Inbound ED], TI05_InBoundActual_DB_Final.[DECLRATION NO], TI05_InBoundActual_DB_Final.[LINE NO], TI05_InBoundActual_DB_Final.[INVOICE NO#], T07_StockBySumCond_Original.[PJT CD], T07_StockBySumCond_Original.[LOT NO], T07_StockBySumCond_Original.[・ｿ_PRT NO_], T07_StockBySumCond_Original.[CASE NO], T07_StockBySumCond_Original.LCTN, T07_StockBySumCond_Original.QTY4, T07_StockBySumCond_Original.[ACTUAL QTY4], T07_StockBySumCond_Original.UNIT4, TI05_InBoundActual_DB_Final.[UNIT PRICE], TI05_InBoundActual_DB_Final.Currency, TI05_InBoundActual_DB_Final.[EX RATE], TI05_InBoundActual_DB_Final.STATUS, TI05_InBoundActual_DB_Final.[FOB PRICE (FOREIGN CURRENCY], TI05_InBoundActual_DB_Final.[PRICE FOB (THAI BAHT)] " +
                        "ORDER BY T07_StockBySumCond_Original.[ACTUAL DT]; ";
            }
            return s_cmd;
        }
        public static string QS02_StockList_FOB_Draft()
        {
            string s_cmd ="";
            if (TableTrucated("TS02_StockList_FOB_Draft "))
            {
                s_cmd = "INSERT INTO TS02_StockList_FOB_Draft " +
                        "SELECT TS01_StockList_Draft.[INBD DT], TS01_StockList_Draft.[Inbound ED], TS01_StockList_Draft.[DECLRATION NO], TS01_StockList_Draft.[LINE NO], TS01_StockList_Draft.[INVOICE NO#], TS01_StockList_Draft.[PJT CD], TS01_StockList_Draft.[SUP PO NO], TS01_StockList_Draft.[PRT NO], TS01_StockList_Draft.LCTN, Sum(TS01_StockList_Draft.QTY) AS QTY, Sum(TS01_StockList_Draft.[ACT QTY]) AS [ACT QTY], TS01_StockList_Draft.UNIT, TS01_StockList_Draft.[UNIT PRICE], TS01_StockList_Draft.Currency, TS01_StockList_Draft.[EX RATE], TS01_StockList_Draft.STATUS, TS01_StockList_Draft.[FOB PRICE (FOREIGN CURRENCY], TS01_StockList_Draft.[PRICE FOB (THAI BAHT)] " +
                    /*INTO TS02_StockList_FOB_Draft*/
                        "FROM TS01_StockList_Draft " +
                        "GROUP BY TS01_StockList_Draft.[INBD DT], TS01_StockList_Draft.[Inbound ED], TS01_StockList_Draft.[DECLRATION NO], TS01_StockList_Draft.[LINE NO], TS01_StockList_Draft.[INVOICE NO#], TS01_StockList_Draft.[PJT CD], TS01_StockList_Draft.[SUP PO NO], TS01_StockList_Draft.[PRT NO], TS01_StockList_Draft.LCTN, TS01_StockList_Draft.UNIT, TS01_StockList_Draft.[UNIT PRICE], TS01_StockList_Draft.Currency, TS01_StockList_Draft.[EX RATE], TS01_StockList_Draft.STATUS, TS01_StockList_Draft.[FOB PRICE (FOREIGN CURRENCY], TS01_StockList_Draft.[PRICE FOB (THAI BAHT)]; ";
            }
            return s_cmd;
        }
        public static string QS03_StockList_FINAL()
        {
            string s_cmd;
            s_cmd = "SELECT TS02_StockList_FOB_Draft.[INBD DT], TS02_StockList_FOB_Draft.[Inbound ED], TS02_StockList_FOB_Draft.[DECLRATION NO], TS02_StockList_FOB_Draft.[LINE NO], TS02_StockList_FOB_Draft.[INVOICE NO#], TS02_StockList_FOB_Draft.[PJT CD], TS02_StockList_FOB_Draft.[SUP PO NO], TS02_StockList_FOB_Draft.[PRT NO], TS02_StockList_FOB_Draft.LCTN, TS02_StockList_FOB_Draft.QTY, TS02_StockList_FOB_Draft.[ACT QTY], TS02_StockList_FOB_Draft.UNIT, TS02_StockList_FOB_Draft.[UNIT PRICE], " +
                    "Round([TS02_StockList_FOB_Draft].[ACT QTY]*[TS02_StockList_FOB_Draft].[UNIT PRICE],2) AS [FOB(FOREIGN)], TS02_StockList_FOB_Draft.Currency, TS02_StockList_FOB_Draft.[EX RATE], Round([TS02_StockList_FOB_Draft].[ACT QTY]*[TS02_StockList_FOB_Draft].[UNIT PRICE]*[TS02_StockList_FOB_Draft].[EX RATE],2) AS [FOB(THB)], TS02_StockList_FOB_Draft.STATUS, TS02_StockList_FOB_Draft.[FOB PRICE (FOREIGN CURRENCY], TS02_StockList_FOB_Draft.[PRICE FOB (THAI BAHT)] " +
                    "FROM TS02_StockList_FOB_Draft; ";
            return s_cmd;
        }
        public static string TI04_InBound_Duplicate_CheckMaster()
        {
            string s_cmd;
            s_cmd = "SELECT * FROM TI04_InBound_Duplicate_CheckMaster";
            return s_cmd;
        }
        public static string TO04_OutBound_Duplicate_CheckMaster()
        {
            string s_cmd;
            s_cmd = "SELECT * FROM TO04_OutBound_Duplicate_CheckMaster";
            return s_cmd;
        }
        



        #endregion
    }
}