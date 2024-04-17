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

namespace Client
{
    public partial class ClientChatForm : Form
    {
        public ClientChatForm()
        {
            InitializeComponent();
        }

        public IPAddress getHost()
        {
            return IPAddress.Parse(tbHost.Text);
        }

        public int getPort()
        {
            return int.Parse(tbPort.Text);
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            ClientSendFileForm clientSendFileForm = new ClientSendFileForm();
            clientSendFileForm.ShowDialog();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }
    }
}
