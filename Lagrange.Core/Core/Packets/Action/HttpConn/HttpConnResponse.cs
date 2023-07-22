using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Action.HttpConn;

#pragma warning disable CS8618

[ProtoContract]
internal class HttpConnResponse
{
    [ProtoMember(1)] public byte[] SigSession { get; set; }
    
    [ProtoMember(2)] public byte[] SessionKey { get; set; }
    
    [ProtoMember(3)] public List<ServerInfo> ServerInfos { get; set; }
}