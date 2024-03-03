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
    [ProtoMember(1)] public uint Command { get; set; } // 1700
    
    [ProtoMember(2)] public int Seq { get; set; } // 0
    
    [ProtoMember(19)] public ApplyUploadReqV3 Upload { get; set; }
    
    [ProtoMember(101)] public int BusinessId { get; set; } // 3
    
    [ProtoMember(102)] public int ClientType { get; set; } // 1
    
    [ProtoMember(200)] public int FlagSupportMediaPlatform { get; set; } // 1
}

[ProtoContract]
internal class ApplyUploadReqV3
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