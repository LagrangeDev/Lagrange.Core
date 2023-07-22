using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Action.HttpConn;

[ProtoContract]
public class ServerAddr
{
    [ProtoMember(1)] public uint Type { get; set; }
    
    [ProtoMember(2)] public uint Ip { get; set; }
    
    [ProtoMember(3)] public uint Port { get; set; }
    
    [ProtoMember(4)] public uint Area { get; set; }
}