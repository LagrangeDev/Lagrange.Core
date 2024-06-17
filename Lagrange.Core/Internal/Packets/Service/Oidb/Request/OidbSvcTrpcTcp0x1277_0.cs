using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
[OidbSvcTrpcTcp(0x1277, 0)]
internal class OidbSvcTrpcTcp0x1277_0
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x1277_0Body Body { get; set; }
}


[ProtoContract]
internal class OidbSvcTrpcTcp0x1277_0Body
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x1277_0Device Device { get; set; }
    
    [ProtoMember(2)] public bool GuidEncryptedType { get; set; }
    
    [ProtoMember(3)] public bool AutoLogin { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x1277_0Device
{
    [ProtoMember(1)] public byte[] Guid { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; }
    
    [ProtoMember(3)] public string PackageName { get; set; }
}