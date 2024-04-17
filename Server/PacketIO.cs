using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class PacketWriter : BinaryWriter
{
    private MemoryStream _ms;
    private BinaryFormatter _bf;

    public PacketWriter() : base()
    {
        _ms = new MemoryStream();
        _bf = new BinaryFormatter();
        OutStream = _ms;
    }

    public byte[] GetBytes()
    {
        Close();

        byte[] data = _ms.ToArray();

        return data;
    }
}

public class PacketReader : BinaryReader
{
    private BinaryFormatter _bf;
    public PacketReader(byte[] data) : base(new MemoryStream(data))
    {
        _bf = new BinaryFormatter();
    }
}