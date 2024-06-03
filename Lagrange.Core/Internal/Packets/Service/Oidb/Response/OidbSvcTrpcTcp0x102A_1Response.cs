using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x102A_1Response
{
    [ProtoMember(2)] public uint Field1 { get; set; }
    
    [ProtoMember(3)] public string ClientKey { get; set; }
    
    [ProtoMember(4)] public uint Expiration { get; set; }
}