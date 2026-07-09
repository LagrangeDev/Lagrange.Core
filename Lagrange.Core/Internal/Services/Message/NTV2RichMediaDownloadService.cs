using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Message;

file static class Common
{
    public static string ParseUrl(this NTV2RichMediaResp resp) => $"https://{resp.Download.Info.Domain}{resp.Download.Info.UrlPath}{resp.Download.RKeyParam}";
}

[Service("OidbSvcTrpcTcp.0x11c5_200")]
[EventSubscribe<ImageDownloadEventReq>(Protocols.All)]
internal class ImageDownloadService : OidbService<ImageDownloadEventReq, ImageDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11c5;

    protected override uint Service => 200;

    protected override Task<NTV2RichMediaReq> ProcessRequest(ImageDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<ImageDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new ImageDownloadEventResp(response.ParseUrl()));
    }
}

[Service("OidbSvcTrpcTcp.0x11c4_200")]
[EventSubscribe<ImageGroupDownloadEventReq>(Protocols.All)]
internal class ImageGroupDownloadService : OidbService<ImageGroupDownloadEventReq, ImageGroupDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11c4;

    protected override uint Service => 200;

    protected override Task<NTV2RichMediaReq> ProcessRequest(ImageGroupDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<ImageGroupDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new ImageGroupDownloadEventResp(response.ParseUrl()));
    }
}

[Service("OidbSvcTrpcTcp.0x126d_200")]
[EventSubscribe<RecordDownloadEventReq>(Protocols.All)]
internal class RecordDownloadService : OidbService<RecordDownloadEventReq, RecordDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x126d;
    
    protected override uint Service => 200;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(RecordDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<RecordDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new RecordDownloadEventResp(response.ParseUrl()));
    }
}

[Service("OidbSvcTrpcTcp.0x126e_200")]
[EventSubscribe<RecordGroupDownloadEventReq>(Protocols.All)]
internal class RecordGroupDownloadService : OidbService<RecordGroupDownloadEventReq, RecordGroupDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x126e;
    
    protected override uint Service => 200;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(RecordGroupDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<RecordGroupDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new RecordGroupDownloadEventResp(response.ParseUrl()));
    }
}

[Service("OidbSvcTrpcTcp.0x11e9_200")]
[EventSubscribe<VideoDownloadEventReq>(Protocols.All)]
internal class VideoDownloadService : OidbService<VideoDownloadEventReq, VideoDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11e9;
    
    protected override uint Service => 200;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(VideoDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<VideoDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new VideoDownloadEventResp(response.ParseUrl()));
    }
}

[Service("OidbSvcTrpcTcp.0x11ea_200")]
[EventSubscribe<VideoGroupDownloadEventReq>(Protocols.All)]
internal class VideoGroupDownloadService : OidbService<VideoGroupDownloadEventReq, VideoGroupDownloadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11ea;
    
    protected override uint Service => 200;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(VideoGroupDownloadEventReq request, BotContext context)
    {
        return Task.FromResult(NTV2RichMedia.BuildDownloadReq(request.Message, request.Entity, new DownloadExt()));
    }

    protected override Task<VideoGroupDownloadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new VideoGroupDownloadEventResp(response.ParseUrl()));
    }
}