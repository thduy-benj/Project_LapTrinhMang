namespace Server
{
    partial class ServerChatForm
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
            tbPort = new TextBox();
            label2 = new Label();
            tbHost = new TextBox();
            label1 = new Label();
            btnSendFile = new Button();
            btnConnect = new Button();
            txtLog = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // tbPort
            // 
            tbPort.Location = new Point(89, 83);
            tbPort.Margin = new Padding(5, 5, 5, 5);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(201, 39);
            tbPort.TabIndex = 9;
            tbPort.Text = "2121";
            tbPort.TextChanged += tbPort_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 83);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 32);
            label2.TabIndex = 8;
            label2.Text = "Port";
            // 
            // tbHost
            // 
            tbHost.Location = new Point(89, 19);
            tbHost.Margin = new Padding(5, 5, 5, 5);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(201, 39);
            tbHost.TabIndex = 7;
            tbHost.Text = "127.0.0.1";
            tbHost.TextChanged += tbHost_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 24);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 32);
            label1.TabIndex = 6;
            label1.Text = "Host";
            // 
            // btnSendFile
            // 
            btnSendFile.Location = new Point(15, 192);
            btnSendFile.Margin = new Padding(5, 5, 5, 5);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(153, 46);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "Send File";
            btnSendFile.UseVisualStyleBackColor = true;
            btnSendFile.Click += btnSendFile_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(15, 136);
            btnConnect.Margin = new Padding(5, 5, 5, 5);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(153, 46);
            btnConnect.TabIndex = 10;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(322, 70);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(484, 548);
            txtLog.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(322, 26);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(139, 32);
            label3.TabIndex = 13;
            label3.Text = "Hành động:";
            // 
            // ServerChatForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(910, 660);
            Controls.Add(label3);
            Controls.Add(txtLog);
            Controls.Add(btnConnect);
            Controls.Add(tbPort);
            Controls.Add(label2);
            Controls.Add(tbHost);
            Controls.Add(label1);
            Controls.Add(btnSendFile);
            Margin = new Padding(5, 5, 5, 5);
            Name = "ServerChatForm";
            Text = "ServerChatForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbPort;
        private Label label2;
        private TextBox tbHost;
        private Label label1;
        private Button btnSendFile;
        private Button btnConnect;
        private TextBox txtLog;
        private Label label3;
    }
}