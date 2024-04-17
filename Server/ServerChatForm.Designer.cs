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
            SuspendLayout();
            // 
            // tbPort
            // 
            tbPort.Location = new Point(55, 52);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(125, 27);
            tbPort.TabIndex = 9;
            tbPort.Text = "2121";
            tbPort.TextChanged += tbPort_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 52);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 8;
            label2.Text = "Port";
            // 
            // tbHost
            // 
            tbHost.Location = new Point(55, 12);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(125, 27);
            tbHost.TabIndex = 7;
            tbHost.Text = "127.0.0.1";
            tbHost.TextChanged += tbHost_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 15);
            label1.Name = "label1";
            label1.Size = new Size(40, 20);
            label1.TabIndex = 6;
            label1.Text = "Host";
            // 
            // btnSendFile
            // 
            btnSendFile.Location = new Point(9, 120);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(94, 29);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "Send File";
            btnSendFile.UseVisualStyleBackColor = true;
            btnSendFile.Click += btnSendFile_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(9, 85);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 10;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // ServerChatForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnConnect);
            Controls.Add(tbPort);
            Controls.Add(label2);
            Controls.Add(tbHost);
            Controls.Add(label1);
            Controls.Add(btnSendFile);
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
    }
}