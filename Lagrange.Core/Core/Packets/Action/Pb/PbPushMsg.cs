using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action.Pb;

[ProtoContract]
internal class PbPushMsg
{
    [ProtoMember(1)] public Message.Message Msg { get; set; }
    
    [ProtoMember(2)] public int Svrip { get; set; }
    
    [ProtoMember(3)] public byte[] PushToken { get; set; }
    
    [ProtoMember(4)] public uint PingFlag { get; set; }
    
    [ProtoMember(9)] public uint GeneralFlag { get; set; }
    
    [ProtoMember(10)] public ulong BindUin { get; set; }
}