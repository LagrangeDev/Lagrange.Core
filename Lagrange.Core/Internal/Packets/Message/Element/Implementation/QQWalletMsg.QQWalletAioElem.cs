using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

internal partial class QQWalletMsg
{
    [ProtoContract]
    internal class QQWalletAioElem
    {
        [ProtoMember(1)] public uint Background { get; set; }
        
        [ProtoMember(2)] public uint Icon { get; set; }
        
        [ProtoMember(3)] public string Title { get; set; }
        
        [ProtoMember(4)] public string Subtitle { get; set; }
        
        [ProtoMember(5)] public string Content { get; set; }
        
        [ProtoMember(6)] public byte[] LinkUrl { get; set; }
        
        [ProtoMember(7)] public byte[] BlackStripe { get; set; }
        
        [ProtoMember(8)] public byte[] Notice { get; set; }
        
        [ProtoMember(9)] public uint TitleColor { get; set; }
        
        [ProtoMember(10)] public uint SubtitleColor { get; set; }
        
        [ProtoMember(11)] public byte[] ActionsPriority { get; set; }
        
        [ProtoMember(12)] public byte[] JumpUrl { get; set; }
        
        [ProtoMember(13)] public byte[] NativeIos { get; set; }
        
        [ProtoMember(14)] public byte[] NativeAndroid { get; set; }
        
        [ProtoMember(15)] public byte[] IconUrl { get; set; }
        
        [ProtoMember(16)] public uint ContentColor { get; set; }
        
        [ProtoMember(17)] public uint ContentBgColor { get; set; }
        
        [ProtoMember(18)] public byte[] AioImageLeft { get; set; }
        
        [ProtoMember(19)] public byte[] AioImageRight { get; set; }
        
        [ProtoMember(20)] public byte[] CftImage { get; set; }
        
        [ProtoMember(21)] public byte[] PbReserve { get; set; }
    }
}