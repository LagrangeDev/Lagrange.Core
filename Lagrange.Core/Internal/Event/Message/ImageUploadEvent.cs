using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageUploadEvent : ProtocolEvent
{
    public ImageEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public string? UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public NotOnlineImage Compat { get; }

    private ImageUploadEvent(ImageEntity entity, string targetUid) : base(true)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private ImageUploadEvent(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, NotOnlineImage compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }
    
    public static ImageUploadEvent Create(ImageEntity entity, string targetUid)
        => new(entity, targetUid);

    public static ImageUploadEvent Result(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, NotOnlineImage compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}