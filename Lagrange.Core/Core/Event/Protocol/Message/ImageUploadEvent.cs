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
    
    public byte[] CommonAdditional { get; } // Field 6 in Response
    
    public NotOnlineImage ImageInfo { get; } // Field 8 in Response

    protected ImageUploadEvent(Stream stream, string targetUid) : base(true)
    {
        Stream = stream;
        TargetUid = targetUid;
        FileSize = (uint)stream.Length;
        FileMd5 = stream.Md5(true);
        FileSha1 = stream.Sha1(true);
    }
    
    protected ImageUploadEvent(int resultCode, byte[] commonAdditional, NotOnlineImage image) : base(resultCode)
    {
        CommonAdditional = commonAdditional;
        ImageInfo = image;
    }
    
    public static ImageUploadEvent Create(Stream stream, string targetUid) => new(stream, targetUid);

    public static ImageUploadEvent Result(int resultCode, byte[] commonAdditional, NotOnlineImage image) 
        => new(resultCode, commonAdditional, image);
}