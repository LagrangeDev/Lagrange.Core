using System.Security.Cryptography;
using Lagrange.Core.Core.Packets.Message.Component;
using Lagrange.Core.Core.Packets.Message.Component.Extra;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(TransElem))]
public class FileEntity : IMessageEntity
{
    public bool IsGroup { get; internal set; }
    
    public long FileSize { get; internal set; }
    
    public string FileName { get; internal set; }
    
    public byte[] FileMd5 { get; internal set; }
    
    internal string? FileUuid { get; set; }
    
    internal string? FileHash { get; set; }
    
    public FileEntity()
    {
        FileName = "";
        FileMd5 = Array.Empty<byte>();
        
    }
    
    public FileEntity(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var md5 = MD5.Create();
        FileMd5 = md5.ComputeHash(stream);
        FileSize = stream.Length;
        FileName = Path.GetFileName(path);
    }

    private FileEntity(long fileSize, string fileName, byte[] fileMd5, string fileUuid, string fileHash)
    {
        FileSize = fileSize;
        FileName = fileName;
        FileMd5 = fileMd5;
        FileUuid = fileUuid;
        FileHash = fileHash;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement() => Array.Empty<Elem>();

    object IMessageEntity.PackMessageContent() => new FileExtra
    {
        File = new NotOnlineFile
        { 
            FileType = 0, 
            FileUuid = "",
            FileMd5 = FileMd5,
            FileName = FileName,
            FileSize = FileSize,
            Subcmd = 1,
            DangerEvel = 0,
            ExpireTime = DateTime.Now.AddDays(7).Second,
            FileHash = "" // TODO: Send out Oidb
        }
    };
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.TransElem?.ElemType == 24)
        {
            var payload = new BinaryPacket(elems.TransElem.ElemValue);
            payload.Skip(1);
            var protobuf = payload.ReadBytes(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
            Console.WriteLine(protobuf.Hex());
        }

        return null;
    }

    IMessageEntity? IMessageEntity.UnpackMessageContent(ReadOnlySpan<byte> content)
    {
        var extra = Serializer.Deserialize<FileExtra>(content);
        var file = extra.File;
        
        return file is { FileSize: not null, FileName: not null, FileMd5: not null, FileUuid: not null, FileHash: not null }
            ? new FileEntity((long)file.FileSize, file.FileName, file.FileMd5, file.FileUuid, file.FileHash) 
            : null;
    }

    public string ToPreviewString() => $"[File] {FileName} ({FileSize})";
}