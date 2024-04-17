using System;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

public delegate void TcpClientEventHandler(object sender, TcpClientEventArgs e);

public class TcpClientEventArgs : EventArgs
{
    public TcpClient Client { get; }

    public TcpClientEventArgs(TcpClient client)
    {
        Client = client;
    }
}

public class Listener
{
    private TcpListener listener;
    private bool running = false;
    private int port = -1;

    public event TcpClientEventHandler Accepted;

    public Listener() { }

    public void Start(int port)
    {
        if (running) return;

        this.port = port;
        running = true;
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        listener.BeginAcceptTcpClient(AcceptCallback, null);
    }

    public void Stop()
    {
        if (!running) return;

        running = false;
        listener.Stop();
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        try
        {
            TcpClient client = listener.EndAcceptTcpClient(ar);

            if (Accepted != null)
            {
                Accepted(this, new TcpClientEventArgs(client));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        if (running)
            listener.BeginAcceptTcpClient(AcceptCallback, null);
    }
    public bool getRunning() { return running; }
}