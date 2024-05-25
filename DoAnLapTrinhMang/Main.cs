namespace DoAnLapTrinhMang
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendFile_Client sendFile_Client = new SendFile_Client();
            sendFile_Client.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Chat chat = new Chat();
            chat.Show();
        }
    }
}