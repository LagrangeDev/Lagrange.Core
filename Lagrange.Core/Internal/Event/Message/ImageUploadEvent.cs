using Lagrange.Core.Utility.Extension;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageUploadEvent : ProtocolEvent
{
    public Stream Stream { get; }
    
    public string TargetUid { get; }
    
    public uint FileSize { get; }
    
    public string FileMd5 { get; }
    
    public string Ticket { get; }
    
    public bool IsExist { get; }
    
    public string ServerPath { get; }
    
    private ImageUploadEvent(Stream stream, string targetUid) : base(true)
    {
        Stream = stream;
        TargetUid = targetUid;
        FileSize = (uint)stream.Length;
        FileMd5 = stream.Md5(true);
    }
    
    private ImageUploadEvent(int resultCode, string ticket, bool isExist, string serverPath) : base(resultCode)
    {
        IsExist = isExist;
        Ticket = ticket;
        ServerPath = serverPath;
    }
    
    public static ImageUploadEvent Create(Stream stream, string targetUid) => new(stream, targetUid);
    
    public static ImageUploadEvent Result(int resultCode, string ticket, bool isExist, string imagePath) 
        => new(resultCode, ticket, isExist, imagePath);
}