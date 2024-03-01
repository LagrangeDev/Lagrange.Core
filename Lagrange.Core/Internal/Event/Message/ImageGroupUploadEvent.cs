using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageGroupUploadEvent : ProtocolEvent
{
    public ImageEntity Entity { get; }
    
    public uint GroupUin { get; }
    
    public string UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public CustomFace Compat { get; }

    private ImageGroupUploadEvent(ImageEntity entity, uint groupUin) : base(true)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private ImageGroupUploadEvent(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, CustomFace compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }

    public static ImageGroupUploadEvent Create(ImageEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static ImageGroupUploadEvent Result(int resultCode, string uKey, MsgInfo msgInfo, List<IPv4> network, CustomFace compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}