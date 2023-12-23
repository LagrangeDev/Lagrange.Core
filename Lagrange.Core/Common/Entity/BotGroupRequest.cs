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
        ulong sequence)
    {
        GroupUin = groupUin;
        InvitorMemberUin = invitorMemberUin;
        InvitorMemberCard = invitorMemberCard;
        TargetMemberUin = targetMemberUin;
        TargetMemberCard = targetMemberCard;
        OperatorUin = operatorUin;
        OperatorName = operatorName;
        State = state;
        Sequence = sequence;
    }

    public uint GroupUin { get; set; }
    
    public uint? InvitorMemberUin { get; set; }
    
    public string? InvitorMemberCard { get; set; }
    
    public uint TargetMemberUin { get; set; }
    
    public string TargetMemberCard { get; set; }
    
    public uint? OperatorUin { get; set; }
    
    public string? OperatorName { get; set; }
    
    public uint State { get; set; }
    
    internal ulong Sequence { get; set; } // for internal use of Approving Requests
}