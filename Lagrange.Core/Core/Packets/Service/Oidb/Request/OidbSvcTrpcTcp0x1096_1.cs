using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x1096, 1)]
internal class OidbSvcTrpcTcp0x1096_1
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public string Uid { get; set; }
    
    [ProtoMember(3)] public bool IsAdmin { get; set; }
}