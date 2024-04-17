using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Client
{
    public partial class ClientSendFileForm : Form
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private RSACryptoServiceProvider rsaClient;
        private ClientChatForm clientChatForm = new ClientChatForm();
        private string clientPubKey;
        private string serverPubKey;
        private IPEndPoint remoteEndPoint;

        private Listener listener;
        private Transfer transferClient;
        private string outputFolder;
        private System.Windows.Forms.Timer tmrOverallProg;
        private bool serverRunning;
        public ClientSendFileForm()
        {
            InitializeComponent();
            remoteEndPoint = new IPEndPoint(clientChatForm.getHost(), clientChatForm.getPort());
            listener = new Listener();
            listener.Accepted += listener_Accepted;
            tmrOverallProg = new System.Windows.Forms.Timer();
            tmrOverallProg.Interval = 1000;
            tmrOverallProg.Tick += tmrOverallProg_Tick;
            outputFolder = "Client";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }

        private async void ClientSendFileForm_Load(object sender, EventArgs e)
        {
            bool isNotConnect = true;
            while (isNotConnect)
            {
                try
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(clientChatForm.getHost(), clientChatForm.getPort());
                    networkStream = tcpClient.GetStream();
                    byte[] recievedData = new byte[2048];
                    networkStream.Read(recievedData, 0, recievedData.Length);
                    serverPubKey = Encoding.UTF8.GetString(recievedData, 0, recievedData.Length);
                    isNotConnect = false;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "client side");
                    await Task.Delay(1000);
                }
            }
            try
            {
                rsaClient = new RSACryptoServiceProvider(2048);
                clientPubKey = rsaClient.ToXmlString(false);
                byte[] byteArray = Encoding.UTF8.GetBytes(clientPubKey);
                networkStream.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {

            }
            tcpClient?.Close();
            networkStream?.Close();
            //MessageBox.Show("server key:\n" + serverPubKey + "\nclient key:\n" + clientPubKey, "client side");
        }

        void tmrOverallProg_Tick(object sender, EventArgs e)
        {
            if (transferClient == null) return;
            progressOverall.Value = transferClient.GetOverallProgress();
        }

        void listener_Accepted(object sender, TcpClientEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new TcpClientEventHandler(listener_Accepted), sender, e);
                return;
            }
            listener.Stop();
            transferClient = new Transfer(e.Client);

            transferClient.OutputFolder = outputFolder;
            registerEvents();
            transferClient.Run();
            tmrOverallProg.Start();
            setConnectionStatus(transferClient.EndPoint.Address.ToString());
        }

        private void setConnectionStatus(string connectedTo)
        {
            lblConnected.Text = "Connection: " + connectedTo;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (transferClient == null) return;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Files (*.*)|*.*";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        transferClient.QueueTransfer(file);
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (transferClient == null)
            {
                transferClient = new Transfer();
                transferClient.Connect(remoteEndPoint.Address, remoteEndPoint.Port, connectCallback);
                Enabled = false;
            }
            else
            {
                transferClient.Close();
                transferClient = null;
            }
        }

        private void connectCallback(object sender, string error)
        {
            if (InvokeRequired)
            {
                Invoke(new ConnectCallback(connectCallback), sender, error);
                return;
            }
            Enabled = true;
            if (error != null)
            {
                transferClient.Close();
                transferClient = null;
                MessageBox.Show(error, "Error in Main.connectCallBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            registerEvents();
            transferClient.OutputFolder = outputFolder;
            transferClient.Run();
            setConnectionStatus(transferClient.EndPoint.Address.ToString());
            tmrOverallProg.Start();
            btnConnect.Text = "Disconnect";
        }

        private void registerEvents()
        {
            transferClient.Complete += transferClient_Complete;
            transferClient.Disconnected += transferClient_Disconnected;
            transferClient.ProgressChanged += transferClient_ProgressChanged;
            transferClient.Queued += transferClient_Queued;
        }

        private void deregisterEvents()
        {
            if (transferClient == null) return;
            transferClient.Complete -= transferClient_Complete;
            transferClient.Disconnected -= transferClient_Disconnected;
            transferClient.ProgressChanged -= transferClient_ProgressChanged;
            transferClient.Queued -= transferClient_Queued;
        }

        void transferClient_Complete(object sender, TransferQueue queue)
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        void transferClient_Disconnected(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(transferClient_Disconnected), sender, e);
                return;
            }
            deregisterEvents();
            foreach (ListViewItem item in lstTransfers.Items)
            {
                TransferQueue queue = (TransferQueue)item.Tag;
                queue.Close();
            }
            lstTransfers.Items.Clear();
            progressOverall.Value = 0;
            transferClient = null;
            setConnectionStatus("-");
            if (serverRunning)
            {
                listener.Stop();
                listener.Start(remoteEndPoint.Port);
                setConnectionStatus("Waiting...");
            }
            else
            {
                btnConnect.Text = "Connect";
            }
        }

        void transferClient_ProgressChanged(object sender, TransferQueue queue)
        {
            if (InvokeRequired)
            {
                Invoke(new TransferEventHandler(transferClient_ProgressChanged), sender, queue);
                return;
            }

            lstTransfers.Items[queue.ID.ToString()].SubItems[3].Text = queue.Progress + "%";
        }

        void transferClient_Queued(object sender, TransferQueue queue)
        {
            if (InvokeRequired)
            {
                Invoke(new TransferEventHandler(transferClient_Queued), sender, queue);
                return;
            }

            ListViewItem i = new ListViewItem();
            i.Text = queue.ID.ToString();
            i.SubItems.Add(queue.Filename);
            i.SubItems.Add(queue.Length.ToString());
            i.SubItems.Add("0%");
            i.Tag = queue;
            i.Name = queue.ID.ToString();
            lstTransfers.Items.Add(i);
            i.EnsureVisible();

            if (queue.Type == QueueType.Download)
            {
                transferClient.StartTransfer(queue);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in lstTransfers.Items)
            {
                TransferQueue queue = (TransferQueue)i.Tag;

                if (queue.Progress == 100 || !queue.Running)
                {
                    i.Remove();
                }
            }
        }

        private void btnSafeSend_Click(object sender, EventArgs e)
        {

        }
    }
}