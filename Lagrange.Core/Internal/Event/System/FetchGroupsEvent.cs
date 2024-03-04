using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchGroupsEvent : ProtocolEvent
{
    public List<BotGroup> Groups { get; }

    private FetchGroupsEvent() : base(true) { }
    
    private FetchGroupsEvent(List<BotGroup> groups) : base(0)
    {
        Groups = groups;
    }
    
    public static FetchGroupsEvent Create() => new();
    
    public static FetchGroupsEvent Result(List<BotGroup> groups) => new(groups);
}