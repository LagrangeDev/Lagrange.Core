using System.Numerics;
using System.Text;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(VideoFile))]
public class VideoEntity : IMessageEntity
{
    public string FilePath { get; set; } = string.Empty;
    
    public Vector2 Size { get; }
    
    public int VideoSize { get; }
    
    internal VideoEntity(Vector2 size, int videoSize, string filePath)
    {
        Size = size;
        VideoSize = videoSize;
        FilePath = filePath;
    }

    internal VideoEntity() { }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        throw new NotImplementedException();
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.VideoFile is not { } videoFile) return null;
        
        var size = new Vector2(videoFile.FileWidth, videoFile.FileHeight);
        
        return new VideoEntity(size, elem.VideoFile.FileSize, Encoding.UTF8.GetString(elem.VideoFile.FileName));
    }

    public string ToPreviewString()
    {
        return $"[Video {Size.X}x{Size.Y}]: {VideoSize}";
    }
}