using Lagrange.Core.Internal.Packets.Message.Component;
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

    [Obsolete]
    public string FilePath { get; set; } = string.Empty;

    public byte[] AudioMd5 { get; set; } = Array.Empty<byte>();

    public string AudioName { get; set; } = string.Empty;

    [Obsolete]
    public int AudioSize { get; }

    public string AudioUrl { get; set; } = string.Empty;

    #region Internal Properties

    internal Lazy<Stream>? AudioStream { get; set; }

    internal string? AudioUuid { get; set; }

    internal string? FileSha1 { get; set; }

    internal MsgInfo? MsgInfo { get; set; }

    internal RichText? Compat { get; set; }

    #endregion

    internal RecordEntity() { }

    public RecordEntity(string audioUuid, string audioName, byte[] audioMd5, string audioUrl)
    {
        AudioUuid = audioUuid;
        AudioName = audioName;
        AudioMd5 = audioMd5;
        AudioUrl = audioUrl;
    }

    public RecordEntity(string filePath, int audioLength = 0) : this(File.ReadAllBytes(filePath), audioLength) { }

    public RecordEntity(byte[] file, int audioLength = 0)
    {
        // We should first determine whether the parameters are valid
        if (file == null) throw new ArgumentNullException(nameof(file));

        AudioStream = new Lazy<Stream>(() => new MemoryStream(file));
        AudioLength = audioLength;
    }

    internal RecordEntity(string audioUuid, string audioName, byte[] audioMd5)
    {
        AudioUuid = audioUuid;
        AudioName = audioName;
        AudioMd5 = audioMd5;
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

            return new RecordEntity(index.FileUuid, index.Info.FileName, index.Info.FileHash.UnHex())
            {
                AudioLength = (int)index.Info.Time,
                FileSha1 = index.Info.FileSha1,
                MsgInfo = extra
            };
        }

        return null;
    }

    public string ToPreviewString() => $"[{nameof(RecordEntity)}: {AudioUrl}]";

    public string ToPreviewText() => "[语音]";
}
