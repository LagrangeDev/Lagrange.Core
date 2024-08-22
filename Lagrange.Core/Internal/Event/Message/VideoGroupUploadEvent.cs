using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoGroupUploadEvent : NTV2RichMediaUploadEvent
{
    public VideoEntity Entity { get; }
    
    public uint GroupUin { get; set; }
    
    public VideoFile Compat { get; }
    
    private VideoGroupUploadEvent(VideoEntity entity, uint groupUin)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private VideoGroupUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, VideoFile compat) 
    : base(resultCode, msgInfo, uKey, network, subFiles)
    {
        Compat = compat;
    }
    
    public static VideoGroupUploadEvent Create(VideoEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static VideoGroupUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, VideoFile compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}