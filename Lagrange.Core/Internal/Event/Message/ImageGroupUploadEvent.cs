using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageGroupUploadEvent : NTV2RichMediaUploadEvent
{
    public ImageEntity Entity { get; }
    
    public uint GroupUin { get; }
    
    public CustomFace Compat { get; }

    private ImageGroupUploadEvent(ImageEntity entity, uint groupUin)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private ImageGroupUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, CustomFace compat) 
        : base(resultCode, msgInfo, uKey, network, subFiles)
    {
        Compat = compat;
    }

    public static ImageGroupUploadEvent Create(ImageEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static ImageGroupUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, CustomFace compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}