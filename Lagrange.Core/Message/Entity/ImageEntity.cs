using System.Numerics;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
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
    
    internal MsgInfo? MsgInfo { get; set; }
    
    internal NotOnlineImage? Compat { get; set; }
    
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
        return new Elem[]
        {
            new() { NotOnlineImage = Compat },
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
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.NotOnlineImage is { } image)
        {
            string baseUrl = image.OrigUrl.Contains("&rkey=") ? BaseUrl : LegacyBaseUrl;
            
            return new ImageEntity
            {
                PictureSize = new Vector2(image.PicWidth, image.PicHeight),
                FilePath = image.FilePath,
                ImageSize = image.FileLen,
                ImageUrl = $"{baseUrl}{image.OrigUrl}"
            };
        }
        
        if (elems.CustomFace is { } face)
        {
            if (face.OrigUrl.Contains("&rkey=")) return null; // NTQQ's shit
            
            return new ImageEntity
            {
                PictureSize = new Vector2(face.Width, face.Height),
                FilePath = face.FilePath,
                ImageSize = face.Size,
                ImageUrl = $"{LegacyBaseUrl}{face.OrigUrl}"
            };
        }

        if (elems.CommonElem is { ServiceType: 48 } common)
        {
            var extra = Serializer.Deserialize<ImageExtra>(common.PbElem.AsSpan());

            if (extra.Metadata.Urls != null)
            {
                string url = $"https://{extra.Metadata.Urls.Domain}{extra.Metadata.Urls.Suffix}{extra.Credential.Resp.GroupKey?.RKey ?? extra.Credential.Resp.FriendKey?.RKey}";
                
                return new ImageEntity
                {
                    PictureSize = new Vector2(extra.Metadata.File.FileInfo.PicWidth, extra.Metadata.File.FileInfo.PicHeight),
                    FilePath = extra.Metadata.File.FileInfo.FilePath,
                    ImageSize = (uint)extra.Metadata.File.FileInfo.FileSize,
                    ImageUrl = url
                };
            }
        }
        
        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";
}