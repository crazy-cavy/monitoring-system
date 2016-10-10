using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//引入绘图命名空间
using ZedGraph;
//引入控制系统命名空间
using monitoring_system;
//引入自适应窗口命名空间
using AutoSizeForm;

namespace GraphFormFormsApplication
{
    public partial class GraphForm : Form
    {
        frmMain frm = new frmMain();

        //自适应窗口
        AutoSizeFormClass asc = new AutoSizeFormClass();

        public static GraphPane myPane = null;
        public static LineItem myLine = null;
        PointPairList list = new PointPairList();
        public static int oldRowsCount = 0;
        public GraphForm()
        {
            InitializeComponent();
            createPane(zedGraphControl1);
            
        }
        public void Pane()
        {
            zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

            DataTable table = frm.frmBrowseGraph();
            for (int i = 0; i < table.Rows.Count - oldRowsCount; i++)
            {
                int x = i;
                DataRow row = table.Rows[i];                     //传递给每个Row
                Double value = Convert.ToDouble(row["体温"]); ; //取值
                list.Add(x, value); //添加一组数据
            }

            //以上生成的图标X轴为数字，下面将转换为日期的文本
            string[] labels = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];                     //传递给每个Row
                labels[i] = Convert.ToString(row["时间"]);
            }
            myPane.XAxis.Scale.TextLabels = labels; //X轴文本取值
            myPane.XAxis.Type = AxisType.Text;   //X轴类型
            //画到zedGraphControl1控件中，此句必加
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
            this.Invalidate();
        }
        public void createPane(ZedGraphControl zgc)
        {
            myPane = zgc.GraphPane;
            //设置图标标题和x、y轴标题
            if (server.form1.comboBox1.Text == "BPM")
            {
                myPane.YAxis.Title.Text = "脉搏/BPM";
                myPane.Title.Text = "BPM变化曲线";
            }
            else if (server.form1.comboBox1.Text == "体温")
            {
                myPane.YAxis.Title.Text = "体温/℃";
                myPane.Title.Text = "体温变化曲线";
            }
            else
                return;

            myPane.XAxis.Title.Text = "时间/t";


            //更改标题的字体
            FontSpec myFont = new FontSpec("Arial", 20, Color.Red, false, false, false);
            myPane.Title.FontSpec = myFont;
            myPane.XAxis.Title.FontSpec = myFont;
            myPane.YAxis.Title.FontSpec = myFont;

            // 造一些数据，PointPairList里有数据对x，y的数组
            // Y轴数据
            DataTable table = frm.frmBrowseGraph();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                int x = i;
                DataRow row = table.Rows[i];                     //传递给每个Row
                Double value = Convert.ToDouble(row[server.form1.comboBox1.Text]); ; //取值
                list.Add(x, value); //添加一组数据
            }

            // 用list生产一条曲线，标注是 filedPosition/filedUserAccount
            myLine = myPane.AddCurve(server.form1.cbPatient.Text, list, Color.Red, SymbolType.Star);

            //填充图表颜色
            myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);

            //以上生成的图标X轴为数字，下面将转换为日期的文本
            string[] labels = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];                     //传递给每个Row
                labels[i] = Convert.ToString(row["时间"]);
            }
            oldRowsCount = table.Rows.Count;
            myPane.XAxis.Scale.TextLabels = labels; //X轴文本取值
            myPane.XAxis.Type = AxisType.Text;   //X轴类型

            //画到zedGraphControl1控件中，此句必加
            zgc.AxisChange();
            zgc.Refresh();

            timer1.Enabled = true;
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }
        private void GraphForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                asc.controlAutoSize(this);
            }
            catch
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Pane();
        }
    }
}
