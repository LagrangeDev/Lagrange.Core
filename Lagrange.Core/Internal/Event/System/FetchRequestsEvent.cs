namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchRequestsEvent : ProtocolEvent
{
    public List<RawEvent> Events { get; }
    
    private FetchRequestsEvent() : base(true) { }

    private FetchRequestsEvent(int resultCode, List<RawEvent> events) : base(resultCode)
    {
        Events = events;
    }

    public static FetchRequestsEvent Create() => new();
    
    public static FetchRequestsEvent Result(int resultCode, List<RawEvent> events) => new(resultCode, events);

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
        uint EventType
    );
}