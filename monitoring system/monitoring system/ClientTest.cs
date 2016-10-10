using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//引入Sockets和线程的命名空间
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace monitoring_system
{
    public partial class server : Form
    {
        //与服务器的连接
        private TcpClient tcpClient;

        //与服务器交流的流通道
        private NetworkStream nsStream;

        //客户端的2种状态
        private static string CLOSED = "closed";
        private static string CONNECTED = "connected";
        private string clientState = CLOSED;

        private bool StopFlag;

        public server()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //连接聊天室服务器
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (clientState == CONNECTED)
            {
                return;
            }

            if (this.tbUserName.Text.Length == 0)
            {
                MessageBox.Show("请输入您的用户名!", "操作提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbUserName.Focus();
                return;
            }
            try
            {
                //创建一个客户端套接字，它是Login的一个公共属性，
                //将被传递给frmClientMain窗体
                tcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                tcpClient.Connect(IPAddress.Parse(tbServer.Text),
                    Int32.Parse(tbServerPort.Text));
                //获得与服务器数据交互的流通道（NetworkStream)
                nsStream = tcpClient.GetStream();

                //启动一个新的线程，执行方法this.ServerResponse()，
                //以便来响应从服务器发回的信息
                Thread newthread = new Thread(new ThreadStart(this.ServerResponse));
                newthread.Start();

                //向服务器发送“CONNECT”请求命令，
                //此命令的格式与服务器端的定义的格式一致，
                //命令格式为：命令标志符（CONNECT）|发送者的用户名|
                string cmd = "(login \"" + this.tbUserName.Text + "\" \"code\" \"azB4a0FPM05HVkxIdUdvK1BtR0NmMmpUUUE1SFpSOVpiK09lTXVhYjU4eUd1UjBIQXNNK3hUU3FlVjFscXJMS2Zn\" \"USER\")\n";

                //将字符串转化为字符数组
                Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                    cmd.ToCharArray());
                nsStream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //用于接收从服务器发回的信息，
        //根据不同的命令，执行相应的操作
        private void ServerResponse()
        {
            ListBox.CheckForIllegalCrossThreadCalls = false;
            //定义一个byte数组，用于接收从服务器端发送来的数据，
            //每次所能接收的数据包的最大长度为1024个字节
            byte[] buff = new byte[1024];
            string msg;
            int len;
            try
            {
                if (!nsStream.CanRead)
                {
                    return;
                }

                StopFlag = false;
                while (!StopFlag)
                {
                    //从流中得到数据，并存入到buff字符数组中
                    len = nsStream.Read(buff, 0, buff.Length);

                    if (len < 1)
                    {
                        Thread.Sleep(200);
                        continue;
                    }

                    //将字符数组转化为字符串
                    msg = System.Text.Encoding.Default.GetString(buff, 0, len);
                    msg.Trim();

                    string[] acceptFromServer = msg.Split(new Char[] { '|' });
                    //acceptFromServer[0]中保存了命令标志符（LIST或JOIN或QUIT）

                    if (acceptFromServer[0].ToUpper() == "#t")
                    {
                        //处理响应
                        Invoke(new addHandler(add), rbChatContent, "数据传输成功");
                    }
                    else if (acceptFromServer[0].ToUpper() == "#f")
                    {
                        //数据传输失败
                        Invoke(new addHandler(add), rbChatContent, "数据传输失败：" + acceptFromServer[1]);
                    }
                    else if (acceptFromServer[0] == "LIST")
                    {
                        //此时从服务器返回的消息格式：
                        //命令标志符（LIST）|用户名1|用户名|2...（所有在线用户名）|
                        Invoke(new addHandler(add), rbChatContent, "获得用户列表");

                        //add("获得用户列表");
                        //更新在线用户列表
                        lbUserList.Items.Clear();
                        for (int i = 1; i < acceptFromServer.Length - 1; i++)
                        {
                            lbUserList.Items.Add(acceptFromServer[i].Trim());
                        }
                    }
                    else if (acceptFromServer[0] == "JOIN")
                    {
                        //此时从服务器返回的消息格式：
                        //命令标志符（JOIN）|刚刚登入的用户名|
                        Invoke(new addHandler(add), rbChatContent, acceptFromServer[1] + " " + "已经进入了聊天室");
                        this.lbUserList.Items.Add(acceptFromServer[1]);
                        if (this.tbUserName.Text == acceptFromServer[1])
                        {
                            this.clientState = CONNECTED;
                        }
                    }
                    else if (acceptFromServer[0] == "QUIT")
                    {
                        if (this.lbUserList.Items.IndexOf(acceptFromServer[1]) > -1)
                        {
                            this.lbUserList.Items.Remove(acceptFromServer[1]);
                        }
                        Invoke(new addHandler(add), rbChatContent, "用户：" + acceptFromServer[1] + " 离开聊天室");
                    }
                    else
                    {
                        //如果从服务器返回的其他消息格式，
                        //则在ListBox控件中直接显示
                        Invoke(new addHandler(add), rbChatContent, msg);
                    }
                }
                //关闭连接
                tcpClient.Close();
            }
            catch
            {
            }
        }

        delegate void addHandler(RichTextBox rb, string msg);

        /// <summary>
        /// 添加信息到聊天窗体
        /// </summary>
        /// <param name="msg">信息</param>

        private void add(RichTextBox rb, string msg)
        {

            rb.SelectedText = msg + "\n";
        }
        delegate void AddUserHandler(ListBox lb, string user);

        private void btSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbUserList.SelectedIndex == -1)
                {
                    MessageBox.Show("请在列表中选择一个用户", "提示信息",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string receiver = lbUserList.SelectedItem.ToString();
                //消息的格式是：
                //  "(@devcall \"接收者的用户名\" (uartdata \"发送内容\") (lambda x x))\n"
                string message = "(@devcall \"" + receiver + "\" (uartdata \"" +
                    rbSendMsg.Text + "\") (lambda x x))\n";
                rbSendMsg.Text = "";
                rbSendMsg.Focus();
                //将字符串转化为字符数组
                Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                    message.ToCharArray());
                nsStream.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                this.rbChatContent.AppendText("网络发生错误或者服务器已经退出！");
            }
        }
    }
}
