using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
//引入OleDb数据库命名空间
using System.Data.OleDb;

namespace monitoring_system
{
    public class frmMain
    {
        //ACCESS数据库连接字符串
        public static string odbcConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\monitoring system.accdb";

        public bool getdata = false;

        OleDbConnection conn = new OleDbConnection(odbcConnStr);
        public void GetPurview()
        {
            OleDbConnection conn = new OleDbConnection(odbcConnStr);
            conn.Open();

            string sql = "select filedSystemManage,filedSpeciManage,filedClassManage,filedCourseManage,filedScoreManage,filedStudentMange from tbRoles where filedRoleName='" + "'";

            OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);            
        }
        //用户登录
        public Boolean frmLogin(string UserName, string Pwd)
        {

            string sql = "select * from tbAccountInfo where filedUserAccount='" + UserName.Trim() +
                    "' and filedUserPwd='" + Pwd.Trim() + "'";
            OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
            conn.Open();

            OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count != 0)
            {
                //System.Windows.Forms.MessageBox.Show("登录成功");
                conn.Close();
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("用户名或者密码错误！");
            }
            return false;

        }
        //用户注册
        public Boolean frmRegistration(string UserName, string Pwd)
        {
          
            if (UserName.Trim() == "" || Pwd.Trim() == "")
            {
                MessageBox.Show("请将信息填写完整!");
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
                conn.Open();

                string sql = "select * from tbAccountInfo where filedUserAccount='" + UserName.Trim() + "'";
                
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    MessageBox.Show("已经存在的用户名称！");
                }
                else
                {
                    sql = "insert into tbAccountInfo (filedUserAccount,filedUserPwd) values ('"
                        + UserName.Trim() + "','" + Pwd.Trim() + "')";
                    MessageBox.Show(sql);
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    //MessageBox.Show("添加用户成功！");
                    return true;
                }
                conn.Close();
            }
            return false;
        }
        public void frmBrowseList()
        {
            string sql = "";
            if (server.form1.cbPatient.Text == "")
            {
                MessageBox.Show("请将信息填写完整!");
            }
            else
            {
                if (server.form1.comboBox1.Text == "BPM")
                {
                    sql = "select filedHeartrate as BPM ,filedTime as 时间,filedDay as 日期 from tbHeartrateInfo where filedUserAccount='"
                        + server.form1.cbPatient.Text + "' and filedDay='" + server.form1.cbData.Text + "' order by filedDay DESC,filedTime DESC";
                }
                else if (server.form1.comboBox1.Text == "体温")
                {
                    sql = "select filedTemperature as 体温 ,filedTime as 时间,filedDay as 日期 from tbTemperatureInfo where filedDay='"
                        + server.form1.cbData.Text + "' and filedPosition='" + server.form1.cbPatient.Text + "' order by filedDay ,filedTime ";
                }
                else
                    return ;
                OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
                conn.Open();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable table = ds.Tables[0];                 //传给Table  filedHeartrate as BPM ,
                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)           //遍历Table的Row
                    {
                        //DataRow row = table.Rows[i];                     //传递给每个Row
                        //string name = Convert.ToString(row["时间"]); //取值
                        //MessageBox.Show(name.Substring(0,10));
                        server.form1.dataGridView1.DataSource = ds.Tables[0].DefaultView;
                    }
                }
                else
                {
                    MessageBox.Show("没有找到数据，请重新输入");
                    server.form1.dataGridView1.DataSource = null;
                }
                conn.Close();
            }
        }
        public DataTable frmBrowseGraph()
        {

            OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
            conn.Open();
            string sql = "";
            if (server.form1.comboBox1.Text == "BPM")
            {
                sql = "select filedHeartrate as BPM ,filedTime as 时间,filedDay as 日期 from tbHeartrateInfo where filedUserAccount='"
                    + server.form1.cbPatient.Text + "' and filedDay='" + server.form1.cbData.Text + "' order by filedDay DESC,filedTime DESC";
            }
            else if (server.form1.comboBox1.Text == "体温")
            {
                sql = "select filedTemperature as 体温 ,filedTime as 时间,filedDay as 日期 from tbTemperatureInfo where filedDay='"
                    + server.form1.cbData.Text + "' and filedPosition='" + server.form1.cbPatient.Text + "' order by filedDay ,filedTime ";
            }
            else
                return null;
            //MessageBox.Show(sql);
            OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            DataTable table = ds.Tables[0];                 //传给Table  filedHeartrate as BPM ,

            if (ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)           //遍历Table的Row
                {
                    //DataRow row = table.Rows[i];                     //传递给每个Row
                    //string name = Convert.ToString(row["时间"]); //取值
                    //MessageBox.Show(name.Substring(0,10));
                    server.form1.dataGridView1.DataSource = ds.Tables[0].DefaultView;
                }
            }
            else
            {
                MessageBox.Show("没有找到数据，请重新输入");
                server.form1.dataGridView1.DataSource = null;
            }
            conn.Close();

            return table;
            
        }
        //添加体温信息
        public void frmTemperature(string Position, string Temperature)
        {
          
            OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
            conn.Open();

            string sql = "insert into tbTemperatureInfo (filedPosition,filedTemperature,filedTime,filedDay) values ('"
                + Position.Trim() + "','" + Temperature.Trim() + "','" + DateTime.Now.ToLongTimeString().ToString()
                + "','" + DateTime.Now.ToShortDateString().ToString() + "')";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        //载入病人列表
        public void frmBrowsePatient()
        {
            OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
            conn.Open();
            DataSet ds = new DataSet();
            if(server.form1.comboBox1.Text == "BPM")
            {
                string sql = "select filedUserAccount from tbHeartrateInfo group by filedUserAccount";

                OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
                adp.Fill(ds);
                DataTable table = ds.Tables[0];                 //传给Table

                //server.form1.cbPatient.DisplayMember = "filedUserAccount";
                //server.form1.cbPatient.ValueMember = "filedUserAccount";
            }
            else if (server.form1.comboBox1.Text == "体温")
            {
                string sql = "select filedPosition from tbTemperatureInfo group by filedPosition";

                OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
                adp.Fill(ds);
                DataTable table = ds.Tables[0];                 //传给Table

                //server.form1.cbPatient.DisplayMember = "filedPosition";
                //server.form1.cbPatient.ValueMember = "filedPosition";
            }
            server.form1.cbPatient.DataSource = ds.Tables[0].DefaultView;

            conn.Close();
        }
        public void frmBrowseData()
        {
            OleDbConnection conn = new OleDbConnection(frmMain.odbcConnStr);
            conn.Open();
            string sql = "";
            DataSet ds = new DataSet();
            if (server.form1.comboBox1.Text == "BPM")
            {
                sql = "select distinct filedDay from (select distinct filedDay,filedUserAccount from tbHeartrateInfo where filedUserAccount='"
                + server.form1.cbPatient.Text + "') group by filedDay ";

            }
            else if (server.form1.comboBox1.Text == "体温")
            {
                sql = "select distinct filedDay from (select distinct filedDay,filedPosition from tbTemperatureInfo where filedPosition='"
                + server.form1.cbPatient.Text + "') group by filedDay ";

            }
            else
                return;
            OleDbDataAdapter adp = new OleDbDataAdapter(sql, conn);
            adp.Fill(ds);
            DataTable table = ds.Tables[0];                 //传给Table

            server.form1.cbData.DisplayMember = "filedDay";
            server.form1.cbData.ValueMember = "filedDay";

            server.form1.cbData.DataSource = ds.Tables[0].DefaultView;

            conn.Close();
        }

    }   
}
