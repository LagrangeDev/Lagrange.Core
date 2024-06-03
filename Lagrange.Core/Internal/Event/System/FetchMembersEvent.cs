using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

internal class FetchMembersEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public List<BotGroupMember> Members { get; set; }
    
    public string? Token { get; }

    private FetchMembersEvent(uint groupUin, string? token) : base(true)
    {
        GroupUin = groupUin;
        Members = new List<BotGroupMember>();
        Token = token;
    }
    
    private FetchMembersEvent(List<BotGroupMember> members, string? token) : base(0)
    {
        Members = members;
        Token = token;
    }
    
    public static FetchMembersEvent Create(uint groupCode, string? token = null) => new(groupCode, token);
    
    public static FetchMembersEvent Result(List<BotGroupMember> members, string? token) => new(members, token);
}