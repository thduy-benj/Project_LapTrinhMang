using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerChatForm : Form
    {
        public int port;
        public IPAddress host;
        public ServerChatForm()
        {
            InitializeComponent();
            host = IPAddress.Parse(tbHost.Text);
            port = int.Parse(tbPort.Text);
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            ServerSendFileForm serverSendFileForm = new ServerSendFileForm();
            serverSendFileForm.ShowDialog();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void tbHost_TextChanged(object sender, EventArgs e)
        {
            host = IPAddress.Parse(tbHost.Text);
        }

        private void tbPort_TextChanged(object sender, EventArgs e)
        {
            port = int.Parse(tbPort.Text);
        }
    }
}
