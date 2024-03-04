using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(TransElem))]
public class FileEntity : IMessageEntity
{
    public long FileSize { get; internal set; }
    
    public string FileName { get; internal set; }
    
    public byte[] FileMd5 { get; internal set; }
    
    public string? FileUrl { get; internal set; }
    
    /// <summary>
    /// Only Group File has such field
    /// </summary>
    public string? FileId { get; set; }
    
    internal string? FileUuid { get; set; }
    
    internal string? FileHash { get; set; }
    
    internal Stream? FileStream { get; set; }
    
    internal byte[] FileSha1 { get; set; }
    
    public FileEntity()
    {
        FileName = "";
        FileMd5 = Array.Empty<byte>();
        FileSha1 = Array.Empty<byte>();
    }
    
    /// <summary>
    /// This entity could not be directly sent via <see cref="MessageChain"/>,
    /// it should be sent via <see cref="Lagrange.Core.Common.Interface.Api.GroupExt.GroupFSUpload"/>
    /// </summary>
    public FileEntity(string path)
    {
        FileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        FileMd5 = FileStream.Md5().UnHex();
        FileSize = FileStream.Length;
        FileName = Path.GetFileName(path);
        FileSha1 = FileStream.Sha1().UnHex();
    }

    /// <summary>
    /// This entity could not be directly sent via <see cref="MessageChain"/>,
    /// it should be sent via <see cref="Lagrange.Core.Common.Interface.Api.GroupExt.GroupFSUpload"/>
    /// </summary>
    public FileEntity(byte[] payload, string fileName)
    {
        FileStream = new MemoryStream(payload);
        FileMd5 = payload.Md5().UnHex();
        FileSize = payload.Length;
        FileName = fileName;
        FileSha1 = FileStream.Sha1().UnHex();
    }

    internal FileEntity(long fileSize, string fileName, byte[] fileMd5, string fileUuid, string fileHash)
    {
        FileSize = fileSize;
        FileName = fileName;
        FileMd5 = fileMd5;
        FileUuid = fileUuid;
        FileHash = fileHash;
        FileSha1 = Array.Empty<byte>();
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement() => Array.Empty<Elem>();

    object IMessageEntity.PackMessageContent() => new FileExtra
    {
        File = new NotOnlineFile
        { 
            FileType = 0, 
            FileUuid = FileUuid,
            FileMd5 = FileMd5,
            FileName = FileName,
            FileSize = FileSize,
            Subcmd = 1,
            DangerEvel = 0,
            ExpireTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0) + TimeSpan.FromDays(7)).TotalSeconds,
            FileHash = FileHash
        }
    };
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.TransElem is { ElemType: 24 } trans)
        {
            var payload = new BinaryPacket(trans.ElemValue);
            payload.Skip(1);
            var data = payload.ReadBytes(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
            var extra = Serializer.Deserialize<GroupFileExtra>(data.AsSpan()).Inner.Info;

            return new FileEntity
            {
                FileSize = extra.FileSize,
                FileMd5 = extra.FileMd5.UnHex(),
                FileId = extra.FileId
            };
        }

        return null;
    }

    public string ToPreviewString() => $"[File] {FileName} ({FileSize}): {FileUrl ?? "failed to receive file url"}";
}