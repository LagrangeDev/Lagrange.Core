using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Rename Group Member
/// </summary>
[ProtoContract]
internal class OidbSvcTrpcTcp0x8FC
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x8FCBody Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x8FCBody
{
    [ProtoMember(1)] public string TargetUid { get; set; }
    
    [ProtoMember(5)] public string SpecialTitle { get; set; }
    
    [ProtoMember(6)] public int SpecialTitleExpireTime { get; set; }
    
    [ProtoMember(7)] public string UinName { get; set; }
    
    [ProtoMember(8)] public string TargetName { get; set; }
}