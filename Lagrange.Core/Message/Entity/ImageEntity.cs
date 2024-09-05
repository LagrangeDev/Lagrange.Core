using System.Numerics;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(NotOnlineImage))]
[MessageElement(typeof(CustomFace))]
[MessageElement(typeof(CommonElem))]
public class ImageEntity : IMessageEntity
{
    private const string BaseUrl = "https://multimedia.nt.qq.com.cn";

    private const string LegacyBaseUrl = "http://gchat.qpic.cn";

    public Vector2 PictureSize { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public byte[] ImageMd5 { get; set; } = Array.Empty<byte>();

    public uint ImageSize { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    internal Lazy<Stream>? ImageStream { get; set; }

    internal string? Path { get; set; }

    internal uint FileId { get; set; }

    internal MsgInfo? MsgInfo { get; set; }

    internal NotOnlineImage? CompatImage { get; set; }

    internal CustomFace? CompatFace { get; set; }

    internal string? Summary { get; set; }
    
    public int SubType { get; set; }

    public ImageEntity() { }

    public ImageEntity(string filePath)
    {
        FilePath = filePath;
        ImageStream = new Lazy<Stream>(() => new FileStream(filePath, FileMode.Open, FileAccess.Read));
    }

    public ImageEntity(byte[] file)
    {
        FilePath = "";
        ImageStream = new Lazy<Stream>(() => new MemoryStream(file));
    }

    public ImageEntity(Stream stream)
    {
        FilePath = "";
        ImageStream = new Lazy<Stream>(stream);
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var common = MsgInfo.Serialize();

        var elems = new Elem[]
        {
            new(),
            new()
            {
                CommonElem = new CommonElem
                {
                    ServiceType = 48,
                    PbElem = common.ToArray(),
                    BusinessType = 10,
                }
            }
        };

        if (CompatFace != null) elems[0].CustomFace = CompatFace;
        if (CompatImage != null) elems[0].NotOnlineImage = CompatImage;

        return elems;
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.CommonElem is { ServiceType: 48, BusinessType: 20 or 10 } common)
        {
            var extra = Serializer.Deserialize<MsgInfo>(common.PbElem.AsSpan());
            var index = extra.MsgInfoBody[0].Index;

            return new ImageEntity
            {
                PictureSize = new Vector2(index.Info.Width, index.Info.Height),
                FilePath = index.Info.FileName,
                ImageMd5 = index.Info.FileHash.UnHex(),
                ImageSize = index.Info.FileSize,
                MsgInfo = extra,
                SubType = (int)extra.ExtBizInfo.Pic.BizType,
            };
        }

        if (elems.NotOnlineImage is { } image)
        {
            if (image.OrigUrl.Contains("&fileid=")) // NTQQ's shit
            {
                return new ImageEntity // NTQQ Mobile
                {
                    PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                    FilePath = image.FilePath,
                    ImageMd5 = image.PicMd5,
                    ImageSize = image.FileLen,
                    ImageUrl = $"{BaseUrl}{image.OrigUrl}",
                    Summary = image.PbRes.Summary,
                    SubType = image.PbRes.SubType
                };
            }

            return new ImageEntity
            {
                PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                FilePath = image.FilePath,
                ImageMd5 = image.PicMd5,
                ImageSize = image.FileLen,
                ImageUrl = $"{LegacyBaseUrl}{image.OrigUrl}",
                Summary = image.PbRes.Summary,
                SubType = image.PbRes.SubType
            };
        }

        if (elems.CustomFace is { } face)
        {
            if (face.OrigUrl.Contains("&fileid="))
            {
                return new ImageEntity // NTQQ Mobile
                {
                    PictureSize = new Vector2(face.Width, face.Height),
                    FilePath = face.FilePath,
                    ImageSize = face.Size,
                    ImageUrl = $"{BaseUrl}{face.OrigUrl}",
                    Summary = face.PbReserve?.Summary,
                    SubType = face.PbReserve?.SubType ?? GetImageTypeFromFaceOldData(face)
                };

            }

            return new ImageEntity
            {
                PictureSize = new Vector2(face.Width, face.Height),
                FilePath = face.FilePath,
                ImageSize = face.Size,
                ImageUrl = $"{LegacyBaseUrl}{face.OrigUrl}",
                Summary = face.PbReserve?.Summary,
                SubType = face.PbReserve?.SubType ?? GetImageTypeFromFaceOldData(face)
            };
        }

        return null;
    }

    private static int GetImageTypeFromFaceOldData(CustomFace face)
    {
        if (face.OldData == null || face.OldData.Length < 5)
        {
            return 0;
        }
        // maybe legacy PCQQ(TIM)
        return face.OldData[4].ToString("X2") switch
        {
            "36" => 1,
            _ => 0,
        };
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {ToPreviewText()} {FilePath} {ImageSize} {ImageUrl}";

    public string ToPreviewText() => string.IsNullOrEmpty(Summary)
        ? SubType switch
        {
            1 => "[动画表情]",
            _ => "[图片]",
        }
        : Summary;
}
