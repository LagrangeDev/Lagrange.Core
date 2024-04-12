using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class RecordEntity : IMessageEntity
{
    public int AudioLength { get; set; }
    
    public string FilePath { get; set; } = string.Empty;

    public string AudioName { get; set; } = string.Empty;
    
    public int AudioSize => (int)AudioStream!.Value.Length;
    
    public string AudioUrl { get; set; } = string.Empty;

    #region Internal Properties

    internal Lazy<Stream>? AudioStream { get; set; }
    
    internal string? AudioUuid { get; set; }
    
    internal string? FileSha1 { get; set; }
    
    internal MsgInfo? MsgInfo { get; set; }
    
    internal RichText? Compat { get; set; }

    #endregion
    
    internal RecordEntity() { }
    
    public RecordEntity(string filePath, int audioLength = 0)
    {
        FilePath = filePath;
        AudioStream = new Lazy<Stream>(() => new FileStream(filePath, FileMode.Open, FileAccess.Read));
        AudioLength = audioLength;
    }

    public RecordEntity(byte[] file, int audioLength = 0)
    {
        FilePath = string.Empty;
        AudioStream = new Lazy<Stream>(() => new MemoryStream(file));
        AudioLength = audioLength;
    }

    internal RecordEntity(string audioUuid, string audioName)
    {
        AudioUuid = audioUuid;
        AudioName = audioName;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var common = MsgInfo.Serialize();
        
        return new Elem[]
        {
            new()
            {
                CommonElem = new CommonElem
                {
                    ServiceType = 48,
                    PbElem = common.ToArray(),
                    BusinessType = 22
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is { BusinessType: 22 or 12, ServiceType: 48 } common) // businessType = 22 for Group
        {
            var extra = Serializer.Deserialize<MsgInfo>(common.PbElem.AsSpan());
            var index = extra.MsgInfoBody[0].Index;

            return new RecordEntity(index.FileUuid, index.Info.FileName)
            {
                AudioLength = (int)index.Info.Time,
                FileSha1 = index.Info.FileSha1,
                MsgInfo = extra
            };
        }
        
        return null;
    }

    public string ToPreviewString() =>  $"[{nameof(RecordEntity)}: {AudioUrl}]";

    public string ToPreviewText() => "[语音]";
}
