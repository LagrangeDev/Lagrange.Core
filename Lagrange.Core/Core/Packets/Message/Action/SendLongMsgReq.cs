using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Action;

[ProtoContract]
internal class SendLongMsgReq
{
    [ProtoMember(2)] public SendLongMsgInfo? Info { get; set; }
    
    [ProtoMember(15)] public LongMsgSettings? Settings { get; set; }
}

[ProtoContract]
internal class SendLongMsgInfo
{
    [ProtoMember(1)] public uint Type { get; set; } // Group: 3, Friend: 1
    
    [ProtoMember(2)] public LongMsgUid? Uid { get; set; }
    
    [ProtoMember(3)] public uint? GroupUin { get; set; }
    
    [ProtoMember(4)] public byte[]? Payload { get; set; }
}