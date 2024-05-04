using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    public partial class ServerSendFileForm : Form
    {
        private ServerChatForm chatForm;
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private RSACryptoServiceProvider rsaServer;
        private string serverPubKey;
        private string clientPubKey;
        private IPEndPoint remoteEndPoint;

        private Listener listener;
        private Transfer transferClient;
        private string outputFolder;
        private System.Windows.Forms.Timer tmrOverallProg;
        private bool serverRunning;
        public ServerSendFileForm()
        {
            InitializeComponent();
            chatForm = new ServerChatForm();
            remoteEndPoint = new IPEndPoint(chatForm.host, chatForm.port);

            listener = new Listener();
            listener.Accepted += listener_Accepted;
            tmrOverallProg = new System.Windows.Forms.Timer();
            tmrOverallProg.Interval = 1000;
            tmrOverallProg.Tick += tmrOverallProg_Tick;
            outputFolder = "Server";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
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

        private async void ServerSendFileForm_Load(object sender, EventArgs e)
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, chatForm.port);
                tcpListener.Start();
                tcpClient = await tcpListener.AcceptTcpClientAsync();
                if (tcpClient != null)
                {
                    networkStream = tcpClient.GetStream();
                    rsaServer = new RSACryptoServiceProvider(2048);
                    serverPubKey = rsaServer.ToXmlString(false);
                    byte[] byteArray = Encoding.UTF8.GetBytes(serverPubKey);
                    await networkStream.WriteAsync(byteArray, 0, byteArray.Length);
                    byte[] recievedData = new byte[2048];
                    int size = await networkStream.ReadAsync(recievedData, 0, recievedData.Length);
                    clientPubKey = Encoding.UTF8.GetString(recievedData, 0, size);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server-side error");
            }
            tcpClient?.Close();
            networkStream?.Close();
            tcpListener?.Stop();

            //MessageBox.Show("server key:\n" + serverPubKey + "\nclient key:\n" + clientPubKey, "server side");
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (serverRunning) return;
            serverRunning = true;
            outputFolder = "Server";
            try
            {
                listener.Start(remoteEndPoint.Port);
                setConnectionStatus("Waiting...");
                btnStartServer.Enabled = false;
                btnStopServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in Main.btnStartServer");

            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (!serverRunning) return;
            if (transferClient != null)
            {
                transferClient.Close();
                transferClient = null;
            }
            listener.Stop();
            tmrOverallProg.Stop();
            setConnectionStatus("-");
            serverRunning = false;
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;
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
            i.SubItems.Add(queue.Type == QueueType.Download ? "Download" : "Upload");
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

                if (queue.Progress == 100 || !queue.Running){ i.Remove(); }
            }
        }
    }
}