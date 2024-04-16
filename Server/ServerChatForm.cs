using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerChatForm : Form
    {
        public ServerChatForm()
        {
            InitializeComponent();
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            ServerSendFileForm serverSendFileForm = new ServerSendFileForm();
            serverSendFileForm.Show();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnSendFile.Enabled = true;
        }
    }
}
