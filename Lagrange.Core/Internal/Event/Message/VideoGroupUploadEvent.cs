using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoGroupUploadEvent : ProtocolEvent
{
    public VideoEntity Entity { get; }
    
    public uint GroupUin { get; set; }
    
    public string? UKey { get; }
    
    public MsgInfo MsgInfo { get; }
    
    public List<IPv4> Network { get; }
    
    public VideoFile Compat { get; }
    
    private VideoGroupUploadEvent(VideoEntity entity, uint groupUin) : base(true)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private VideoGroupUploadEvent(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, VideoFile compat) : base(resultCode)
    {
        UKey = uKey;
        MsgInfo = msgInfo;
        Network = network;
        Compat = compat;
    }
    
    public static VideoGroupUploadEvent Create(VideoEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static VideoGroupUploadEvent Result(int resultCode, string? uKey, MsgInfo msgInfo, List<IPv4> network, VideoFile compat)
        => new(resultCode, uKey, msgInfo, network, compat);
}