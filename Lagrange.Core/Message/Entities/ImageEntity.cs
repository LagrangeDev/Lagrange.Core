using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Message.Entities;

public class ImageEntity : RichMediaEntityBase
{
    private const string BaseUrl = "https://multimedia.nt.qq.com.cn";

    private const string LegacyBaseUrl = "https://gchat.qpic.cn";

    internal override Lazy<Stream>? Stream { get; }
    
    public Vector2 ImageSize { get; set; }
    
    public int SubType { get; init; }
    
    public string Summary { get; init; } = "[图片]";

    public ImageEntity() { }
    
    public ImageEntity(Stream stream, string? summary = "[图片]", int subType = 0, bool disposeOnCompletion = false)
    {
        Stream = new Lazy<Stream>(() => stream);
        Summary = summary ?? "[图片]";
        SubType = subType;
        DisposeOnCompletion = disposeOnCompletion;
    }
    
    public override async Task Preprocess(BotContext context, BotMessage message)
    { 
        ArgumentNullException.ThrowIfNull(Stream);

        try
        {
            IsGroup = message.IsGroup();
            NTV2RichMediaUploadEventResp result = IsGroup
                ? await context.EventContext.SendEvent<ImageGroupUploadEventResp>(new ImageGroupUploadEventReq(message, this))
                : await context.EventContext.SendEvent<ImageUploadEventResp>(new ImageUploadEventReq(message, this));

            _compat = result.Compat;
            MsgInfo = result.Info;

            if (result.Ext != null)
            {
                // Aot 和 MacOS 下使用 FlashTransfer 上传
                if (RuntimeFeature.IsDynamicCodeCompiled && !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    await context.HighwayContext.UploadFile(Stream.Value, IsGroup ? 1004 : 1003, ProtoHelper.Serialize(result.Ext));
                }
                else
                {
                    await context.FlashTransferContext.UploadFile(result.Ext.UKey, (uint)(IsGroup ? 1407 : 1406), Stream.Value);
                }
            }
        }
        finally
        {
            if (DisposeOnCompletion) await Stream.Value.DisposeAsync();
        }
    }

    public override async Task Postprocess(BotContext context, BotMessage message)
    {
        if (MsgInfo == null) return;

        NTV2RichMediaDownloadEventResp result = message.IsGroup()
            ? await context.EventContext.SendEvent<ImageGroupDownloadEventResp>(new ImageGroupDownloadEventReq(message, this))
            : await context.EventContext.SendEvent<ImageDownloadEventResp>(new ImageDownloadEventReq(message, this));
        
        FileUrl = result.Url;
    }

    public override string ToPreviewString() => Summary;
    internal override Elem[] Build()
    {
        if (_compat != null)
        {
            var compatElem = IsGroup
                ? new Elem { CustomFace = ProtoHelper.Deserialize<CustomFace>(_compat) }
                : new Elem { NotOnlineImage = ProtoHelper.Deserialize<NotOnlineImage>(_compat) };
            
            return
            [
                compatElem,
                new Elem()
                {
                    CommonElem = new CommonElem
                    {
                        ServiceType = 48,
                        PbElem = ProtoHelper.Serialize(MsgInfo ?? throw new ArgumentNullException(nameof(MsgInfo))),
                        BusinessType = IsGroup ? 20u : 10u,
                    }
                }
            ];
        }
        else
        {
            return
            [
                new Elem()
                {
                    CommonElem = new CommonElem
                    {
                        ServiceType = 48,
                        PbElem = ProtoHelper.Serialize(MsgInfo ?? throw new ArgumentNullException(nameof(MsgInfo))),
                        BusinessType = IsGroup ? 20u : 10u,
                    }
                }
            ];
        }
    }

    internal override IMessageEntity? Parse(List<Elem> elements, Elem target)
    {
        if (target.CommonElem is { BusinessType: 10 or 20 } commonElem)
        {
            var msgInfo = ProtoHelper.Deserialize<MsgInfo>(commonElem.PbElem.Span);
            var info = msgInfo.MsgInfoBody[0].Index.Info;
            
            return new ImageEntity
            {
                MsgInfo = msgInfo,
                ImageSize = new Vector2(info.Width, info.Height),
                SubType = (int)msgInfo.ExtBizInfo.Pic.BizType,
                Summary = string.IsNullOrEmpty(msgInfo.ExtBizInfo.Pic.TextSummary) ? "[图片]" : msgInfo.ExtBizInfo.Pic.TextSummary,
            };
        }
        
        // NTQQ sends the legacy element along with the CommonElem above, only clients before NTQQ send it alone
        if (elements.Any(x => x.CommonElem is { BusinessType: 10 or 20 })) return null;

        if (target.CustomFace is { OrigUrl.Length: > 0 } face)
        {
            return new ImageEntity
            {
                FileUrl = GetLegacyUrl(face.OrigUrl),
                FileName = face.FilePath,
                FileMd5 = Convert.ToHexString(face.Md5.AsSpan()),
                FileSize = face.Size,
                ImageSize = new Vector2(face.Width, face.Height),
                SubType = face.PbReserve?.SubType ?? 0,
                Summary = string.IsNullOrEmpty(face.PbReserve?.Summary) ? "[图片]" : face.PbReserve.Summary,
            };
        }

        if (target.NotOnlineImage is { OrigUrl.Length: > 0 } image)
        {
            var reserve = image.PbReserve is { Length: > 0 } ? ProtoHelper.Deserialize<NotOnlineImage.PbReserve1>(image.PbReserve) : null;
            return new ImageEntity
            {
                FileUrl = GetLegacyUrl(image.OrigUrl),
                FileName = Encoding.UTF8.GetString(image.FilePath.AsSpan()),
                FileMd5 = Convert.ToHexString(image.PicMd5.AsSpan()),
                FileSize = image.FileLen,
                ImageSize = new Vector2(image.PicWidth, image.PicHeight),
                SubType = reserve?.SubType ?? 0,
                Summary = string.IsNullOrEmpty(reserve?.Summary) ? "[图片]" : reserve.Summary,
            };
        }

        return null;
    }

    private static string GetLegacyUrl(string url) => (url.Contains("&fileid=") ? BaseUrl : LegacyBaseUrl) + url;
}