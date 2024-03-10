using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
internal class OidbSvcTrpcTcp0xEAC
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Sequence { get; set; }
    
    [ProtoMember(3)] public uint Random { get; set; }
}