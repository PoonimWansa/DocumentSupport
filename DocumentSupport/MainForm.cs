using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using ComFunction;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace DocumentSupport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            try
            {
                ComFunc.s_CurrentPath = Directory.GetCurrentDirectory();

                if ("" != ConfigurationManager.AppSettings.Get("VersionUpdate"))
                {
                    try
                    {
                        string s_ver = "SELECT TOP 1 * FROM [TB_VERSION] WHERE [SystemName] = '" + ConfigurationManager.AppSettings.Get("VersionUpdate") + "' ORDER BY [Version] DESC";
                        DataTable dtVer = ConnectDatabaseVersion(s_ver);
                        if (null != dtVer && 0 < dtVer.Rows.Count)
                        {
                            double dLatest = ComFunc.ConvertDouble(dtVer.Rows[0]["Version"].ToString());
                            double dCurrent = ComFunc.ConvertDouble(lblVersion.Text);
                            if (dLatest > dCurrent)
                            {
                                string VersionManagement = ComFunc.s_CurrentPath + "\\VersionManagement.exe";
                                System.Diagnostics.Process.Start(VersionManagement);
                                Environment.Exit(0);
                            }
                        }
                        else
                        {
                            MessageBox.Show("DB Connection Error");
                            Environment.Exit(0);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Environment.Exit(0);
                    }
                }

                if (false == Directory.Exists(ComFunc.s_CurrentPath + "/Log"))
                {
                    Directory.CreateDirectory(ComFunc.s_CurrentPath + "/Log");
                }
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E1301" + ex.Message;
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }

        }

        public DataTable ConnectDatabaseVersion(string sCommand)
        {
            const string strConnection = "VersionConnectionString";

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
                ComFunc.WriteLogLocal("ERROR",e.ToString());
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

        private void btn_PackingList_Click(object sender, EventArgs e)
        {
            PackingList f = new PackingList();
            f.Show();
        }

        private void btn_DebitNote_Click(object sender, EventArgs e)
        {
            DebitNote f = new DebitNote();
            f.Show();
        }

        private void btn_StockList_Click(object sender, EventArgs e)
        {
            StockList f = new StockList();
            f.Show();
        }

        private void btn_TFZReport_Click(object sender, EventArgs e)
        {
            TFZReport f = new TFZReport();
            f.Show();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\Template\SystemManual.xls");
        }

        private void btn_user_Click(object sender, EventArgs e)
        {
            Master_User form = new Master_User();
            form.Show();
        }
    }
}
