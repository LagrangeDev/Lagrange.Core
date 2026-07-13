using Lagrange.Proto;
using Lagrange.Proto.Serialization;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D5CFReqBody
{
    [ProtoMember(1)] public uint Version { get; set; }
    [ProtoMember(2, NumberHandling = ProtoNumberHandling.Fixed32)] public uint Sequence { get; set; }
    [ProtoMember(3)] public uint Type { get; set; }
    [ProtoMember(4)] public string SelfUid { get; set; }
    [ProtoMember(5)] public uint StartIndex { get; set; }
    [ProtoMember(6)] public uint RequestCount { get; set; }
    [ProtoMember(7)] public string? Reserve { get; set; }
    [ProtoMember(8)] public uint GetFlag { get; set; }
    [ProtoMember(9)] public uint StartTime { get; set; }
    [ProtoMember(10)] public uint ClearTime { get; set; }
    [ProtoMember(11)] public uint NeedUnreadUndecidedCount { get; set; }
    [ProtoMember(12)] public uint NeedCommonFriend { get; set; }
    [ProtoMember(13)] public uint NeedUnreadNoAgreeCount { get; set; }
    [ProtoMember(14)] public uint NeedGroupUin { get; set; }
    [ProtoMember(15)] public D5CFDelMsgInfo? DelMsgInfo { get; set; }
    [ProtoMember(16)] public D5CFClearMsgInfo? ClearMsgInfo { get; set; }
    [ProtoMember(22)] public uint Field22 { get; set; }
}

[ProtoPackable]
internal partial class D5CFRspBody
{
    [ProtoMember(1)] public uint Version { get; set; }
    [ProtoMember(2)] public uint Result { get; set; }
    [ProtoMember(3)] public D5CFSuccessRead? SuccessRead { get; set; }
    [ProtoMember(4)] public D5CFFailedSystem? FailedSystem { get; set; }
    [ProtoMember(5)] public string? Reserve { get; set; }
}

[ProtoPackable]
internal partial class D5CFSuccessRead
{
    [ProtoMember(1, NumberHandling = ProtoNumberHandling.Fixed32)] public uint Sequence { get; set; }
    [ProtoMember(2)] public uint Over { get; set; }
    [ProtoMember(3)] public uint Total { get; set; }
    [ProtoMember(4)] public List<D5CFUndecided>? Undecided { get; set; }
    [ProtoMember(5)] public List<D5CFDecided>? Decided { get; set; }
    [ProtoMember(6)] public uint UnreadCount { get; set; }
    [ProtoMember(7)] public List<D5CFAll>? All { get; set; }
    [ProtoMember(8)] public ulong UpdateTime { get; set; }
    [ProtoMember(9)] public uint UnreadCount2 { get; set; }
    [ProtoMember(10)] public uint FirstUpdate { get; set; }
    [ProtoMember(11)] public uint UnreadUndecidedCount { get; set; }
    [ProtoMember(12)] public List<ulong>? UnreadUndecidedUins { get; set; }
}

[ProtoPackable]
internal partial class D5CFAll
{
    [ProtoMember(1)] public string SelfUid { get; set; }
    [ProtoMember(2)] public string FriendUid { get; set; }
    [ProtoMember(3)] public uint State { get; set; }
    [ProtoMember(4)] public uint Time { get; set; }
    [ProtoMember(5)] public string? Comment { get; set; }
    [ProtoMember(6)] public string? AddSource { get; set; }
    [ProtoMember(7)] public uint SourceId { get; set; }
    [ProtoMember(8)] public uint SourceSubId { get; set; }
    [ProtoMember(20)] public bool IsInitiator { get; set; }
}

[ProtoPackable]
internal partial class D5CFUndecided
{
    [ProtoMember(1)] public ulong Uin { get; set; }
    [ProtoMember(2)] public ulong FriendUin { get; set; }
    [ProtoMember(3)] public uint Flag { get; set; }
    [ProtoMember(4)] public uint AcknowledgeFlag { get; set; }
    [ProtoMember(5)] public uint RequestType { get; set; }
    [ProtoMember(6)] public uint GroupId { get; set; }
    [ProtoMember(7)] public uint SourceId { get; set; }
    [ProtoMember(8)] public uint SourceSubId { get; set; }
    [ProtoMember(9)] public uint Time { get; set; }
    [ProtoMember(10)] public uint Count { get; set; }
    [ProtoMember(11)] public List<string>? Wording { get; set; }
    [ProtoMember(12)] public string MainEmailAccount { get; set; }
    [ProtoMember(13)] public string AddSource { get; set; }
    [ProtoMember(14)] public ulong GroupCode { get; set; }
}

[ProtoPackable]
internal partial class D5CFDecided
{
    [ProtoMember(1)] public ulong Uin { get; set; }
    [ProtoMember(2)] public ulong FriendUin { get; set; }
    [ProtoMember(3)] public uint RequestType { get; set; }
    [ProtoMember(4)] public uint Time { get; set; }
    [ProtoMember(5)] public List<string>? Wording { get; set; }
    [ProtoMember(6)] public string AddSource { get; set; }
    [ProtoMember(7)] public uint SourceId { get; set; }
    [ProtoMember(8)] public uint SourceSubId { get; set; }
    [ProtoMember(9)] public ulong GroupCode { get; set; }
}

[ProtoPackable]
internal partial class D5CFDelMsgInfo
{
    [ProtoMember(1)] public List<D5CFDelDecidedInfo>? Decided { get; set; }
    [ProtoMember(2)] public List<D5CFDelUndecidedInfo>? Undecided { get; set; }
    [ProtoMember(3)] public uint Platform { get; set; }
}

[ProtoPackable]
internal partial class D5CFDelDecidedInfo
{
    [ProtoMember(1)] public ulong Uin { get; set; }
    [ProtoMember(2)] public uint Time { get; set; }
    [ProtoMember(3)] public uint RequestType { get; set; }
}

[ProtoPackable]
internal partial class D5CFDelUndecidedInfo
{
    [ProtoMember(1)] public ulong Uin { get; set; }
}

[ProtoPackable]
internal partial class D5CFClearMsgInfo
{
    [ProtoMember(1)] public uint Platform { get; set; }
}

[ProtoPackable]
internal partial class D5CFFailedSystem
{
    [ProtoMember(1)] public string Message { get; set; }
}
