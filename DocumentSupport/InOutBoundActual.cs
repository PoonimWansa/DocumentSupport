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
    public partial class InOutBoundActual : Form
    {

        public InOutBoundActual()
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



        private void setScreen()
        {
            try
            {
                string s_cmd;
                s_cmd = ComFunc.QR01_InBoundActual_DB("","");//from to
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }

                s_cmd = ComFunc.QR01_OutBoundActual_DB("", "");//from to
                if (null == ComFunc.ConnectDatabase(s_cmd))
                {
                    string error_msg = @"System Error E1205";
                    ComFunc.WriteLogLocal(error_msg, "");
                }
                else
                {
                    s_cmd = ComFunc.QR02_INOUT_RECORD_TFZ("","");//from to
                    DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                    if (null != dt)
                    {
                        dataGridView1.DataSource = dt;
                        dataGridView1.ClearSelection();
                        lbl_Cnt.Text = dt.Rows.Count.ToString();

                    }
                }
                OutBoundActual();
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3101";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }
        private void OutBoundActual()
        {
            try
            {
                string s_cmd = "";
                s_cmd = ComFunc.QR03_OutBoundActual_M3_TFZ("", "");//from To
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);  
                dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView2.DataSource = dt;
                    dataGridView2.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();
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

        private void btn_PackingFinal_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "QR02_INOUT RECORD_TFZ", true);
        }
        private void btn_PackingTFZ_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView2, "QR03_OutBoundActual_M3_TFZ", true);

            //Delete record
            //string s_cmd = "";
            //s_cmd = ComFunc.QI03_InBoundActual_DB_Delete();//Until
            //if (null == ComFunc.ConnectDatabase(s_cmd))
            //{
            //    string error_msg = @"System Error E1205";
            //    ComFunc.WriteLogLocal(error_msg, "");
            //}

            //s_cmd = ComFunc.QO09_OutBoundActual_DB_Delete();//Until
            //if (null == ComFunc.ConnectDatabase(s_cmd))
            //{
            //    string error_msg = @"System Error E1205";
            //    ComFunc.WriteLogLocal(error_msg, "");
            //}
        }

    }
}
