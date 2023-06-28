namespace Lagrange.Core.Utility.Binary.JceStruct;

internal class JceWriter
{
    private readonly BinaryPacket _packet;

    private readonly JceStruct _jce;
    public JceWriter(JceStruct jce)
    {
        _jce = jce;
        _packet = new BinaryPacket();
    }

    public BinaryPacket Serialize()
    {
        foreach (var (tag, obj) in _jce) WriteType(ResolveType(obj), tag);

        return _packet;
    }

    private void WriteJceObject(object obj)
    {

        switch (ResolveType(obj))
        {
            case JceType.Byte:
                _packet.WriteByte((byte)obj);
                break;
            case JceType.Short:
                _packet.WriteShort((short)obj, false);
                break;
            case JceType.Int:
                _packet.WriteInt((int)obj, false);
                break;
            case JceType.Long:
                _packet.WriteLong((long)obj, false);
                break;
            case JceType.Float:
                throw new NotImplementedException();
            case JceType.Double:
                throw new NotImplementedException();
            case JceType.StringByte:
                _packet.WriteString((string)obj, BinaryPacket.Prefix.None | BinaryPacket.Prefix.Uint8);
                break;
            case JceType.StringInt:
                _packet.WriteString((string)obj, BinaryPacket.Prefix.None | BinaryPacket.Prefix.Uint32);
                break;
            case JceType.Map:
                WriteJceMap((Dictionary<object, object>)obj);
                break;
            case JceType.List:
                WriteJceList((List<object>)obj);
                break;
            case JceType.StructBegin:
                _packet.WritePacket(new JceWriter((JceStruct)obj).Serialize());
                break;
            case JceType.StructEnd:
                break;
            case JceType.ZeroTag:
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    private void WriteJceMap(Dictionary<object, object> map)
    {
        _packet.WriteInt(map.Count, false);
        foreach (var (key, value) in map)
        {
            WriteJceObject(key);
            WriteJceObject(value);
        }
    }

    private void WriteJceList(List<object> list)
    {
        _packet.WriteInt(list.Count, false);
        foreach (var item in list) WriteJceObject(item);
    }

    private void WriteType(JceType type, byte tag)
    {
        if (tag < 15) _packet.WriteByte((byte)((byte)type << 4 | tag));
        else _packet.WriteByte((byte)((byte)type << 4 | 0xF)).WriteByte(tag);
    }

    private static JceType ResolveType(object? obj) => obj switch
    {
        byte => JceType.Byte,
        bool => JceType.Byte,
        short => JceType.Short,
        int => JceType.Int,
        long => JceType.Long,
        float => JceType.Float,
        double => JceType.Double,
        string str => str.Length < 256 ? JceType.StringByte : JceType.StringInt,
        Dictionary<object, object> => JceType.Map,
        List<object> => JceType.List,
        JceStruct => JceType.StructBegin,
        null => JceType.StructEnd,
        _ => throw new InvalidOperationException()
    };
}