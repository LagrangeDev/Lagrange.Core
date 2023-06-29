using System.Numerics;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(NotOnlineImage))]
public class ImageEntity : IMessageEntity
{
    private const string BaseUrl = "https://multimedia.nt.qq.com.cn";
    
    public Vector2 PictureSize { get; set; }
    
    public string FilePath { get; set; } = string.Empty;
    
    public uint ImageSize { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public ImageEntity() { }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                CommonElem = new CommonElem
                {
                    
                }
            },
            new()
            {
                NotOnlineImage = new NotOnlineImage
                {
                    
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
        
        return null;
    }

    public string ToPreviewString() => $"[Image: {PictureSize.X}x{PictureSize.Y}] {FilePath} {ImageSize} {ImageUrl}";
}