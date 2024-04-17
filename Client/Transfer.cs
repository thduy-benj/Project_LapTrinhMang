using System.Net;
using System.Net.Sockets;

namespace Client
{
    public delegate void TransferEventHandler(object sender, TransferQueue queue);
    public delegate void ConnectCallback(object sender, string error);

    public class Transfer
    {
        //private Socket baseSocket;
        private TcpClient tcpClient;
        private byte[] buffer = new byte[8192];
        private ConnectCallback connectCallback;
        private Dictionary<int, TransferQueue> transfers = new Dictionary<int, TransferQueue>();

        public Dictionary<int, TransferQueue> Transfers
        {
            get { return transfers; }
        }
        public bool Closed { get; private set; }
        public string OutputFolder { get; set; }
        public IPEndPoint EndPoint {  get; private set; }

        public event TransferEventHandler Queued; 
        public event TransferEventHandler ProgressChanged;
        public event TransferEventHandler Complete; 
        public event EventHandler Disconnected;

        public Transfer()
        {
            tcpClient = new TcpClient();
        }

        public Transfer(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            EndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
        }

        public void Connect(IPAddress hostName, int port, ConnectCallback callback)
        {
            connectCallback = callback;
            tcpClient.BeginConnect(hostName, port, ConnectCallback, null);
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            string er = null;
            try
            {
                tcpClient.EndConnect(ar);
                EndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                er = ex.Message;
            }
            connectCallback(this, er);
        }
        public void Run()
        {
            try
            { 
                NetworkStream networkStream = tcpClient.GetStream();
                networkStream.BeginRead(buffer, 0, buffer.Length, receiveCallback, null);
            }
            catch
            {
                Close();
            }
        }
        public void QueueTransfer(string fileName)
        {
            try
            {
                TransferQueue queue = TransferQueue.CreateUploadQueue(this, fileName);
                transfers.Add(queue.ID, queue);

                PacketWriter pw = new PacketWriter();
                pw.Write((byte)Headers.Queue);
                pw.Write(queue.ID);
                pw.Write(queue.Filename);
                pw.Write(queue.Length);
                Send(pw.GetBytes());

                if (Queued != null) { Queued(this, queue); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in TransferClient.QueueTransfer");
            }
        }
        public void StartTransfer(TransferQueue queue)
        {
            PacketWriter pw = new PacketWriter();
            pw.Write((byte)Headers.Start);
            pw.Write(queue.ID);
            Send(pw.GetBytes());
        }
        public int GetOverallProgress()
        {
            int overall = 0;
            try
            {
                foreach (KeyValuePair<int, TransferQueue> pair in transfers)
                {
                    overall += pair.Value.Progress;
                }

                if (overall > 0)
                {
                    overall = (overall * 100) / (transfers.Count * 100);
                }
            }
            catch { overall = 0; }

            return overall;
        }
        public void Send(byte[] data)
        {
            if (Closed) return;
            lock (this)
            {
                try
                {
                    tcpClient.GetStream().Write(BitConverter.GetBytes(data.Length), 0, 4);
                    tcpClient.GetStream().Write(data, 0, data.Length);
                }
                catch
                {
                    Close();
                }
            }
        }
        public void Close()
        {
            if (Closed)
                return;

            Closed = true;
            tcpClient.Close();
            transfers.Clear(); 
            transfers = null;
            buffer = null;
            OutputFolder = null;

            if (Disconnected != null)
                Disconnected(this, EventArgs.Empty);
        }
        private void process()
        {
            PacketReader pr = new PacketReader(buffer);
            Headers header = (Headers)pr.ReadByte();

            switch (header)
            {
                case Headers.Queue:
                    {
                        int id = pr.ReadInt32();
                        string fileName = pr.ReadString();
                        long length = pr.ReadInt64();

                        TransferQueue queue = TransferQueue.CreateDownloadQueue(this, id, Path.Combine(OutputFolder, Path.GetFileName(fileName)), length);

                        transfers.Add(id, queue);

                        if (Queued != null)
                        {
                            Queued(this, queue);
                        }
                    }
                    break;
                case Headers.Start:
                    {
                        int id = pr.ReadInt32();
                        if (transfers.ContainsKey(id))
                        {
                            transfers[id].Start();
                        }
                    }
                    break;
                case Headers.Chunk:
                    {
                        int id = pr.ReadInt32();
                        long index = pr.ReadInt64();
                        int size = pr.ReadInt32();
                        byte[] buffer = pr.ReadBytes(size);
                        TransferQueue queue = transfers[id];
                        queue.Write(buffer, index);
                        queue.Progress = (int)(queue.Transferred * 100 / queue.Length);
                        if (queue.LastProgress < queue.Progress)
                        {
                            queue.LastProgress = queue.Progress;
                            if (ProgressChanged != null) { ProgressChanged(this, queue); }

                            if (queue.Progress == 100)
                            {
                                queue.Close();

                                if (Complete != null) { Complete(this, queue); }
                            }
                        }
                    }
                    break;
            }
            pr.Dispose();
        }
        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                int found = networkStream.EndRead(ar);
                if (found >= 4)
                {
                    int size = BitConverter.ToInt32(buffer, 0);
                    int read = networkStream.Read(buffer, 0, size);
                    // Các buffer có thể bị phân mảnh trong quá trình truyền nên dùng phòng while để đọc đủ 1 buffer
                    while (read < size)
                    {
                        read += networkStream.Read(buffer, read, size - read);
                    }
                    process();
                }

                Run();
            }
            catch
            {
                Close();
            }
        }
        internal void callProgressChanged(TransferQueue queue)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, queue);
            }
        }
    }
}
