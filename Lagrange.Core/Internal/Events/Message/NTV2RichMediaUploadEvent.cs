using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entities;

namespace Lagrange.Core.Internal.Events.Message;

internal class NTV2RichMediaUploadEventReq(BotMessage message, RichMediaEntityBase entity) : ProtocolEvent
{
    public BotMessage Message { get; } = message;

    public RichMediaEntityBase Entity { get; } = entity;
}

internal class NTV2RichMediaUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext) : ProtocolEvent
{
    public MsgInfo Info { get; } = info;
    
    public byte[] Compat { get; } = compat;
    
    public NTV2RichMediaHighwayExt? Ext { get; } = ext;
}

internal class ImageUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class ImageUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext)
    : NTV2RichMediaUploadEventResp(info, compat, ext);

internal class ImageGroupUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class ImageGroupUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext)
    : NTV2RichMediaUploadEventResp(info, compat, ext);

internal class RecordUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class RecordUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext)
    : NTV2RichMediaUploadEventResp(info, compat, ext);

internal class RecordGroupUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class RecordGroupUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext)
    : NTV2RichMediaUploadEventResp(info, compat, ext);

internal class VideoUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class VideoUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext, NTV2RichMediaHighwayExt? subExt)
    : NTV2RichMediaUploadEventResp(info, compat, ext)
{
    public NTV2RichMediaHighwayExt? SubExt { get; } = subExt;
}

internal class VideoGroupUploadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaUploadEventReq(message, entity);

internal class VideoGroupUploadEventResp(MsgInfo info, byte[] compat, NTV2RichMediaHighwayExt? ext, NTV2RichMediaHighwayExt? subExt)
    : NTV2RichMediaUploadEventResp(info, compat, ext)
{
    public NTV2RichMediaHighwayExt? SubExt { get; } = subExt;
}