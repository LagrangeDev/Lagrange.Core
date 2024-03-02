using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message;

/// <summary>
/// trpc.msg.msg_svc.MsgService.SsoReadedReport
/// </summary>
[ProtoContract]
internal class SsoReadedReport
{
    [ProtoMember(1)] public SsoReadedReportGroup? Group { get; set; }
    
    [ProtoMember(2)] public SsoReadedReportC2C? C2C { get; set; }
}

[ProtoContract]
internal class SsoReadedReportC2C
{
    [ProtoMember(2)] public string? TargetUid { get; set; }
    
    [ProtoMember(3)] public uint Time { get; set; }
    
    [ProtoMember(4)] public uint StartSequence { get; set; }
}

[ProtoContract]
internal class SsoReadedReportGroup
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint StartSequence { get; set; }
}