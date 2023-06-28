namespace Lagrange.Core.Utility.Binary.JceStruct;

internal class JceReader
{
    private readonly BinaryPacket _packet;

    private bool _simpleListStruct;

    public JceReader(BinaryPacket packet) => _packet = packet;

    public JceReader(byte[] data) => _packet = new BinaryPacket(data);

    public JceStruct Deserialize(bool simpleListStruct = false)
    {
        _simpleListStruct = simpleListStruct;
        var jceStruct = new JceStruct();

        while (_packet.Remaining > 0)
        {
            var jceObject = ReadJceObject(out byte tag);
            if (jceObject != null) jceStruct[tag] = jceObject;
            else break; // represents struct end
        }

        return jceStruct;
    }

    private object? ReadJceObject(out byte tag)
    {
        var type = ReadType(out tag);
        return type switch
        {
            JceType.Byte => _packet.ReadByte(),
            JceType.Short => _packet.ReadShort(false),
            JceType.Int => _packet.ReadInt(false),
            JceType.Long => _packet.ReadLong(false),
            JceType.Float => throw new NotImplementedException(),
            JceType.Double => throw new NotImplementedException(),
            JceType.StringByte => _packet.ReadString(BinaryPacket.Prefix.None | BinaryPacket.Prefix.Uint8),
            JceType.StringInt => _packet.ReadString(BinaryPacket.Prefix.None | BinaryPacket.Prefix.Uint32),
            JceType.Map => ReadJceMap(),
            JceType.List => ReadJceList(),
            JceType.StructBegin => new JceReader(_packet).Deserialize(_simpleListStruct),
            JceType.StructEnd => null,
            JceType.ZeroTag => (byte)0,
            JceType.SimpleList => ReadJceSimpleList(),
            _ => throw new InvalidOperationException()
        };
    }

    private int ReadJceNumber() => ReadType(out _) switch
    {
        JceType.Byte => _packet.ReadByte(),
        JceType.Short => _packet.ReadShort(false),
        JceType.Int => _packet.ReadInt(false),
        JceType.Long => (int)_packet.ReadLong(false),
        JceType.ZeroTag => 0,
        _ => throw new InvalidOperationException(),
    };

    private Dictionary<object, object> ReadJceMap()
    {
        int count = ReadJceNumber();
        var map = new Dictionary<object, object>(count);
        for (int i = 0; i < count; i++)
        {
            var key = ReadJceObject(out _);
            var value = ReadJceObject(out _);
            if (key != null && value != null) map[key] = value;
        }

        return map;
    }

    private List<object> ReadJceList()
    {
        int count = ReadJceNumber();
        var list = new List<object>(count);
        for (int i = 0; i < count; i++)
        {
            var value = ReadJceObject(out _);
            if (value != null) list.Add(value);
        }

        return list;
    }

    private object ReadJceSimpleList()
    {
        _packet.Skip(1);
        
        var array = _packet.ReadBytes(ReadJceNumber());
        return _simpleListStruct ? new JceReader(array).Deserialize(true) : array;
    }

    private JceType ReadType(out byte tag)
    {
        byte value = _packet.ReadByte(); // lower 4 bit is type, higher 4 bit is tag
        byte type = (byte)(value & 0xF); // lower 4 bit
        tag = (byte)(value >> 4); // higher 4 bit

        if (tag == 0xF) tag = (byte)(_packet.ReadByte() & 0xFF);
        return (JceType)type;
    }
}