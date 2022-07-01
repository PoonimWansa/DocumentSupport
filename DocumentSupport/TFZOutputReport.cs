using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComFunction;
using System.IO;

namespace DocumentSupport
{
    public partial class TFZOutputRe : Form
    {
        public TFZOutputRe()
        {
            InitializeComponent();
            SceenSetting();
        }
        private void SceenSetting()
        {
            utDateFrom.Value = null;
            utDateTo.Value = null;
        }

        string PathTemplateOutReport = Application.StartupPath + @"\Template\QR01_Details_of_Export_MoveIn_Report_Template.xls";
        string PathOutputReport = Application.StartupPath + @"\Output\QR01_Details_of_Export_MoveIn_Report.xls";
        string PathTemplateOutReport2 = Application.StartupPath + @"\Template\QR02_Details of Export MoveOut_Report_Template.xls";
        string PathOutputReport2 = Application.StartupPath + @"\Output\QR02_Details of Export MoveOut_Report.xls";
        string PathTemplateInOut = Application.StartupPath + @"\Template\QR03_InOutReport_Template.xls";
        string PathInOut = Application.StartupPath + @"\Output\QR03_InOutReport.xls";

        private void UtDateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
        private void UtDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void Btn_OutputReport_Click(object sender, EventArgs e)
        {
            GenerateReportOutReport();
            GenerateReportOutReport2();
            GenerateReportInOut();
        }
        private void GenerateReportOutReport()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateOutReport, PathOutputReport, true);
            XlsCreator1.OpenBook(PathOutputReport, "");
            DateTime dateFrom = Convert.ToDateTime(utDateFrom.Value);
            DateTime dateTo = Convert.ToDateTime(utDateTo.Value);

            if (dateFrom > dateTo)
            {
                MessageBox.Show("Date format error.");
                return;
            }

            string sql = ComFunc.QR01_Details_of_Export_MoveIn_Report(dateFrom, dateTo);
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;

            if (null != dt)
            {
              
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                    }
                    RowCnt++;
                }
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathOutputReport);
        }
        private void GenerateReportOutReport2()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateOutReport2, PathOutputReport2, true);
            XlsCreator1.OpenBook(PathOutputReport2, "");
            DateTime dateFrom = Convert.ToDateTime(utDateFrom.Value);
            DateTime dateTo = Convert.ToDateTime(utDateTo.Value);

            if (dateFrom > dateTo)
            {
                MessageBox.Show("Date format error.");
                return;
            }

            string sql = ComFunc.QR02_Details_of_Export_MoveOut_Report(dateFrom, dateTo);
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;

            if (null != dt)
            {
                //check data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                    }
                    RowCnt++;
                }
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathOutputReport2);
        }
        private void GenerateReportInOut()
        {
            ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
            File.Copy(PathTemplateInOut, PathInOut, true);
            XlsCreator1.OpenBook(PathInOut, "");
            DateTime dateFrom = Convert.ToDateTime(utDateFrom.Value);
            DateTime dateTo = Convert.ToDateTime(utDateTo.Value);

            if (dateFrom > dateTo)
            {
                MessageBox.Show("Date format error.");
                return;
            }

            string sql = ComFunc.QR03_InOutReport(dateFrom,dateTo);
            DataTable dt = ComFunc.ConnectDatabase(sql);
            int RowCnt = 1;

            if (null != dt)
            {

                //check data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        XlsCreator1.Pos(j, RowCnt).Str = dt.Rows[i][j].ToString();
                    }
                    RowCnt++;
                }
            }

            XlsCreator1.CloseBook(true);
            System.Diagnostics.Process.Start(PathInOut);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
