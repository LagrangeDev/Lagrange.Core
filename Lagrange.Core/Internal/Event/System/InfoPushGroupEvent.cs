using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;
#pragma warning disable CS8618

internal class InfoPushGroupEvent : ProtocolEvent
{
    public List<BotGroup> Groups { get; set; }
    
    private InfoPushGroupEvent() : base(false) { }
    
    private InfoPushGroupEvent(List<BotGroup> groups) : base(0)
    {
        Groups = groups;
    }
    
    public static InfoPushGroupEvent Result(List<BotGroup> groups) => new(groups);
}