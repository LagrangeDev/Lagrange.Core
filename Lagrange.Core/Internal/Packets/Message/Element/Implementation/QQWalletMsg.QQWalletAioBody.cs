using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

internal partial class QQWalletMsg
{
    [ProtoContract]
    internal class QQWalletAioBody
    {
        [ProtoMember(1)] public ulong SendUin { get; set; }
        
        [ProtoMember(2)] public QQWalletAioElem Sender { get; set; }
        
        [ProtoMember(3)] public QQWalletAioElem Receiver { get; set; }
        
        [ProtoMember(4, DataFormat = DataFormat.ZigZag)] public int ChannelId { get; set; }
        
        [ProtoMember(5, DataFormat = DataFormat.ZigZag)] public int TemplateId { get; set; }
        
        [ProtoMember(6)] public uint Resend { get; set; }
        
        [ProtoMember(7)] public uint MsgPriority { get; set; }
        
        [ProtoMember(8, DataFormat = DataFormat.ZigZag)] public int RedType { get; set; }
        
        [ProtoMember(9)] public byte[] BillNo { get; set; }
        
        [ProtoMember(10)] public byte[] AuthKey { get; set; }
        
        [ProtoMember(11, DataFormat = DataFormat.ZigZag)] public int SessionType { get; set; }
        
        [ProtoMember(12, DataFormat = DataFormat.ZigZag)] public int MsgType { get; set; }
        
        [ProtoMember(13, DataFormat = DataFormat.ZigZag)] public int EnvelOpeId { get; set; }
        
        [ProtoMember(14)] public byte[] Name { get; set; }
        
        [ProtoMember(15, DataFormat = DataFormat.ZigZag)] public int ConfType { get; set; }
        
        [ProtoMember(16, DataFormat = DataFormat.ZigZag)] public int MsgFrom { get; set; }
        
        [ProtoMember(17)] public byte[] PcBody { get; set; }
        
        [ProtoMember(18)] public byte[] Index { get; set; }
        
        [ProtoMember(19)] public uint RedChannel { get; set; }
        
        [ProtoMember(20)] public ulong[] GrapUin { get; set; }
        
        [ProtoMember(21)] public byte[] PbReserve { get; set; }
    }
}