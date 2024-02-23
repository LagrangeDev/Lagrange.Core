using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Common;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class NTV2RichMediaResp
{
    [ProtoMember(1)] public MultiMediaRespHead RespHead { get; set; }
    
    [ProtoMember(2)] public UploadResp Upload { get; set; }
    
    [ProtoMember(3)] public DownloadResp Download { get; set; }
    
    [ProtoMember(4)] public DownloadRKeyResp DownloadRKey { get; set; }
    
    [ProtoMember(5)] public DeleteResp Delete { get; set; }
    
    [ProtoMember(6)] public UploadCompletedResp UploadCompleted { get; set; }

    [ProtoMember(7)] public MsgInfoAuthResp MsgInfoAuth { get; set; }

    [ProtoMember(8)] public UploadKeyRenewalResp UploadKeyRenewal { get; set; }

    [ProtoMember(9)] public DownloadSafeResp DownloadSafe { get; set; }
    
    [ProtoMember(99)] public byte[]? Extension { get; set; }
}

[ProtoContract]
internal class MultiMediaRespHead
{
    [ProtoMember(1)] public CommonHead Common { get; set; }
    
    [ProtoMember(2)] public uint RetCode { get; set; }
    
    [ProtoMember(3)] public string Message { get; set; }
}

[ProtoContract]
internal class DownloadResp
{
    [ProtoMember(1)] public string RKeyParam { get; set; }
    
    [ProtoMember(2)] public uint RKeyTtlSecond { get; set; }

    [ProtoMember(3)] public DownloadInfo Info { get; set; }
    
    [ProtoMember(4)] public uint RKeyCreateTime { get; set; }
}

[ProtoContract]
internal class DownloadInfo
{
    [ProtoMember(1)] public string Domain { get; set; }
    
    [ProtoMember(2)] public string UrlPath { get; set; }
    
    [ProtoMember(3)] public uint HttpsPort { get; set; }

    [ProtoMember(4)] public List<IPv4> IPv4s { get; set; }
    
    [ProtoMember(5)] public List<IPv6> IPv6s { get; set; }
    
    [ProtoMember(6)] public PicUrlExtInfo PicUrlExtInfo { get; set; }
    
    [ProtoMember(7)] public VideoExtInfo VideoExtInfo { get; set; }
}

[ProtoContract]
internal class IPv4
{
    [ProtoMember(1)] public uint OutIP { get; set; }
    
    [ProtoMember(2)] public uint OutPort { get; set; }
    
    [ProtoMember(3)] public uint InIP { get; set; }
    
    [ProtoMember(4)] public uint InPort { get; set; }
    
    [ProtoMember(5)] public uint IPType { get; set; }
}

[ProtoContract]
internal class IPv6
{
    [ProtoMember(1)] public byte[] OutIP { get; set; }
    
    [ProtoMember(2)] public uint OutPort { get; set; }
    
    [ProtoMember(3)] public byte[] InIP { get; set; }
    
    [ProtoMember(4)] public uint InPort { get; set; }
    
    [ProtoMember(5)] public uint IPType { get; set; }
}

[ProtoContract]
internal class UploadResp
{
    [ProtoMember(1)] public string UKey { get; set; }
    
    [ProtoMember(2)] public uint UKeyTtlSecond { get; set; }

    [ProtoMember(3)] public List<IPv4> IPv4s { get; set; }
    
    [ProtoMember(4)] public List<IPv6> IPv6s { get; set; }
    
    [ProtoMember(5)] public ulong MsgSeq { get; set; }
    
    [ProtoMember(6)] public MsgInfo MsgInfo { get; set; }
    
    [ProtoMember(7)] public List<RichMediaStorageTransInfo> Ext { get; set; }
    
    [ProtoMember(8)] public byte[] CompatQMsg { get; set; }

    [ProtoMember(10)] public List<SubFileInfo> SubFileInfos { get; set; }
}

[ProtoContract]
internal class RichMediaStorageTransInfo
{
    [ProtoMember(1)] public uint SubType { get; set; }
    
    [ProtoMember(2)] public uint ExtType { get; set; }

    [ProtoMember(3)] public byte[] ExtValue { get; set; }
}

[ProtoContract]
internal class SubFileInfo
{
    [ProtoMember(1)] public uint SubType { get; set; }

    [ProtoMember(2)] public string UKey { get; set; }
    
    [ProtoMember(3)] public uint UKeyTtlSecond { get; set; }

    [ProtoMember(4)] public List<IPv4> IPv4s { get; set; }
    
    [ProtoMember(5)] public List<IPv6> IPv6s { get; set; }
}

[ProtoContract]
internal class DownloadSafeResp { }

[ProtoContract]
internal class UploadKeyRenewalResp
{
    [ProtoMember(1)] public string Ukey { get; set; }
    
    [ProtoMember(2)] public ulong UkeyTtlSec { get; set; }
}

[ProtoContract]
internal class MsgInfoAuthResp
{
    [ProtoMember(1)] public uint AuthCode { get; set; }
    
    [ProtoMember(2)] public byte[] Msg { get; set; }
    
    [ProtoMember(3)] public ulong ResultTime { get; set; }
}

[ProtoContract]
internal class UploadCompletedResp
{
    [ProtoMember(1)] public ulong MsgSeq { get; set; }
}

[ProtoContract]
internal class DeleteResp { }

[ProtoContract]
internal class DownloadRKeyResp
{
    [ProtoMember(1)] public List<RKeyInfo> RKeys { get; set; }
}

[ProtoContract]
internal class RKeyInfo
{
    [ProtoMember(1)] public string Rkey { get; set; }
    
    [ProtoMember(2)] public ulong RkeyTtlSec { get; set; }
    
    [ProtoMember(3)] public uint StoreId { get; set; }
    
    [ProtoMember(4)] public uint? RkeyCreateTime { get; set; }
    
    [ProtoMember(5)] public uint? Type { get; set; }
}