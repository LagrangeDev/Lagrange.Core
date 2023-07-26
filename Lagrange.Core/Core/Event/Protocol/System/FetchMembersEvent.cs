using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Core.Event.Protocol.System;

internal class FetchMembersEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public List<BotGroupMember> Members { get; set; }

    private FetchMembersEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
        Members = new List<BotGroupMember>();
    }
    
    private FetchMembersEvent(List<BotGroupMember> members) : base(0)
    {
        Members = members;
    }
    
    public static FetchMembersEvent Create(uint groupCode) => new(groupCode);
    
    public static FetchMembersEvent Result(List<BotGroupMember> members) => new(members);
}