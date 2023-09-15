using ProtoBuf;
#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

/// <summary>
/// Upload Offline File
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xE37, 1700)]
internal class OidbSvcTrpcTcp0xE37_1700
{
    [ProtoMember(1)] public uint SubCommand => 1700;
    
    [ProtoMember(2)] public int Field2 { get; set; } // Unknown
    
    [ProtoMember(101)] public int Field101 { get; set; } // Unknown
    
    [ProtoMember(102)] public int Field102 { get; set; } // Unknown
    
    [ProtoMember(200)] public int Field200 { get; set; } // Unknown
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1700Body
{
    [ProtoMember(10)] public string SenderUid { get; set; }
    
    [ProtoMember(20)] public string ReceiverUid { get; set; }
    
    [ProtoMember(30)] public uint FileSize { get; set; }
    
    [ProtoMember(40)] public string FileName { get; set; }
    
    [ProtoMember(50)] public byte[] Md510MCheckSum { get; set; }
    
    [ProtoMember(60)] public byte[] Sha1CheckSum { get; set; }
    
    [ProtoMember(70)] public string LocalPath { get; set; }
    
    [ProtoMember(110)] public byte[] Md5CheckSum { get; set; }
    
    [ProtoMember(120)] public byte[] Sha3CheckSum { get; set; }
}