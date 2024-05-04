namespace Client
{
    public enum QueueType : byte
    {
        Download,
        Upload
    }

    public class TransferQueue
    {
        // Mỗi buffer sẽ có 8KB = 8192 trong đó 
        // * 1 Byte = Header
        // * 4 Bytes = ID
        // * 8 Bytes = Index
        // * 4 Bytes = read
        // => 8175 Bytes = file_buffer
        private const int BUFFER_SIZE = 8175;
        private static byte[] file_buffer = new byte[BUFFER_SIZE];
        public int ID; // ID của file
        public int Progress, LastProgress;
        public long Transferred;
        public long Index; // dùng để check xem đã truyền hết file chưa
        public long Length; // kích thước của file
        public bool Running; // Có đang truyền file hay không 
        public string Filename; 
        public QueueType Type; // Up hay Down
        public Transfer Client;
        public Thread Thread; // chạy thread (%) của từng file được up và down
        public FileStream FS;

        public static TransferQueue CreateUploadQueue(Transfer client, string fileName)
        {
            try
            {
                var queue = new TransferQueue();
                queue.Filename = Path.GetFileName(fileName);
                queue.Client = client;
                queue.Type = QueueType.Upload;
                queue.FS = new FileStream(fileName, FileMode.Open);
                queue.Thread = new Thread(new ParameterizedThreadStart(transferProc));
                queue.Thread.IsBackground = true;
                queue.ID = Program.random.Next();
                queue.Length = queue.FS.Length;
                return queue;
            }
            catch
            {
                return null;
            }
        }

        public static TransferQueue CreateDownloadQueue(Transfer client, int id, string saveName, long length)
        {
            try
            {
                TransferQueue queue = new TransferQueue();
                queue.Filename = Path.GetFileName(saveName);
                queue.Client = client;
                queue.Type = QueueType.Download;
                queue.FS = new FileStream(saveName, FileMode.Create);
                queue.FS.SetLength(length);
                queue.Length = length;
                queue.ID = id;
                return queue;
            }
            catch
            {
                return null;
            }
        }
        private TransferQueue() { Running = true; }
        public void Start()
        {
            Running = true;
            Thread.Start(this);
        }
        public void Close()
        {
            try
            {
                Client.Transfers.Remove(ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Running = false;
            FS.Close();

            Client = null;
        }
        public void Write(byte[] bytes, long index)
        {
            lock (this)
            {
                FS.Position = index;
                FS.Write(bytes, 0, bytes.Length);
                Transferred += bytes.Length;
            }
        }
        private static void transferProc(object o)
        {
            TransferQueue queue = (TransferQueue)o;

            while (queue.Running && queue.Index < queue.Length)
            {
                if (!queue.Running)
                {
                    break;
                }
                lock (file_buffer)
                {
                    queue.FS.Position = queue.Index;
                    int read = queue.FS.Read(file_buffer, 0, file_buffer.Length);
                    PacketWriter pw = new PacketWriter();

                    pw.Write((byte)Headers.Chunk);
                    pw.Write(queue.ID);
                    pw.Write(queue.Index);
                    pw.Write(read);
                    pw.Write(file_buffer, 0, read);
                    queue.Transferred += read;
                    queue.Index += read;

                    queue.Client.Send(pw.GetBytes());

                    queue.Progress = (int)(queue.Transferred * 100 / queue.Length);

                    if (queue.LastProgress < queue.Progress)
                    {
                        queue.LastProgress = queue.Progress;

                        queue.Client.callProgressChanged(queue);
                    }
                    Thread.Sleep(1);
                }
            }
            queue.Close();
        }
    }
}
