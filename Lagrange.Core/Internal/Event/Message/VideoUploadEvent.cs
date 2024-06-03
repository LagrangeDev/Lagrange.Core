using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoUploadEvent : ProtocolEvent
{
    public VideoEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public string? UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public VideoFile Compat { get; }

    private VideoUploadEvent(VideoEntity entity, string targetUid) : base(true)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private VideoUploadEvent(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, VideoFile compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }
    
    public static VideoUploadEvent Create(VideoEntity entity, string targetUid)
        => new(entity, targetUid);

    public static VideoUploadEvent Result(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, VideoFile compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}