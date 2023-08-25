using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class ImageUploadEvent : ProtocolEvent
{
    public Stream Stream { get; }
    
    public string TargetUid { get; }
    
    public uint FileSize { get; }
    
    public string FileMd5 { get; }
    
    public string FileSha1 { get; }
    
    public string Ticket { get; }
    
    public byte[] CommonAdditional { get; } // Field 6 in Response
    
    public NotOnlineImage ImageInfo { get; } // Field 8 in Response

    private ImageUploadEvent(Stream stream, string targetUid) : base(true)
    {
        Stream = stream;
        TargetUid = targetUid;
        FileSize = (uint)stream.Length;
        FileMd5 = stream.Md5(true);
        FileSha1 = stream.Sha1(true);
    }
    
    private ImageUploadEvent(int resultCode, string ticket, byte[] commonAdditional, NotOnlineImage image) : base(resultCode)
    {
        Ticket = ticket;
        CommonAdditional = commonAdditional;
        ImageInfo = image;
    }
    
    public static ImageUploadEvent Create(Stream stream, string targetUid) => new(stream, targetUid);

    public static ImageUploadEvent Result(int resultCode, string ticket, byte[] commonAdditional, NotOnlineImage image) 
        => new(resultCode, ticket, commonAdditional, image);
}