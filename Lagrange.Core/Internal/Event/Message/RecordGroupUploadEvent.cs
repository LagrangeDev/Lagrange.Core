using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordGroupUploadEvent : ProtocolEvent
{
    public RecordEntity Entity { get; }
    
    public uint GroupUin { get; }
    
    public string UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public RichText Compat { get; }

    private RecordGroupUploadEvent(RecordEntity entity, uint groupUin) : base(true)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private RecordGroupUploadEvent(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, RichText compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }

    public static RecordGroupUploadEvent Create(RecordEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static RecordGroupUploadEvent Result(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, RichText compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}