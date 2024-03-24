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

    internal int? PicSubType { get; set; }

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

    /*
    Currently, After some basic testing,
    it appears that the second digit of "PbReserve" might represent the type of image.
    0 for regular images (though there are exceptions, it seems that emoticons sent by models are also 0)
    1 for favorite emoticons
    >1 for emoticons from other sources (such as system recommendations)
    For more, see ref below.
    Note I:
    As @Linwenxuan05 says, "PbReserve" is a piece of NTQQ's shit.
    Note II:
    After some testing, it has been observed that "PbReserve" is present only in all image transmissions (maybe) from
    NTQQ, and (maybe) legacy PCQQ emoticon transmissions. When sent from other clients, this field may be null,
    and "OldData" instead.
    Inspiration and field reference: https://docs.go-cqhttp.org/cqcode/#%E5%9B%BE%E7%89%87
    */
    private static int GetImageTypeFromPbReserve(CustomFace face)
    {
        // maybe NTQQ and legacy PCQQ emoticon
        if (face.PbReserve is not null && face.PbReserve.Length >= 2) return face.PbReserve[1];
        // maybe other legacy PCQQ
        else if (face.OldData is not null && face.OldData.Length >= 4)
        {
            switch (face.OldData[4].ToString("X2"))
            {
                case "32":
                    return 0;
                case "36":
                    return 1;
                // Anohana: The value We Saw That Day
                default:
                    return 114514;
            }
        }
        // still don't know
        else return -1;
    }


    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        //As @Linwenxuan05 says, "NotOnlineImage" only in private message
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
                    PicSubType = -1
                };
            }

            return new ImageEntity
            {
                PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                FilePath = image.FilePath,
                ImageSize = image.FileLen,
                ImageUrl = $"{LegacyBaseUrl}{image.OrigUrl}",
                PicSubType = -1
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
                    PicSubType = GetImageTypeFromPbReserve(face)
                };
            }

            return new ImageEntity
            {
                PictureSize = new Vector2(face.Width, face.Height),
                FilePath = face.FilePath,
                ImageSize = face.Size,
                ImageUrl = $"{LegacyBaseUrl}{face.OrigUrl}",
                PicSubType = GetImageTypeFromPbReserve(face)
            };
        }

        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";
}