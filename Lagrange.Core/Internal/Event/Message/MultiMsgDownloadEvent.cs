using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class MultiMsgDownloadEvent : ProtocolEvent
{
    public string? Uid { get; }
    
    public string? ResId { get; }
    
    public List<MessageChain>? Chains { get; }

    private MultiMsgDownloadEvent(string uid, string resId) : base(true)
    {
        Uid = uid;
        ResId = resId;
    }
    
    private MultiMsgDownloadEvent(int resultCode, List<MessageChain> chains) : base(resultCode)
    {
        Chains = chains;
    }
    
    public static MultiMsgDownloadEvent Create(string uid, string resId) => new(uid, resId);
    
    public static MultiMsgDownloadEvent Result(int resultCode, List<MessageChain> chains) => new(resultCode, chains);
}