using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class GroupMute
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint SubType { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; }
    
    [ProtoMember(4)] public string? OperatorUid { get; set; }
    
    [ProtoMember(5)] public GroupMuteData Data { get; set; }
}

[ProtoContract]
internal class GroupMuteData
{
    [ProtoMember(1)] public uint Timestamp { get; set; }
    
    [ProtoMember(2)] public uint Type { get; set; }
    
    [ProtoMember(3)] public GroupMuteState State { get; set; }
}

[ProtoContract]
internal class GroupMuteState
{
    [ProtoMember(1)] public string? TargetUid { get; set; }
    
    [ProtoMember(2)] public uint Duration { get; set; } // uint.MaxValue = Mute else Lift
}