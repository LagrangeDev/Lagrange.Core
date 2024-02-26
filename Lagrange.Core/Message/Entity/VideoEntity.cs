using System.Numerics;
using System.Text;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(VideoFile))]
public class VideoEntity : IMessageEntity
{
    public string FilePath { get; set; } = string.Empty;

    public string VideoHash { get; set; } = string.Empty;
    
    public Vector2 Size { get; }
    
    public int VideoSize { get; }

    public string VideoUrl { get; set; } = string.Empty;
    
    internal Stream? VideoStream { get; set; }
    
    internal string? VideoUuid { get; }
    
    internal VideoEntity() { }
    
    internal VideoEntity(Vector2 size, int videoSize, string filePath, string fileMd5, string videoUuid)
    {
        Size = size;
        VideoSize = videoSize;
        FilePath = filePath;
        VideoHash = fileMd5;
        VideoUuid = videoUuid;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.VideoFile is not { } videoFile) return null;
        
        var size = new Vector2(videoFile.ThumbWidth, videoFile.ThumbHeight);
        return new VideoEntity(size, videoFile.FileSize, videoFile.FileName, videoFile.FileMd5.Hex(), videoFile.FileUuid);
    }

    public string ToPreviewString()
    {
        return $"[Video {Size.X}x{Size.Y}]: {VideoSize} {VideoUrl}";
    }
}