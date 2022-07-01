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
    public partial class Master_User : Form
    {
        private string s_selected = "";
        private string s_selectedBak = "";

        public Master_User()
        {
            InitializeComponent();
            setScreen();
        }

        private void setScreen()
        {
            try
            {
                string str = "SELECT USER_ID " +
                                  ",USER_NAME " +
                                  ",USER_PASSWORD " +
                                  ",USER_POSITION " +
                              "FROM TB_M_USER";
                if (true == chb_id.Checked
                    || true == chb_name.Checked)
                {
                    str = str + " WHERE ";
                    bool b_first = true;
                    if (true == chb_id.Checked)
                    {
                        str = str + " USER_ID LIKE '%" + txtSearch.Text + "%'";
                        b_first = false;
                    }
                    if (true == chb_name.Checked)
                    {
                        if (false == b_first)
                        {
                            str = str + " OR ";
                        }
                        str = str + " USER_NAME LIKE '%" + txtSearch.Text + "%'";
                        b_first = false;
                    }
                }
                DataTable dt = ComFunc.ConnectDatabase(str);
                if (null != dt)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    lbl_Cnt.Text = dt.Rows.Count.ToString();
                    if (0 != dt.Rows.Count)
                    {
                        dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
                    }
                }
                selectData();
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            selectData();
        }

        private void selectData()
        {

            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                s_selected = r.Cells[0].Value.ToString();
                txtID.Text = r.Cells[0].Value.ToString();
                txtName.Text = r.Cells[1].Value.ToString().Trim();
                txtPosition.Text = r.Cells[3].Value.ToString().Trim();
            }
        }

        private void selectAgain()
        {
            if ("" != s_selectedBak)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (s_selectedBak.Trim() == dataGridView1[0, row.Index].Value.ToString().Trim())
                    {
                        dataGridView1[0, row.Index].Selected = true;
                    }
                }
                s_selectedBak = "";
            }
        }

        private void EnableChanged(bool b_data)
        {
            if (true == b_data)
            {
                gpDetail.Enabled = false;
                gpDetail.BackColor = Color.LightGray;
                gpData.Enabled = true;
            }
            else
            {
                gpDetail.Enabled = true;
                gpDetail.BackColor = Color.LightPink;
                gpData.Enabled = false;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            EnableChanged(false);
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            EnableChanged(false);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" != s_selected)
                {
                    string s_SQL = "DELETE FROM TB_M_USER" +
                                    " WHERE USER_ID = '" + s_selected + "'";
                    if (null == ComFunc.ConnectDatabase(s_SQL))
                    {
                        string error_msg = @"System Error E3102";
                        MessageBox.Show(error_msg);
                    }
                    setScreen();
                }
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3103";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            s_selected = "";
            txtID.Text = "";
            txtName.Text = "";
            txtPosition.Text = "";
            gpDetail.Enabled = true;
            dataGridView1.ClearSelection();
            EnableChanged(false);
            txtID.Focus();
        }

        private bool checkInput()
        {
            string error_msg = "";

            // check duplicate number.
            string str = "SELECT * FROM TB_M_USER WHERE USER_ID = '" + txtID.Text + "'";
            DataTable dt = ComFunc.ConnectDatabase(str);
            if ("" == s_selected)
            {
                // new mode.
                if (null == dt || 0 != dt.Rows.Count)
                {
                    error_msg = @"Used Value.";
                    MessageBox.Show(error_msg);
                    txtID.Focus();
                    return false;
                }
            }
            else
            {
                // edit mode.
                if (s_selected != txtID.Text)
                {
                    if (null == dt || 0 != dt.Rows.Count)
                    {
                        error_msg = @"Used Value.";
                        MessageBox.Show(error_msg);
                        txtID.Focus();
                        return false;
                    }
                }
            }

            // check blank.
            if ("" == txtID.Text)
            {
                error_msg = @"Invalid Value.";
                MessageBox.Show(error_msg);
                txtID.Focus();
                return false;
            }
            else if ("" == txtName.Text)
            {
                error_msg = @"Invalid Value.";
                MessageBox.Show(error_msg);
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (true == checkInput())
                {
                    if ("" == s_selected)
                    {
                        //New Mode
                        string s_cmd = "INSERT INTO TB_M_USER VALUES ( " +
                                              "'" + txtID.Text + "' " +
                                              ",N'" + txtName.Text + "' " +
                                              ",'' " +
                                              ",N'" + txtPosition.Text + "' " +
                                              ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                              ",'" + "9999" + "' " +
                                              ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                              ",'" + "9999" + "' " +
                                         ") ";
                        if (null == ComFunc.ConnectDatabase(s_cmd))
                        {
                            string error_msg = @"System Error E3104";
                            MessageBox.Show(error_msg);
                        }
                    }
                    else
                    {
                        // edit mode.
                        string s_cmd = "UPDATE TB_M_USER SET " +
                                              "USER_NAME = N'" + txtName.Text + "' " +
                                              ",USER_POSITION = N'" + txtPosition.Text + "' " +
                                              ",UPDATE_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                              ",UPDATE_BY = '" + "9999" + "' " +
                                        "WHERE USER_ID = '" + s_selected + "' ";
                        if (null == ComFunc.ConnectDatabase(s_cmd))
                        {
                            string error_msg = @"System Error E3105";
                            MessageBox.Show(error_msg);
                        }
                    }
                    s_selectedBak = txtID.Text;
                    setScreen();
                    EnableChanged(true);
                    selectAgain();
                }
            }
            catch (Exception ex)
            {
                string error_msg = @"System Error E3106";
                MessageBox.Show(error_msg);
                ComFunc.WriteLogLocal(error_msg, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            EnableChanged(true);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            EnableChanged(true);
            dataGridView1.ClearSelection();
            setScreen();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            chb_id.Checked = true;
            chb_name.Checked = true;

            EnableChanged(true);
            dataGridView1.ClearSelection();
            setScreen();
        }

        #region generate excel file.
        // private ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();
        public static ExcelCreator.XlsCreator XlsCreator1 = new ExcelCreator.XlsCreator();

        protected bool b_existSelect = true;
        protected const int EXCEL_STR = 0;
        protected const int EXCEL_NUM = 1;
        protected const int EXCEL_MON = 2;
        protected const int EXCEL_DATE = 3;
        struct data
        {
            public data(string _value1, int _value2)
            {
                this.value1 = _value1;
                this.value2 = _value2;
            }
            public string value1;
            public int value2;
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            ComFunc.GenerateDatagridview(dataGridView1, "UserMaster", true);
        }

        #endregion

        private void btn_import_Click(object sender, EventArgs e)
        {
            ComFunc.ImportDatagridviewEXCEL(dataGridView1, "TB_M_USER");
            setScreen();
            MessageBox.Show("Import finished!","Document Support");
        }
    }
}
