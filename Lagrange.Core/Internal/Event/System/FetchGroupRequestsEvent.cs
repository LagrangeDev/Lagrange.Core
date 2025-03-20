namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchGroupRequestsEvent : ProtocolEvent
{
    public List<RawEvent> Events { get; }

    public string? Message { get; }

    private FetchGroupRequestsEvent() : base(true) { }

    private FetchGroupRequestsEvent(int resultCode, string? message, List<RawEvent> events) : base(resultCode)
    {
        Message = message;
        Events = events;
    }

    public static FetchGroupRequestsEvent Create() => new();

    public static FetchGroupRequestsEvent Result(List<RawEvent> events) => new(0, null, events);

    public static FetchGroupRequestsEvent Result(int resultCode, string message) => new(resultCode, message, new List<RawEvent>());

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
        string? Comment,
        bool IsFiltered
    );
}