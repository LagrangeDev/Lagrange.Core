using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Message.Entities;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

file class Common
{
    private const int BlockSize = 1024 * 1024;

    public static NTV2RichMediaHighwayExt? GenerateExt(UploadResp upload)
    {
        if (upload.UKey == null) return null;
        
        var index = upload.MsgInfo.MsgInfoBody[0].Index;
        return new NTV2RichMediaHighwayExt
        {
            FileUuid = index.FileUuid,
            UKey = upload.UKey,
            Network = ConvertIPv4(upload.IPv4s),
            MsgInfoBody = upload.MsgInfo.MsgInfoBody,
            BlockSize = BlockSize,
            Hash = new NTHighwayHash { FileSha1 = [Convert.FromHexString(index.Info.FileSha1)] }
        };
    }
    
    public static NTV2RichMediaHighwayExt? GenerateExt(UploadResp upload, SubFileInfo subFileInfo)
    {
        if (subFileInfo.UKey == null) return null;
        
        var index = upload.MsgInfo.MsgInfoBody[0].Index;
        return new NTV2RichMediaHighwayExt
        {
            FileUuid = index.FileUuid,
            UKey = subFileInfo.UKey,
            Network = ConvertIPv4(subFileInfo.IPv4s),
            MsgInfoBody = upload.MsgInfo.MsgInfoBody,
            BlockSize = BlockSize,
            Hash = new NTHighwayHash { FileSha1 = [Convert.FromHexString(index.Info.FileSha1)] }
        };
    }

    private static NTHighwayNetwork ConvertIPv4(List<IPv4> i) => new()
    {
        IPv4s = i.Select(x => new NTHighwayIPv4
        {
            Domain = new NTHighwayDomain { IsEnable = true, IP = ProtocolHelper.UInt32ToIPV4Addr(x.OutIP) },
            Port = x.OutPort
        }).ToList()
    };
}

[EventSubscribe<ImageUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x11c5_100")]
internal class ImageUploadService : OidbService<ImageUploadEventReq, ImageUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11c5;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(ImageUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo
            {
                TextSummary = ((ImageEntity)request.Entity).Summary,
                BytesPbReserveC2c = [0x08, 0x00, 0x18, 0x00, 0x20, 0x00, 0x42, 0x00, 0x50, 0x00, 0x62, 0x00, 0x92, 0x01, 0x00, 0x9A, 0x01, 0x00, 0xA2, 0x01, 0x0C, 0x08, 0x00, 0x12, 0x00, 0x18, 0x00, 0x20, 0x00, 0x28, 0x00, 0x3A, 0x00]
            },
            Video = new VideoExtBizInfo { BytesPbReserve = [] },
            Ptt = new PttExtBizInfo { BytesReserve = [], BytesPbReserve = [], BytesGeneralFlags = [] }
        };
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext));
    }

    protected override Task<ImageUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new ImageUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload)));
    }
}

[EventSubscribe<ImageGroupUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x11c4_100")]
internal class ImageGroupUploadService : OidbService<ImageGroupUploadEventReq, ImageGroupUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11c4;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(ImageGroupUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo
            {
                TextSummary = ((ImageEntity)request.Entity).Summary,
                BytesPbReserveC2c = [0x08, 0x00, 0x18, 0x00, 0x20, 0x00, 0x4A, 0x00, 0x50, 0x00, 0x62, 0x00, 0x92, 0x01, 0x00, 0x9A, 0x01, 0x00, 0xAA, 0x01, 0x0C, 0x08, 0x00, 0x12, 0x00, 0x18, 0x00, 0x20, 0x00, 0x28, 0x00, 0x3A, 0x00]
            },
            Video = new VideoExtBizInfo { BytesPbReserve = [] },
            Ptt = new PttExtBizInfo { BytesReserve = [], BytesPbReserve = [], BytesGeneralFlags = [] }
        };
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext));
    }

    protected override Task<ImageGroupUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new ImageGroupUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload)));
    }
}

[EventSubscribe<RecordUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x126d_100")]
internal class RecordUploadService : OidbService<RecordUploadEventReq, RecordUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x126d;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(RecordUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo { TextSummary = "" },
            Video = new VideoExtBizInfo { BytesPbReserve = [] },
            Ptt = new PttExtBizInfo
            {
                BytesReserve = [0x08, 0x00, 0x38, 0x00],
                BytesPbReserve = [],
                BytesGeneralFlags = [0x9a, 0x01, 0x0b, 0xaa, 0x03, 0x08, 0x08, 0x04, 0x12, 0x04, 0x00, 0x00, 0x00, 0x00]
            }
        };
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext));
    }

    protected override Task<RecordUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new RecordUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload)));
    }
}

[EventSubscribe<RecordGroupUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x126e_100")]
internal class RecordGroupUploadService : OidbService<RecordGroupUploadEventReq, RecordGroupUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x126e;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(RecordGroupUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo { TextSummary = "" },
            Video = new VideoExtBizInfo { BytesPbReserve = [] },
            Ptt = new PttExtBizInfo
            {
                BytesReserve = [],
                BytesPbReserve = [0x08, 0x00, 0x38, 0x00],
                BytesGeneralFlags = [0x9a, 0x01, 0x07, 0xaa, 0x03, 0x04, 0x08, 0x08, 0x12, 0x00]
            }
        };
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext));
    }

    protected override Task<RecordGroupUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new RecordGroupUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload)));
    }
}

[EventSubscribe<VideoUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x11e9_100")]
internal class VideoUploadService : OidbService<VideoUploadEventReq, VideoUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11e9;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(VideoUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo { BizType = 0, TextSummary = "" },
            Video = new VideoExtBizInfo { BytesPbReserve = [0x80, 0x01, 0x00] },
            Ptt = new PttExtBizInfo { BytesReserve = [], BytesPbReserve = [], BytesGeneralFlags = [] }
        };
        var entity = (VideoEntity)request.Entity;
        ArgumentNullException.ThrowIfNull(entity.ThumbnailEntity);
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext, (100, entity.ThumbnailEntity)));
    }

    protected override Task<VideoUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new VideoUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload), Common.GenerateExt(response.Upload, response.Upload.SubFileInfos[0])));
    }
}

[EventSubscribe<VideoGroupUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x11ea_100")]
internal class VideoGroupUploadService : OidbService<VideoGroupUploadEventReq, VideoGroupUploadEventResp, NTV2RichMediaReq, NTV2RichMediaResp>
{
    protected override uint Command => 0x11ea;

    protected override uint Service => 100;
    
    protected override Task<NTV2RichMediaReq> ProcessRequest(VideoGroupUploadEventReq request, BotContext context)
    {
        var ext = new ExtBizInfo
        {
            Pic = new PicExtBizInfo { BizType = 0, TextSummary = "" },
            Video = new VideoExtBizInfo { BytesPbReserve = [0x80, 0x01, 0x00] },
            Ptt = new PttExtBizInfo { BytesReserve = [], BytesPbReserve = [], BytesGeneralFlags = [] }
        };
        var entity = (VideoEntity)request.Entity;
        ArgumentNullException.ThrowIfNull(entity.ThumbnailEntity);
        return Task.FromResult(NTV2RichMedia.BuildUploadReq(request.Message, request.Entity, ext, (100, entity.ThumbnailEntity)));
    }

    protected override Task<VideoGroupUploadEventResp> ProcessResponse(NTV2RichMediaResp response, BotContext context)
    {
        return Task.FromResult(new VideoGroupUploadEventResp(response.Upload.MsgInfo, response.Upload.CompatQMsg, Common.GenerateExt(response.Upload), Common.GenerateExt(response.Upload, response.Upload.SubFileInfos[0])));
    }
}