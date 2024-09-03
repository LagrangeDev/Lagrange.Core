using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageUploadEvent : NTV2RichMediaUploadEvent
{
    public ImageEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public NotOnlineImage Compat { get; }

    private ImageUploadEvent(ImageEntity entity, string targetUid)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private ImageUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, NotOnlineImage compat) 
        : base(resultCode, msgInfo, uKey, network, subFiles)
    {
        Compat = compat;
    }
    
    public static ImageUploadEvent Create(ImageEntity entity, string targetUid)
        => new(entity, targetUid);

    public static ImageUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, NotOnlineImage compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}