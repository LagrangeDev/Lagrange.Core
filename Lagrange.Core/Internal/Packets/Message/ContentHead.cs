using Lagrange.Core.Internal.Packets.Message.Routing;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message;

[ProtoContract]
internal class ContentHead
{
    [ProtoMember(1)] public uint Type { get; set; }  // 消息类型

    [ProtoMember(2)] public uint? SubType { get; set; }  // 消息子类型（0x211\0x2dc\0x210等系统消息的子类型,取值同c2c_cmd）

    [ProtoMember(3)] public uint? C2CCmd { get; set; }   // c2c消息子类型

    [ProtoMember(4)] public long? Random { get; set; } 

    [ProtoMember(5)] public uint? Sequence { get; set; }

    [ProtoMember(6)] public long? Timestamp { get; set; }

    [ProtoMember(7)] public long? PkgNum { get; set; } // 分包数目，消息需要分包发送时该值不为1

    [ProtoMember(8)] public uint? PkgIndex { get; set; } // 当前分包索引，从 0开始
    
    [ProtoMember(9)] public uint? DivSeq { get; set; }  // 消息分包的序列号，同一条消息的各个分包的 div_seq 相同
    
    [ProtoMember(10)] public uint AutoReply { get; set; }

    [ProtoMember(11)] public uint? NTMsgSeq { get; set; }  // 两个uin之间c2c消息唯一递增seq

    [ProtoMember(12)] public ulong? MsgUid { get; set; }

    [ProtoMember(15)] public ForwardHead? Forward { get; set; }
}