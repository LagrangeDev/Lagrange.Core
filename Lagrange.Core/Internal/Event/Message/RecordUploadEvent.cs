using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordUploadEvent : ProtocolEvent
{
    public RecordEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public string UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public RichText Compat { get; }

    private RecordUploadEvent(RecordEntity entity, string targetUid) : base(true)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private RecordUploadEvent(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, RichText compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }
    
    public static RecordUploadEvent Create(RecordEntity entity, string targetUid)
        => new(entity, targetUid);

    public static RecordUploadEvent Result(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, RichText compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}