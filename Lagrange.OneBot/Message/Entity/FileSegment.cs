using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class FileSegment(string fileName, string fileHash, string fileId, string url)
{
    public FileSegment() : this("", "", "", "") { }
    
    [JsonPropertyName("filename")] public string Filename { get; set; } = fileName;
    
    [JsonPropertyName("filehash")] public string Filehash { get; set; }  = fileHash;

    [JsonPropertyName("id")] public string Fileid { get; set; } = fileId;

    [JsonPropertyName("url")] public string Url { get; set; } = url;
    
}

[SegmentSubscriber(typeof(FileEntity), "file")]
public partial class FileSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is FileSegment fileSegment and not { Fileid: "" })
        {
            builder.Add(new FileEntity
            {
                FileName = fileSegment.Filename,
                FileUrl = fileSegment.Url,
                FileId = fileSegment.Fileid,
                FileHash = fileSegment.Filehash
            });
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not FileEntity fileEntity) throw new ArgumentException("Invalid entity type.");

        return new FileSegment(fileEntity.FileName, fileEntity.FileHash ?? "", fileEntity.FileId ?? fileEntity.FileUuid ??  "", fileEntity.FileUrl ?? "");
    }
}