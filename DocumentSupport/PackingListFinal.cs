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
    public partial class PackingListFinal : Form
    {

        public PackingListFinal()
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
                s_cmd = ComFunc.Q06_PackingList_FINAL();
                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();

                }

                PackingListSummary_TFZ();
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3101";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }
        private void PackingListSummary_TFZ()
        {
            try
            {
                string s_cmd = "";
                string s_cmd1 = "";
                //Picking List Draft Sum
                if (ComFunc.TableDeleted("T10_PackingList_INBOUND_ED_NO"))
                {

                    s_cmd = ComFunc.Q06_PackingList_INBOUND_ED_NO();

                    if (null == ComFunc.ConnectDatabase(s_cmd))
                    {
                        string error_msg = @"System Error E1205";
                        ComFunc.WriteLogLocal(error_msg, "");
                    }
                    else
                    {

                        if (ComFunc.TableDeleted("T09_InBound_LINE_NO"))
                        {
                            s_cmd = ComFunc.Q07_TIFFA_LINE_NO();
                            if (null == ComFunc.ConnectDatabase(s_cmd))
                            {
                                string error_msg = @"System Error E1205";
                                ComFunc.WriteLogLocal(error_msg, "");
                            }
                            else
                            {
                                s_cmd1 = ComFunc.Q06_PackingList_Summary_TFZ();
                            }

                        }

                        DataTable dt = ComFunc.ConnectDatabase(s_cmd1);
                        dt = ComFunc.ConnectDatabase(s_cmd1);
                        if (null != dt)
                        {
                            dataGridView2.DataSource = dt;
                            dataGridView2.ClearSelection();
                            lbl_Cnt.Text = dt.Rows.Count.ToString();

                            //Q08_PackingList_INBD_ED_LINE_NO
                            PackingList_ED_LINE_NO();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3101";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }

        }
        private void PackingList_ED_LINE_NO()
        {
            try {
                string s_cmd = "";

                s_cmd = ComFunc.Q08_PackingList_INBD_ED_LINE_NO();

                DataTable dt = ComFunc.ConnectDatabase(s_cmd);
                dt = ComFunc.ConnectDatabase(s_cmd);
                if (null != dt)
                {
                    dataGridView3.DataSource = dt;
                    dataGridView3.ClearSelection();
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


        private void btn_excel_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "M02_PackingList_FINAL", true);
        }

        private void btn_excel2_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView2, "Q06_PackingList_Summary_TFZ", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView3, "Q08_PackingList_INBD_ED_LINE_NO", true);
        }




    }

}
