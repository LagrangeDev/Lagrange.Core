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

    internal Stream? ImageStream { get; set; }

    internal string? Path { get; set; }

    internal uint FileId { get; set; }

    internal MsgInfo? MsgInfo { get; set; }

    internal NotOnlineImage? CompatImage { get; set; }

    internal CustomFace? CompatFace { get; set; }

    public ImageEntity() { }

    public ImageEntity(string filePath)
    {
        FilePath = filePath;
        ImageStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public ImageEntity(byte[] file)
    {
        FilePath = "";
        ImageStream = new MemoryStream(file);
    }
    
    public ImageEntity(Stream stream)
    {
        FilePath = "";
        ImageStream = stream;
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
                    ImageUrl = $"{BaseUrl}{image.OrigUrl}"
                };

            }

            return new ImageEntity
            {
                PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                FilePath = image.FilePath,
                ImageSize = image.FileLen,
                ImageUrl = $"{LegacyBaseUrl}{image.OrigUrl}"
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
                    ImageUrl = $"{BaseUrl}{face.OrigUrl}"
                };

            }

            return new ImageEntity
            {
                PictureSize = new Vector2(face.Width, face.Height),
                FilePath = face.FilePath,
                ImageSize = face.Size,
                ImageUrl = $"{LegacyBaseUrl}{face.OrigUrl}"
            };
        }

        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";

    public string ToPreviewText() => "[图片]";
}
