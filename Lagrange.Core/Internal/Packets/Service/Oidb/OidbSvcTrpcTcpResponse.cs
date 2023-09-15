using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb;

[ProtoContract]
internal class OidbSvcTrpcTcpResponse<T>
{
    [ProtoMember(1)] public uint Command { get; set; }
    
    [ProtoMember(2)] public uint SubCommand { get; set; }
    
    [ProtoMember(3)] public uint ErrorCode { get; set; }
    
    [ProtoMember(4)] public T Body { get; set; }
    
    [ProtoMember(5)] public string Field5 { get; set; }
    
    [ProtoMember(11)] public List<OidbProperty> Properties { get; set; }
}