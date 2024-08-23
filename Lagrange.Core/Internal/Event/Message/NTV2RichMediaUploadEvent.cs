#pragma warning disable CS8618

using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal abstract class NTV2RichMediaUploadEvent : ProtocolEvent
{
    public MsgInfo MsgInfo { get; }
    
    public string? UKey { get; }
    
    public List<IPv4> Network { get; }
    
    public List<SubFileInfo> SubFiles { get; }

    protected NTV2RichMediaUploadEvent() : base(true) { }

    protected NTV2RichMediaUploadEvent(int resultCode, MsgInfo msgInfo, string? uKey, List<IPv4> network, List<SubFileInfo> subFiles) : base(resultCode)
    {
        MsgInfo = msgInfo;
        UKey = uKey;
        Network = network;
        SubFiles = subFiles;
    }
}