namespace Client
{
    partial class ClientChatForm
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
            btnSendFile = new Button();
            label1 = new Label();
            tbHost = new TextBox();
            label2 = new Label();
            tbPort = new TextBox();
            btnConnect = new Button();
            txtLog = new TextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            txtUser = new TextBox();
            label3 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // btnSendFile
            // 
            btnSendFile.Location = new Point(3, 427);
            btnSendFile.Margin = new Padding(5);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(120, 46);
            btnSendFile.TabIndex = 0;
            btnSendFile.Text = "Send File";
            btnSendFile.UseVisualStyleBackColor = true;
            btnSendFile.Click += btnSendFile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 25);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 32);
            label1.TabIndex = 1;
            label1.Text = "Host";
            // 
            // tbHost
            // 
            tbHost.Location = new Point(95, 25);
            tbHost.Margin = new Padding(5);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(221, 39);
            tbHost.TabIndex = 2;
            tbHost.Text = "127.0.0.1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 74);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 32);
            label2.TabIndex = 3;
            label2.Text = "Port";
            // 
            // tbPort
            // 
            tbPort.Location = new Point(95, 74);
            tbPort.Margin = new Padding(5);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(221, 39);
            tbPort.TabIndex = 4;
            tbPort.Text = "2121";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(429, 74);
            btnConnect.Margin = new Padding(5);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(179, 44);
            btnConnect.TabIndex = 5;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtLog
            // 
            txtLog.Cursor = Cursors.IBeam;
            txtLog.Location = new Point(12, 181);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(727, 217);
            txtLog.TabIndex = 7;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(131, 427);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(537, 41);
            txtMessage.TabIndex = 8;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(674, 422);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(78, 46);
            btnSend.TabIndex = 9;
            btnSend.Text = "Gửi";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(404, 25);
            txtUser.Margin = new Padding(5);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(248, 39);
            txtUser.TabIndex = 11;
            txtUser.Text = "Tran Trung Long";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(342, 25);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(52, 32);
            label3.TabIndex = 10;
            label3.Text = "Tên";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(5, 129);
            label5.Name = "label5";
            label5.Size = new Size(120, 32);
            label5.TabIndex = 13;
            label5.Text = "Nội dung:";
            // 
            // ClientChatForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 530);
            Controls.Add(label5);
            Controls.Add(txtUser);
            Controls.Add(label3);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(txtLog);
            Controls.Add(btnConnect);
            Controls.Add(tbPort);
            Controls.Add(label2);
            Controls.Add(tbHost);
            Controls.Add(label1);
            Controls.Add(btnSendFile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5);
            Name = "ClientChatForm";
            Text = "ClientChatForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSendFile;
        private Label label1;
        private TextBox tbHost;
        private Label label2;
        private TextBox tbPort;
        private Button btnConnect;
        private TextBox txtLog;
        private TextBox txtMessage;
        private Button btnSend;
        private TextBox txtUser;
        private Label label3;
        private Label label5;
    }
}