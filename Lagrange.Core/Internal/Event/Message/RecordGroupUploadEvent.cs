using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordGroupUploadEvent : NTV2RichMediaUploadEvent
{
    public RecordEntity Entity { get; }
    
    public uint GroupUin { get; }
    
    public RichText Compat { get; }

    private RecordGroupUploadEvent(RecordEntity entity, uint groupUin)
    {
        Entity = entity;
        GroupUin = groupUin;
    }

    private RecordGroupUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, RichText compat)
        : base(resultCode, msgInfo, uKey, network, subFiles)
    {
        Compat = compat;
    }

    public static RecordGroupUploadEvent Create(RecordEntity entity, uint groupUin)
        => new(entity, groupUin);

    public static RecordGroupUploadEvent Result(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles, RichText compat)
        => new(resultCode, msgInfo, uKey, network, subFiles, compat);
}