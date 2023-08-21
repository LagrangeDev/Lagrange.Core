using Lagrange.Core.Message;

namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class MultiMsgUploadEvent : ProtocolEvent
{
    public string? Uid { get; }
    
    public string? ResId { get; }
    
    public List<MessageChain>? Chains { get; }

    private MultiMsgUploadEvent(int resultCode, string resId) : base(resultCode)
    {
        ResId = resId;
    }
    
    private MultiMsgUploadEvent(string uid, List<MessageChain> chains) : base(true)
    {
        Uid = uid;
        Chains = chains;
    }
    
    public MultiMsgUploadEvent Create(string uid, List<MessageChain> chains) => new(uid, chains);
    
    public MultiMsgUploadEvent Result(int resultCode, string resId) => new(resultCode, resId);
}