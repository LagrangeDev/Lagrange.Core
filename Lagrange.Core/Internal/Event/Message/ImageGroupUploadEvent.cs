using Lagrange.Core.Utility.Extension;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageGroupUploadEvent : ProtocolEvent
{
    public Stream Stream { get; }
    
    public uint TargetGroupUin { get; }
    
    public uint FileSize { get; }
    
    public string FileMd5 { get; }
    
    public string Ticket { get; }
    
    public bool IsExist { get; }
    
    public uint FileId { get; }
    
    private ImageGroupUploadEvent(Stream stream, uint targetGroupUin) : base(true)
    {
        Stream = stream;
        TargetGroupUin = targetGroupUin;
        FileSize = (uint)stream.Length;
        FileMd5 = stream.Md5(true);
    }
    
    private ImageGroupUploadEvent(int resultCode, string ticket, bool isExist, uint fileId) : base(resultCode)
    {
        IsExist = isExist;
        Ticket = ticket;
        FileId = fileId;
    }
    
    public static ImageGroupUploadEvent Create(Stream stream, uint targetGroupUin) => new(stream, targetGroupUin);
    
    public static ImageGroupUploadEvent Result(int resultCode, string ticket, bool isExist, uint fileId) 
        => new(resultCode, ticket, isExist, fileId);
}