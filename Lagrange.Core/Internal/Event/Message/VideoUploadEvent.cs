using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoUploadEvent : NTV2RichMediaUploadEvent
{
    public VideoEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public VideoFile Compat { get; }

    private VideoUploadEvent(VideoEntity entity, string targetUid)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private VideoUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, VideoFile compat) 
        : base(resultCode, msgInfo, uKey, network, new List<SubFileInfo>())
    {
        Compat = compat;
    }
    
    public static VideoUploadEvent Create(VideoEntity entity, string targetUid)
        => new(entity, targetUid);

    public static VideoUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, VideoFile compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}