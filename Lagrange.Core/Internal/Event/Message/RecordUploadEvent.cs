using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordUploadEvent : NTV2RichMediaUploadEvent
{
    public RecordEntity Entity { get; }
    
    public string TargetUid { get; set; }
    
    public RichText Compat { get; }

    private RecordUploadEvent(RecordEntity entity, string targetUid)
    {
        Entity = entity;
        TargetUid = targetUid;
    }

    private RecordUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, RichText compat) 
        : base(resultCode, msgInfo, uKey, network, subFiles)
    {
        Compat = compat;
    }
    
    public static RecordUploadEvent Create(RecordEntity entity, string targetUid)
        => new(entity, targetUid);

    public static RecordUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, RichText compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}