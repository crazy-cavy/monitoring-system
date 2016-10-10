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
//引入自适应窗口命名空间
using AutoSizeForm;
//引入语音命名空间
using DotNetSpeech;
//引入OleDb数据库命名空间
using System.Data.OleDb;
using GraphFormFormsApplication;

namespace monitoring_system

{
    public partial class server : Form
    {
        ////软件运行时间
        //System.DateTime currentTime = new System.DateTime();

        public static server form1 = null;

        frmMain frm = new frmMain();
         
        //自适应窗口
        AutoSizeFormClass asc = new AutoSizeFormClass();

        ////与服务器连接的TCP客户端
        //private TcpClient tcpClient;
        //与内网连接的TCP服务器
        private TcpListener tcpListener;
        List<TcpClient> list= new List<TcpClient>();
        
        //与服务器交流的流通道
        private NetworkStream nsStream;

        //客户端的2种状态
        private static string CLOSED = "closed";
        private static string CONNECTED = "connected";
        public string clientState = CLOSED;

        private bool StopFlag;

        //服务器是否开启的标志
        internal static bool ServerFlag = false;

        public server()
        {
            InitializeComponent();
            server_Init();
            Speak("程序已准备完毕",1);
        }

        //主窗口加载

        private void server_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            ConnectAndLogin();
            form1 = this;
            

        }


        private void server_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                asc.controlAutoSize(this);
            }
            catch
            {
            }
        }

        //连接服务器
        private void Connect()
        {
            //向服务器发送“CONNECT”请求命令，
            //此命令的格式与服务器端的定义的格式一致，
            //命令格式为：
            string cmd = "(login \"" + "monitor1" +
            "\" \"code\" \"azB4a0FPM05HVkxIdUdvK1BtR0NmMmpUUUE1SFpSOVpiK09lTXVhYjU4eUd1UjBIQXNNK3hUU3FlVjFscXJMS2Zn\" \"USER\")\n";

            //将字符串转化为字符数组
            Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                cmd.ToCharArray());
            nsStream.Write(bytes, 0, bytes.Length);
            //开启定时ping
            timer1.Enabled = true;
        }

        private void ReConnect()
        {
            //向服务器发送“CONNECT”请求命令，
            //此命令的格式与服务器端的定义的格式一致，
            //命令格式为：
            list[0].Close();
            nsStream.Dispose();
            nsStream.Flush();
            list.Remove(new TcpClient());
            list.Add(new TcpClient());
            //list[0].Connect(IPAddress.Parse("123.59.77.157"),
            //       Int32.Parse("9999"));
            //tcpClient.Connect(IPAddress.Parse("192.168.191.1"),
            //Int32.Parse("9999"));
            //获得与服务器数据交互的流通道（NetworkStream)
            nsStream = list[0].GetStream();
            //启动一个新的线程，执行方法this.ServerResponse()，
            //以便来响应从服务器发回的信息
            Thread newthread = new Thread(new ThreadStart(this.ServerResponse));
            newthread.Start();
            string cmd = "(login \"" + "monitor" +
            "\" \"code\" \"azB4a0FPM05HVkxIdUdvK1BtR0NmMmpUUUE1SFpSOVpiK09lTXVhYjU4eUd1UjBIQXNNK3hUU3FlVjFscXJMS2Zn\" \"USER\")\n";

            //将字符串转化为字符数组
            Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                cmd.ToCharArray());
            nsStream.Write(bytes, 0, bytes.Length);
            //开启定时ping
            timer1.Enabled = true;
        }
        private void ConnectAndLogin()
        {
            try
            {
                //创建一个客户端套接字，它是Login的一个公共属性，
                //将被传递给主窗体
                
                //tcpClient = new TcpClient();
                list.Add(new TcpClient());
                list[0].Connect(IPAddress.Parse("123.59.77.157"),
                   Int32.Parse("9999"));
                ////向指定的IP地址的服务器发出连接请求
                //tcpClient.Connect(IPAddress.Parse("123.59.77.157"),
                //   Int32.Parse("9999"));
                //tcpClient.Connect(IPAddress.Parse("192.168.191.1"),
                // Int32.Parse("9999"));
                //获得与服务器数据交互的流通道（NetworkStream)
                nsStream = list[0].GetStream();
                
                //启动一个新的线程，执行方法this.ServerResponse()，
                //以便来响应从服务器发回的信息
                Thread newthread = new Thread(new ThreadStart(this.ServerResponse));
                newthread.Start();

                //向服务器发送请求命令
                if (clientState == CLOSED)
                { 
                    Connect(); 
                }
                //创建一个服务器套接字，它是Login的一个公共属性，
                //将被传递给主窗体
                IPAddress userIP = IPAddress.Parse("192.168.191.1");
                tcpListener = new TcpListener(userIP,6000);
                tcpListener.Start();
                ServerFlag = true ;

                //启动一个新的线程，执行方法this.ServerResponse()，
                //以便来响应从服务器发回的信息
                Thread Listenerthread = new Thread(StartListen);
                Listenerthread.Start();
                clientState = CONNECTED;
                ping();
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
                    if (msg.Substring(1,4) == "pong")
                    {
                        clientState = CONNECTED;
                        radioButton1.Text = "在线";
                        radioButton1.Checked = true ;
                        timer2.Enabled = false ;
                    } 
                    //Invoke(new addHandler(add), rbChatContent, DateTime.Now.ToLongTimeString().ToString());
                    string[] smsg = msg.Split(new char[2] { ')','(' });
                    //foreach (string e in smsg)
                    //{
                    //    System.Windows.Forms.MessageBox.Show(e);
                    //}
                    if (smsg[1] == "logining")
                    {

                        if (frm.frmLogin(smsg[5], smsg[7]))
                        {
                            //Senddata(smsg[3], "true");
                        }
                        else
                        {
                            //SendToUser(smsg[3], "false");
                        }
                        //System.Windows.Forms.MessageBox.Show(smsg[3]);
                        //System.Windows.Forms.MessageBox.Show(smsg[5]);
                    }
                    else if (smsg[1] == "BPM")
                    {
                        //frm.GetBPM(smsg[1], smsg[2]);
                    }
                    else if (smsg[1] == "registering")
                    {
                        if (frm.frmRegistration(smsg[5], smsg[7] ) )
                        {
                            //SendToUser(smsg[3], "true");
                        }
                        else
                        {
                            SendToUser(smsg[3], "false");
                        }
                    }
                    if (msg == "#t")
                    {
                        //处理响应
                        //Invoke(new addHandler(add), rbChatContent, "数据传输成功");
                    }
                    else if (msg == "#f")
                    {
                        //数据传输失败
                        //Invoke(new addHandler(add), rbChatContent, "数据传输失败：" + msg);
                    }

                    else
                    {
                        //如果从服务器返回的其他消息格式，
                        //则在ListBox控件中直接显示
                        //Invoke(new addHandler(add), rbChatContent, msg);
                    }
                }
                //关闭连接
                list[0].Close();
            }
            catch
            {
            }
        }
        //用于接收客户端的请求,确认与客户端的连接
        //并且启动一个新的线程处理客户端的请求
        private void StartListen()
        {
            while (server.ServerFlag)
            {
                try
                {
                    //当接收到客户端请求时，确认与客户端的连接
                    if (tcpListener.Pending())
                    {
                            Socket newSocket = tcpListener.AcceptSocket();
                            //启动一个新的线程,处理用户相应的请求
                            ChatClient newClient = new ChatClient(this, newSocket);
                            Thread ClientThread = new Thread(newClient.ClientService);
                            ClientThread.Start();
                    }
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
        //用于接收从客户端发回的信息，
        //根据不同的命令，执行相应的操作
        public class ChatClient
        {

            private Socket currentSocket = null;

            private server server;

            public Socket CurrentSocket
            {
                get { return currentSocket; }
                set { currentSocket = value; }
            }
            public ChatClient(server server, Socket currentSocket)
            {
                this.server = server;
                this.currentSocket = currentSocket;

            }
            //和客户端进行数据通信,包括接收客户端的请求
            //根据不同的请求命令,执行相应的操作,并将操作结果返回给客户端
            public void ClientService()
            {
                byte[] buff = new byte[1024];//缓冲区
                bool keepConnected = true;
                //用循环不断地与客户端进行交互,直到其发出EXIT或者QUIT命令
                //将keepConnected设置为false,退出循环，关闭当前连接,中止当前线程
                while (keepConnected && server.ServerFlag)
                {
                    try
                    {
                        if (currentSocket == null || currentSocket.Available < 1)
                        {
                            Thread.Sleep(300);
                            continue;
                        }


                        //接收信息存入buff数组中
                        int length = currentSocket.Receive(buff);

                        //将字符数组转换为字符串
                        string Command = Encoding.Default.GetString(buff, 0, length);
                        server.rbChatContent.Text = server.rbChatContent.Text + Command;
                        if (Command == null)
                        {
                            Thread.Sleep(200);
                            continue;
                        }
                        else if (Command.Substring(0, 2) == "53" && Command.Substring(6, 2) == "54" && Command.Substring(12, 2) == "48")
                        {
                            string position = Command.Substring(2, 4);
                            Int16 temperature_h = Convert.ToInt16(Command.Substring(8, 2),16);
                            Int16 temperature_l = Convert.ToInt16(Command.Substring(10, 2), 16);
                            float temperature = temperature_h + (float)temperature_l / 100;

                            string speaking = "";

                            position = position.Substring(0, 2) + "号房间"
                                    + position.Substring(2, 2) + "号";
                            if (Command.Substring(14, 2) == "01")
                            {
                                speaking = position + "病人需要帮助";
                                server.Speak(speaking,2);
                                server.rbChatContent.AppendText(speaking);
                            }
                            if (temperature > 37.5)
                            {
                                speaking = position + "病人当前体温为" + Convert.ToString(temperature) + "摄氏度";
                                server.Speak(speaking, 1);
                                server.rbChatContent.AppendText(speaking);
                            }
                            server.frm.frmTemperature(position, Convert.ToString(temperature));
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("程序发生异常,异常信息:" + ex.Message);
                    }

                    Thread.Sleep(200);
                }
            }

            //回发消息给客户端
            private void SendToClient(ChatClient client, string msg)
            {
                Byte[] message = System.Text.Encoding.Default.GetBytes(msg.ToCharArray());
                client.CurrentSocket.Send(message, message.Length, 0);
            }
        }
        private string GetStr(string TxtStr, string FirstStr, string SecondStr)
        {
            if (FirstStr.IndexOf(SecondStr, 0) != -1)
                return "";
            int FirstSite = TxtStr.IndexOf(FirstStr, 0);
            int SecondSite = TxtStr.IndexOf(SecondStr, FirstSite + 1);
            if (FirstSite == -1 || SecondSite == -1)
                return "";
            return TxtStr.Substring(FirstSite + FirstStr.Length, SecondSite - FirstSite - FirstStr.Length);
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
                //string receiver = lbUserList.SelectedItem.ToString();
                ////消息的格式是：
                ////  "(@devcall \"接收者的用户名\" (uartdata \"发送内容\") (lambda x x))\n"
                //string message = 
                //rbSendMsg.Focus();
                ////将字符串转化为字符数组
                //Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                //    message.ToCharArray());
                //nsStream.Write(bytes, 0, bytes.Length);
                //Invoke(new addHandler(add), rbChatContent, message);
                //ping的格式是：
                string message = "(@devcall \"" + "UserName" + "\" (uartdata \""
                    + "true" + "\") (lambda x x))\n";
                //将字符串转化为字符数组   
                Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                    message.ToCharArray());
                nsStream.Write(bytes, 0, bytes.Length);
                //如果从服务器返回的其他消息格式，
                //则在ListBox控件中直接显示
                //Invoke(new addHandler(add), rbChatContent, message);
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Senddata(string UserName, string str)
        {
            try
            {
                //ping的格式是：
                //string message = "(ping)\n";
                string message = "(@devcall \"" + UserName + "\" (uartdata \""
                    + str + "\") (lambda x x))\n";
                //将字符串转化为字符数组   
                Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                    message.ToCharArray());
                nsStream.Write(bytes, 0, bytes.Length);
                //如果从服务器返回的其他消息格式，
                //则在ListBox控件中直接显示
                //Invoke(new addHandler(add), rbChatContent, message);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SendToUser(string UserName,string str)
        {
            try
            {
                string message = "(@devcall \"" + UserName + "\" (uartdata \""
                    + str + "\") (lambda x x))\n";
                //将字符串转化为字符数组   
                Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                    message.ToCharArray());
                nsStream.Write(bytes, 0, bytes.Length);
                //如果从服务器返回的其他消息格式，
                //则在ListBox控件中直接显示
                //Invoke(new addHandler(add), rbChatContent, message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序发生异常,异常信息:" + ex.Message);
            }
        }
        private void ping()
        {
            //ping的格式是：
            string message = "(ping)\n";
            //将字符串转化为字符数组   
            Byte[] bytes = System.Text.Encoding.Default.GetBytes(
                message.ToCharArray());
            nsStream.Write(bytes, 0, bytes.Length);
            //如果从服务器返回的其他消息格式，
            //则在ListBox控件中直接显示
            //Invoke(new addHandler(add), rbChatContent, message);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //ping的格式是：
            //  "(ping)\n"
            if (clientState == CLOSED)
                Connect();
            ping();
            //清空缓存
            timer2.Enabled = true;    
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            clientState = CLOSED;
            timer2.Enabled = false; 
            radioButton1.Text = "连接中...";
            radioButton1.Checked = false;
        }
        /// <summary>
        ///发音线程，来管理发音；如果，多处发音，可以创建多个；

        ///backgroundWorker的dowork事件，双击backgroundWorker1， 即可自动生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice1 = new SpVoice();
            voice1.Rate = 0;//调整发音语速，可以为负数，如-3，0，5
            voice1.Speak(e.Argument.ToString(), spFlags);

            
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice1 = new SpVoice();
            voice1.Rate = 0;//调整发音语速，可以为负数，如-3，0，5
            voice1.Speak(e.Argument.ToString(), spFlags);

        }

        /// <summary>
        /// 发音
        /// </summary>
        /// <param name="content"></param>
        void Speak(object content,byte Byte)
        {
            try
            {
                switch (Byte)
                {
                    case 1:
                        backgroundWorker1.RunWorkerAsync(content); break ;
                    case 2:
                        backgroundWorker2.RunWorkerAsync(content); break;
                }
                    
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        //private void tabPare1_Enter(object sender, EventArgs e)
        //{
        //    Bitmap bm = new Bitmap("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\monitoring system\\monitoring system\\bin\\Debug\\res\\nice.jpg"); //fbImage图片路径 "C:\\Users\\Administrator\\Desktop\\nice.jpg
        //    this.tabPage1.BackgroundImage = bm;//设置背景图片
        //    this.tabPage1.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图片自动适应
        //}
        private void server_Init()
        {
            Bitmap bm = new Bitmap("C:\\Users\\Administrator\\Desktop\\monitoring system1.2\\monitoring system\\monitoring system\\bin\\Release\\nice.jpg"); //fbImage图片路径
            this.tabPage1.BackgroundImage = bm;//设置背景图片
            this.tabPage1.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图片自动适应 
            this.tabControl1.Cursor = Cursors.WaitCursor;
            this.tabControl1.Cursor = Cursors.Default;
        }
        private void button3_Click(object sender, EventArgs e)
        {            
            //string strT = DateTime.Now.ToLongTimeString().ToString(); 
            //MessageBox.Show(strT);
            //string strd = DateTime.Now.ToString("d");
            //MessageBox.Show(strd);
            frm.frmBrowseList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text  == "")
            {
                MessageBox.Show("请选择类型");
                return ;
            }
            else if (cbPatient.Text == "")
            {
                MessageBox.Show("请选择病人");
                return;
            }
            GraphForm graphForm = new GraphForm();
            graphForm.Show();   
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //创建一个客户端套接字，它是Login的一个公共属性，
                //将被传递给主窗体

                //tcpClient = new TcpClient();
                list.Add(new TcpClient());
                list[0].Connect(IPAddress.Parse("123.59.77.157"),
                   Int32.Parse("9999"));
                ////向指定的IP地址的服务器发出连接请求
                //tcpClient.Connect(IPAddress.Parse("123.59.77.157"),
                //   Int32.Parse("9999"));
                //tcpClient.Connect(IPAddress.Parse("192.168.191.1"),
                // Int32.Parse("9999"));
                //获得与服务器数据交互的流通道（NetworkStream)
                nsStream = list[0].GetStream();

                //启动一个新的线程，执行方法this.ServerResponse()，
                //以便来响应从服务器发回的信息
                Thread newthread = new Thread(new ThreadStart(this.ServerResponse));
                newthread.Start();

                //向服务器发送请求命令
                if (clientState == CLOSED)
                {
                    Connect();
                }
                //创建一个服务器套接字，它是Login的一个公共属性，
                //将被传递给主窗体
                IPAddress userIP = IPAddress.Parse("192.168.191.1");
                tcpListener = new TcpListener(userIP, 6000);
                tcpListener.Start();
                ServerFlag = true;

                //启动一个新的线程，执行方法this.ServerResponse()，
                //以便来响应从服务器发回的信息
                Thread Listenerthread = new Thread(StartListen);
                Listenerthread.Start();
                clientState = CONNECTED;
                ping();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //载入病人列表
        private void frmBrowsePatient_Load(object sender, EventArgs e)
        {
            frm.frmBrowsePatient();

        }
        //载入日期列表
        private void cbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            frm.frmBrowseData();

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
