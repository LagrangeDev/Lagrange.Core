using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class FaceRoamResponse
{
    [ProtoMember(1)] public uint RetCode { get; set; }
    
    [ProtoMember(2)] public string ErrMsg { get; set; }
    
    [ProtoMember(3)] public uint SubCmd { get; set; }

    [ProtoMember(4)] public FaceRoamUserInfo UserInfo { get; set; }
}

[ProtoContract]
internal class FaceRoamUserInfo
{
    [ProtoMember(1)] public List<string> FileName { get; set; }
    
    [ProtoMember(2)] public List<string> DeleteFile { get; set; }
    
    [ProtoMember(3)] public string Bid { get; set; }

    [ProtoMember(4)] public uint MaxRoamSize { get; set; }

    [ProtoMember(5)] public List<uint> EmojiType { get; set; }
}