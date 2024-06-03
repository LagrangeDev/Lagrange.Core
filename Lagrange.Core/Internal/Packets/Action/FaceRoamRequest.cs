using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

[ProtoContract]
internal class FaceRoamRequest
{
    [ProtoMember(1)] public PlatInfo? Comm { get; set; }
    
    [ProtoMember(2)] public uint SelfUin { get; set; }
    
    [ProtoMember(3)] public uint SubCmd { get; set; }  // 1
    
    [ProtoMember(6)] public uint Field6 { get; set; }  // 1
}

[ProtoContract]
internal class PlatInfo
{
    [ProtoMember(1)] public uint ImPlat { get; set; }

    [ProtoMember(2)] public string? OsVersion { get; set; }
    
    [ProtoMember(3)] public string? QVersion { get; set; }
}