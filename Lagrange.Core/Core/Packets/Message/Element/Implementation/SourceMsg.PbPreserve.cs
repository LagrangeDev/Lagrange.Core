using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

internal partial class SrcMsg
{
    [ProtoContract]
    internal class Preserve
    {
        [ProtoMember(3)] public long? Field3 { get; set; }
        
        [ProtoMember(6)] public string? SenderUid { get; set; }
        
        [ProtoMember(7)] public string? ReceiverUid { get; set; }
        
        [ProtoMember(8)] public int? Field8 { get; set; }
    }
}