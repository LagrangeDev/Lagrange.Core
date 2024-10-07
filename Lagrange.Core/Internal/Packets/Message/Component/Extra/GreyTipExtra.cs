using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class GreyTipExtra
{
    [ProtoMember(101)] public GreyTipExtraLayer1? Layer1 { get; set; }
}

[ProtoContract]
internal class GreyTipExtraLayer1
{
    [ProtoMember(1)] public GreyTipExtraInfo? Info { get; set; }
}

[ProtoContract]
internal class GreyTipExtraInfo
{
    [ProtoMember(1)] public uint Type { get; set; } // 1
    
    [ProtoMember(2)] public string Content { get; set; } = string.Empty; // "{\"gray_tip\":\"谨防兼职刷单、游戏交易、投资荐股、色情招嫖、仿冒公检法及客服人员的网络骗局，如有资金往来请谨慎操作。 \",\"object_type\":3,\"sub_type\":2,\"type\":4}"
}