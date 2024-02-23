using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class Ptt
{
    [ProtoMember(1)] public int FileType { get; set; }
    
    [ProtoMember(2)] public long SrcUin { get; set; }
    
    [ProtoMember(3)] public string FileUuid { get; set; }
    
    [ProtoMember(4)] public byte[] FileMd5 { get; set; }
    
    [ProtoMember(5)] public string FileName { get; set; }
    
    [ProtoMember(6)] public int FileSize { get; set; }
    
    [ProtoMember(7)] public byte[] Reserve { get; set; }
    
    [ProtoMember(8)] public int FileId { get; set; }
    
    [ProtoMember(9)] public int ServerIp { get; set; }
    
    [ProtoMember(10)] public int ServerPort { get; set; }
    
    [ProtoMember(11)] public bool BoolValid { get; set; }
    
    [ProtoMember(12)] public byte[] Signature { get; set; }
    
    [ProtoMember(13)] public byte[] Shortcut { get; set; }
    
    [ProtoMember(14)] public byte[] FileKey { get; set; }
    
    [ProtoMember(15)] public int MagicPttIndex { get; set; }
    
    [ProtoMember(16)] public int VoiceSwitch { get; set; }
    
    [ProtoMember(17)] public byte[] PttUrl { get; set; }
    
    [ProtoMember(18)] public string GroupFileKey { get; set; }
    
    [ProtoMember(19)] public int Time { get; set; }
    
    [ProtoMember(20)] public byte[] DownPara { get; set; }
    
    [ProtoMember(29)] public int Format { get; set; }
    
    [ProtoMember(30)] public byte[] PbReserve { get; set; }
    
    [ProtoMember(31)] public List<byte[]> BytesPttUrls { get; set; }
    
    [ProtoMember(32)] public int DownloadFlag { get; set; }
}