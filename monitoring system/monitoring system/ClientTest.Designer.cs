namespace monitoring_system
{
    partial class ClientTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbUserList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btLogin = new System.Windows.Forms.Button();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbChatContent = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbSendMsg = new System.Windows.Forms.RichTextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbUserList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 436);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "在线用户列表";
            // 
            // lbUserList
            // 
            this.lbUserList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUserList.FormattingEnabled = true;
            this.lbUserList.ItemHeight = 12;
            this.lbUserList.Location = new System.Drawing.Point(3, 17);
            this.lbUserList.Name = "lbUserList";
            this.lbUserList.Size = new System.Drawing.Size(144, 416);
            this.lbUserList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btLogin);
            this.groupBox2.Controls.Add(this.tbUserName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbServerPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbServer);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(189, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 46);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置登录服务器";
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(458, 16);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(75, 23);
            this.btLogin.TabIndex = 6;
            this.btLogin.Text = "登录";
            this.btLogin.UseVisualStyleBackColor = true;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(350, 17);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(100, 21);
            this.tbUserName.TabIndex = 5;
            this.tbUserName.Text = "eross.lau";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(303, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "用户名";
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(197, 17);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(100, 21);
            this.tbServerPort.TabIndex = 3;
            this.tbServerPort.Text = "9999";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(54, 17);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(100, 21);
            this.tbServer.TabIndex = 1;
            this.tbServer.Text = "123.59.77.157";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbChatContent);
            this.groupBox3.Location = new System.Drawing.Point(189, 64);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(543, 233);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "聊天内容";
            // 
            // rbChatContent
            // 
            this.rbChatContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rbChatContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbChatContent.Location = new System.Drawing.Point(3, 17);
            this.rbChatContent.Name = "rbChatContent";
            this.rbChatContent.Size = new System.Drawing.Size(537, 213);
            this.rbChatContent.TabIndex = 0;
            this.rbChatContent.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbSendMsg);
            this.groupBox4.Location = new System.Drawing.Point(189, 304);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(421, 144);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "发送信息";
            // 
            // rbSendMsg
            // 
            this.rbSendMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rbSendMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendMsg.Location = new System.Drawing.Point(3, 17);
            this.rbSendMsg.Name = "rbSendMsg";
            this.rbSendMsg.Size = new System.Drawing.Size(415, 124);
            this.rbSendMsg.TabIndex = 0;
            this.rbSendMsg.Text = "";
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(619, 367);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(113, 39);
            this.btSend.TabIndex = 12;
            this.btSend.Text = "发送";
            this.btSend.UseVisualStyleBackColor = true;
            // 
            // ClientTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 487);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ClientTest";
            this.Text = "ClientTest";
            this.Load += new System.EventHandler(this.ClientTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbUserList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox rbChatContent;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox rbSendMsg;
        private System.Windows.Forms.Button btSend;
    }
}