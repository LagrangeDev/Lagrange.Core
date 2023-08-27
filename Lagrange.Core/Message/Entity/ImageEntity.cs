using System.Numerics;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Extension;

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

        string md5 = ImageStream.Md5(true);
        uint fileLen = (uint)ImageStream.Length;

        ImageStream?.Close();
        ImageStream?.Dispose();
        
        Path ??= "";
        return new Elem[]
        {
            new()
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
            }
        };
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
            return new ImageEntity
            {
                PictureSize = new Vector2(target.Width, target.Height),
                FilePath = target.FilePath,
                ImageSize = (uint)target.Size,
                ImageUrl = $"{LegacyBaseUrl}{target.OrigUrl}"
            };
        }
        
        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";
}