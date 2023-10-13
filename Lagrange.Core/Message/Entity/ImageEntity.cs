using System.Numerics;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using ImageExtra = Lagrange.Core.Internal.Packets.Message.Component.Extra.ImageExtra;

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
    
    public uint ImageSize { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    internal Stream? ImageStream { get; set; }

    internal string? Path { get; set; }
    
    internal uint FileId { get; set; }
    
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
        if (ImageStream is null) throw new NullReferenceException(nameof(ImageStream));
        ImageStream.Seek(0, SeekOrigin.Begin);
        var buffer = new byte[1024]; // parse image header
        int _ = ImageStream.Read(buffer.AsSpan());
        var type = ImageResolver.Resolve(buffer, out var size);
        
        string imageExt = type switch
        {
            ImageFormat.Jpeg => ".jpg",
            ImageFormat.Png => ".png",
            ImageFormat.Gif => ".gif",
            ImageFormat.Webp => ".webp",
            ImageFormat.Bmp => ".bmp",
            ImageFormat.Tiff => ".tiff",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        ImageStream.Seek(0, SeekOrigin.Begin);
        string md5 = ImageStream.Md5(true);
        uint fileLen = (uint)ImageStream.Length;

        ImageStream?.Close();
        ImageStream?.Dispose();
        
        var targetElem = Path != null ? new Elem
        {
            NotOnlineImage = new NotOnlineImage
            {
                FilePath = md5 + imageExt,
                FileLen = fileLen,
                DownloadPath = Path,
                ImgType = 1001,
                PicMd5 = md5.UnHex(),
                PicHeight = (uint)size.Y,
                PicWidth = (uint)size.X,
                ResId = Path,
                Original = 1, // true
                PbRes = new NotOnlineImage.PbReserve { Field1 = 0 }
            }
        } : new Elem
        {
            CustomFace = new CustomFace
            {
                FilePath = $"{{{$"{md5[..8]}-{md5.Substring(8, 4)}-{md5.Substring(12, 4)}-{md5.Substring(16, 4)}-{md5.Substring(20, 12)}".ToUpper()}}}{imageExt}",
                FileId = FileId,
                ServerIp = 0,
                ServerPort = 0,
                FileType = 1001,
                Useful = 1,
                Md5 = md5.UnHex(),
                ImageType = 1001,
                Width = (int)size.X,
                Height = (int)size.Y,
                Size = fileLen,
                Origin = 1,
                ThumbWidth = 0,
                ThumbHeight = 0,
                PbReserve = new CustomFaceExtra { Field1 = 0 }
            }
        };
        
        return new[] { targetElem };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.NotOnlineImage is not null)
        {
            var target = elems.NotOnlineImage;
            return new ImageEntity
            {
                PictureSize = new Vector2(target.PicWidth, target.PicHeight),
                FilePath = target.FilePath,
                ImageSize = target.FileLen,
                ImageUrl = $"{BaseUrl}{target.OrigUrl}"
            };
        }
        
        if (elems.CustomFace is not null)
        {
            var target = elems.CustomFace;
            if (target.OrigUrl.StartsWith("&rkey=")) return null; // NTQQ's shit
            
            return new ImageEntity
            {
                PictureSize = new Vector2(target.Width, target.Height),
                FilePath = target.FilePath,
                ImageSize = target.Size,
                ImageUrl = $"{LegacyBaseUrl}{target.OrigUrl}"
            };
        }

        if (elems.CommonElem is { ServiceType: 48 })
        {
            var extra = Serializer.Deserialize<ImageExtra>(elems.CommonElem.PbElem.AsSpan());
            
            return new ImageEntity
            {
                PictureSize = new Vector2(extra.Metadata.File.FileInfo.PicWidth, extra.Metadata.File.FileInfo.PicHeight),
                FilePath = extra.Metadata.File.FileInfo.FilePath,
                ImageSize = (uint)extra.Metadata.File.FileInfo.FileSize,
                ImageUrl = $"https://{extra.Metadata.Urls.Domain}{extra.Metadata.Urls.Suffix}{extra.Credential.Resp.GroupKey?.RKey ?? extra.Credential.Resp.FriendKey?.RKey}"
            };
        }
        
        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";
}