using System.Numerics;
using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(NotOnlineImage))]
[MessageElement(typeof(CustomFace))]
public class ImageEntity : IMessageEntity
{
    private const string BaseUrl = "https://multimedia.nt.qq.com.cn";

    private const string LegacyBaseUrl = "http://gchat.qpic.cn";

    public Vector2 PictureSize { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public uint ImageSize { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    internal Lazy<Stream>? ImageStream { get; set; }

    internal string? Path { get; set; }

    internal uint FileId { get; set; }

    internal MsgInfo? MsgInfo { get; set; }

    internal NotOnlineImage? CompatImage { get; set; }

    internal CustomFace? CompatFace { get; set; }

    internal string? Summary { get; set; }
    
    internal int SubType { get; set; }

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
        if (elems.NotOnlineImage is { } image)
        {
            if (image.OrigUrl.Contains("&fileid=")) // NTQQ's shit
            {
                return new ImageEntity // NTQQ Mobile
                {
                    PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                    FilePath = image.FilePath,
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
        if (face.OldData.Length < 5)
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
