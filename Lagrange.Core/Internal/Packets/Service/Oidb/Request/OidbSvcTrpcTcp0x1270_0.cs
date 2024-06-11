using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x1270_0
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x1270_0Device Device { get; set; }
    
    [ProtoMember(2)] public bool EnableNoNeed { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x1270_0Device
{
    [ProtoMember(1)] public byte[] Guid { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; }
    
    [ProtoMember(3)] public string PackageName { get; set; }
}