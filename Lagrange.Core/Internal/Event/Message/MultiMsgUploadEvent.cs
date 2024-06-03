using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class MultiMsgUploadEvent : ProtocolEvent
{
    public uint? GroupUin { get; }
    
    public string? ResId { get; }
    
    public List<MessageChain>? Chains { get; }

    private MultiMsgUploadEvent(int resultCode, string resId) : base(resultCode)
    {
        ResId = resId;
    }
    
    private MultiMsgUploadEvent(uint? groupUin, List<MessageChain> chains) : base(true)
    {
        GroupUin = groupUin;
        Chains = chains;
    }
    
    public static MultiMsgUploadEvent Create(uint? groupUin, List<MessageChain> chains) => new(groupUin, chains);
    
    public static MultiMsgUploadEvent Result(int resultCode, string resId) => new(resultCode, resId);
}