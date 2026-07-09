using Lagrange.Core.Events;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entities;

namespace Lagrange.Core.Internal.Events.Message;

internal class NTV2RichMediaDownloadEventReq(BotMessage message, RichMediaEntityBase entity) : ProtocolEvent
{
    public BotMessage Message { get; } = message;
    
    public RichMediaEntityBase Entity { get; } = entity;
}

internal class NTV2RichMediaDownloadEventResp(string url) : ProtocolEvent
{
    public string Url { get; } = url;
}

internal class ImageDownloadEventReq(BotMessage message, RichMediaEntityBase entity) 
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class ImageDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);

internal class VideoDownloadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class VideoDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);

internal class RecordDownloadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class RecordDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);

internal class ImageGroupDownloadEventReq(BotMessage message, RichMediaEntityBase entity) 
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class ImageGroupDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);

internal class VideoGroupDownloadEventReq(BotMessage message, RichMediaEntityBase entity) 
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class VideoGroupDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);

internal class RecordGroupDownloadEventReq(BotMessage message, RichMediaEntityBase entity)
    : NTV2RichMediaDownloadEventReq(message, entity);

internal class RecordGroupDownloadEventResp(string url) : NTV2RichMediaDownloadEventResp(url);