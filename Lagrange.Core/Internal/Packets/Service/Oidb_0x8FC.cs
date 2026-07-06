using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D8FCReqBody
{
    [ProtoPackable]
    internal partial class CardNameElem
    {
        public enum CardType : int
        {
            Text = 1,
            XC = 2,
        }

        [ProtoMember(1)] public CardType Type { get; set; } = CardType.Text;

        [ProtoMember(2)] public byte[] Value { get; set; }
    }

    [ProtoPackable]
    internal partial class ClientInfo
    {
        [ProtoMember(1)] public uint Implat { get; set; }

        [ProtoMember(2)] public string ClientVer { get; set; }
    }

    [ProtoPackable]
    internal partial class LevelName
    {
        [ProtoMember(1)] public uint Level { get; set; }

        [ProtoMember(2)] public string Name { get; set; }
    }

    [ProtoPackable]
    internal partial class RichCardNameElem
    {
        [ProtoMember(1)] public byte[] Ctrl { get; set; }

        [ProtoMember(2)] public byte[] Text { get; set; }
    }

    [ProtoPackable]
    internal partial class CommCardNameBuf
    {
        [ProtoMember(1)] public List<RichCardNameElem> RichCardName { get; set; }

        [ProtoMember(2)] public uint CoolId { get; set; }
    }

    [ProtoPackable]
    internal partial class MemberInfo
    {
        [ProtoMember(1)] public string Uid { get; set; }

        [ProtoMember(2)] public uint? Point { get; set; }

        [ProtoMember(3)] public uint? ActiveKey { get; set; }

        [ProtoMember(4)] public uint? Level { get; set; }

        [ProtoMember(5)] public byte[] SpecialTitle { get; set; }

        [ProtoMember(6)] public uint? SpecialTitleExpireTime { get; set; }

        [ProtoMember(7)] public byte[] UinName { get; set; }

        [ProtoMember(8)] public byte[] MemberCardName { get; set; }

        [ProtoMember(9)] public byte[] Phone { get; set; }

        [ProtoMember(10)] public byte[] Email { get; set; }

        [ProtoMember(11)] public byte[] Remark { get; set; }

        [ProtoMember(12)] public uint? Gender { get; set; }

        [ProtoMember(13)] public byte[] Job { get; set; }

        [ProtoMember(14)] public uint? TribeLevel { get; set; }

        [ProtoMember(15)] public uint? TribePoint { get; set; }

        [ProtoMember(16)] public List<CardNameElem> RichCardName { get; set; }

        [ProtoMember(17)] public byte[] CommRichCardName { get; set; }

        [ProtoMember(18)] public uint? RingtoneId { get; set; }

        [ProtoMember(19)] public byte[] GroupHonor { get; set; }

        [ProtoMember(20)] public uint? CmduinFlagEx3Grocery { get; set; }

        [ProtoMember(21)] public uint? CmduinFlagEx3Mask { get; set; }
    }

    [ProtoMember(1)] public long? GroupCode { get; set; }

    [ProtoMember(2)] public uint? ShowFlag { get; set; }

    [ProtoMember(3)] public List<MemberInfo> MemLevelInfo { get; set; }

    [ProtoMember(4)] public List<LevelName> LevelNames { get; set; }

    [ProtoMember(5)] public uint? UpdateTime { get; set; }

    [ProtoMember(6)] public uint? OfficeMode { get; set; }

    [ProtoMember(7)] public uint? GroupOpenAppid { get; set; }

    [ProtoMember(8)] public ClientInfo? Client { get; set; }

    [ProtoMember(9)] public byte[]? AuthKey { get; set; }

    [ProtoMember(10)] public List<LevelName> LevelNamesNew { get; set; }
}

[ProtoPackable]
internal partial class D8FCRspBody
{
    [ProtoMember(1)] public long GroupCode { get; set; }

    [ProtoMember(2)] public string ErrInfo { get; set; }

    [ProtoMember(3)] public byte[] CoolGroupCardRsp { get; set; }
}
