namespace Lagrange.Core.Utility.Binary.JceStruct;

[Serializable]
internal class UniPacket
{
    public byte Version { get; set; }
    
    public byte PacketType { get; set; }
    
    public byte MessageType { get; set; }
    
    public int RequestId { get; set; }

    public string ServantName { get; set; }
    
    public string FuncName { get; set; }
    
    public JceStruct Buffer { get; set; }
    
    public byte Timeout { get; set; }
    
    public Dictionary<string, string> Status { get; set; }
    
    public Dictionary<string, string> Context { get; set; }
    
    public UniPacket(byte version, byte packetType, byte messageType, int requestId, string servantName, string funcName,
        JceStruct buffer, byte timeout, Dictionary<string, string>? status = null, Dictionary<string, string>? context = null)
    {
        Version = version;
        PacketType = packetType;
        MessageType = messageType;
        RequestId = requestId;
        ServantName = servantName;
        FuncName = funcName;
        Buffer = buffer;
        Timeout = timeout;
        Status = status ?? new Dictionary<string, string>();
        Context = context ?? new Dictionary<string, string>();
    }

    public UniPacket(byte[] buffer)
    {
        var jce = new JceReader(buffer).Deserialize();

        Version = (byte)jce[1];
        PacketType = (byte)jce[2];
        MessageType = (byte)jce[3];
        RequestId = (byte)jce[4];
        ServantName = (string)jce[5];
        FuncName = (string)jce[6];
        Buffer = new JceReader((byte[])jce[7]).Deserialize(true);
        Timeout = (byte)jce[8];
        Status = Cast<string, string>((Dictionary<object, object>)jce[9]);
        Context = Cast<string, string>((Dictionary<object, object>)jce[10]);
    }
    
    private static Dictionary<TU, TV> Cast<TU, TV>(Dictionary<object, object> dict) where TU : notnull
    {
        var result = new Dictionary<TU, TV>();
        foreach (var (key, value) in dict) result[(TU)key] = (TV)value;
        return result;
    }
}