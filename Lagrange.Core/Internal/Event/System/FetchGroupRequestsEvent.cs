namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchGroupRequestsEvent : ProtocolEvent
{
    public List<RawEvent> Events { get; }
    
    private FetchGroupRequestsEvent() : base(true) { }

    private FetchGroupRequestsEvent(int resultCode, List<RawEvent> events) : base(resultCode)
    {
        Events = events;
    }

    public static FetchGroupRequestsEvent Create() => new();
    
    public static FetchGroupRequestsEvent Result(int resultCode, List<RawEvent> events) => new(resultCode, events);

    public record RawEvent(
        uint GroupUin,
        string? InvitorMemberUid,
        string? InvitorMemberCard,
        string TargetMemberUid,
        string TargetMemberCard,
        string? OperatorUid,
        string? OperatorName,
        ulong Sequence,
        uint State,
        uint EventType,
        string? Comment
    );
}