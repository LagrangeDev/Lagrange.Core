using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

/// <summary>
/// Response for LongConn.OffPicUp
/// </summary>
[ProtoContract]
internal class OffPicUpResponse
{
    [ProtoMember(1)] public uint FileId { get; set; }
    
    [ProtoMember(2)] public uint ClientIp { get; set; }
    
    [ProtoMember(3)] public uint Result { get; set; }
    
    [ProtoMember(4)] public byte[]? FailMsg { get; set; }
    
    [ProtoMember(5)] public bool FileExit { get; set; }
    
    [ProtoMember(7)] public List<uint>? UpIp { get; set; }
    
    [ProtoMember(8)] public List<uint>? UpPort { get; set; }
    
    [ProtoMember(9)] public byte[]? UpUkey { get; set; }
    
    [ProtoMember(10)] public string? UpResid { get; set; }
    
    [ProtoMember(11)] public string? UpUuid { get; set; }
    
    [ProtoMember(12)] public uint UpOffset { get; set; }
    
    [ProtoMember(13)] public ulong BlockSize { get; set; }
    
    /*[ProtoMember(14)] public uint Roamdays { get; set; }
    
    [ProtoMember(15)] public byte[]? ClientIp6 { get; set; }
    
    [ProtoMember(16)] public byte[]? ThumbDownPara { get; set; }
    
    [ProtoMember(17)] public byte[]? OriginalDownPara { get; set; }
    
    [ProtoMember(18)] public byte[]? DownDomain { get; set; }
    
    [ProtoMember(19)] public byte[]? BigDownPara { get; set; }
    
    [ProtoMember(20)] public byte[]? BigThumbDownPara { get; set; }
    
    [ProtoMember(21)] public uint HttpsUrlFlag { get; set; }
    
    [ProtoMember(22)] public byte[]? Info4Busi { get; set; }*/
}