using System.Security.Cryptography;
using Lagrange.Core.Core.Packets.Message.Component;
using Lagrange.Core.Core.Packets.Message.Component.Extra;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

public class FileEntity : IMessageEntity
{
    public long FileSize { get; set; }
    
    public string FileName { get; set; }
    
    public byte[] FileMd5 { get; set; }
    
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

    private FileEntity(long fileSize, string fileName, byte[] fileMd5)
    {
        FileSize = fileSize;
        FileName = fileName;
        FileMd5 = fileMd5;
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
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems) => null;

    IMessageEntity? IMessageEntity.UnpackMessageContent(ReadOnlySpan<byte> content)
    {
        var extra = Serializer.Deserialize<FileExtra>(content);
        var notOnlineFile = extra.File;
        
        return notOnlineFile is { FileSize: not null, FileName: not null, FileMd5: not null } 
            ? new FileEntity((long)notOnlineFile.FileSize, notOnlineFile.FileName, notOnlineFile.FileMd5) 
            : null;
    }

    public string ToPreviewString() => $"[File] {FileName} ({FileSize})";
}