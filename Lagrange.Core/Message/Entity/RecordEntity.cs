using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class RecordEntity : IMessageEntity
{
    public int AudioLength { get; set; }
    
    public string FilePath { get; set; } = string.Empty;

    public string AudioName { get; set; } = string.Empty;
    
    public int AudioSize { get; set; }
    
    public string AudioUrl { get; set; } = string.Empty;
    
    internal Stream? AudioStream { get; set; }

    internal string? Path { get; set; }
    
    internal string? AudioUuid { get; set; }
    
    internal string? FileSha1 { get; set; }
    
    internal RecordEntity() { }
    
    public RecordEntity(string filePath)
    {
        FilePath = filePath;
        AudioStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public RecordEntity(byte[] file)
    {
        FilePath = string.Empty;
        AudioStream = new MemoryStream(file);
    }

    internal RecordEntity(string audioUuid, string audioName)
    {
        AudioUuid = audioUuid;
        AudioName = audioName;
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
        if (elem.CommonElem is { BusinessType:22, ServiceType: 48 } common) // businessType = 22 for Group
        {
            var extra = Serializer.Deserialize<ImageExtra>(common.PbElem.AsSpan());

            if (extra.Metadata.File.FileUuid != null)
            {
                return new RecordEntity(extra.Metadata.File.FileUuid, extra.Metadata.File.FileInfo.FilePath)
                {
                    FileSha1 = extra.Metadata.File.FileInfo.FileSha1
                };
            }
        }
        
        return null;
    }

    public string ToPreviewString() =>  $"[{nameof(RecordEntity)}: {AudioUrl}]";
}