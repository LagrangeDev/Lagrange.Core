using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class ImageUploadSuccessEvent : ProtocolEvent
{
    public string TargetUid { get; }
    
    public byte[] CommonAdditional { get; } // Field 6 in Response

    private ImageUploadSuccessEvent(string targetUid, byte[] commonAdditional) : base(true)
    {
        TargetUid = targetUid;
        CommonAdditional = commonAdditional;
    }
    
    private ImageUploadSuccessEvent(int resultCode) : base(resultCode) { }
    
    public static ImageUploadSuccessEvent Create(string targetUid, byte[] commonAdditional) => new(targetUid, commonAdditional);

    public static ImageUploadSuccessEvent Result(int resultCode) => new(resultCode);
}