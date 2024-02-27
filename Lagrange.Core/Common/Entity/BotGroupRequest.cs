namespace Lagrange.Core.Common.Entity;

public class BotGroupRequest
{
    internal BotGroupRequest(
        uint groupUin, 
        uint? invitorMemberUin, 
        string? invitorMemberCard, 
        uint targetMemberUin,
        string targetMemberCard, 
        uint? operatorUin,
        string? operatorName, 
        uint state,
        ulong sequence,
        uint type,
        string? comment)
    {
        GroupUin = groupUin;
        InvitorMemberUin = invitorMemberUin;
        InvitorMemberCard = invitorMemberCard;
        TargetMemberUin = targetMemberUin;
        TargetMemberCard = targetMemberCard;
        OperatorUin = operatorUin;
        OperatorName = operatorName;
        EventState = (State)state;
        Sequence = sequence;
        EventType = (Type)type;
        Comment = comment;
    }

    public uint GroupUin { get; set; }
    
    public uint? InvitorMemberUin { get; set; }
    
    public string? InvitorMemberCard { get; set; }
    
    public uint TargetMemberUin { get; set; }
    
    public string TargetMemberCard { get; set; }
    
    public uint? OperatorUin { get; set; }
    
    public string? OperatorName { get; set; }
    
    public Type EventType { get; set; }
    
    public State EventState { get; set; }
    
    internal ulong Sequence { get; set; } // for internal use of Approving Requests
    
    public string? Comment { get; }

    public enum State
    {
        Default = 0,
        Pending = 1,
        Approved = 2,
        Disapproved = 3,
    }
    
    public enum Type
    {
        GroupRequest = 1,
        SelfInvitation = 2,
        ExitGroup = 13,
        GroupInvitation = 22,
    }
}