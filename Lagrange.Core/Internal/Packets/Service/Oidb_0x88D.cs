using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

[ProtoPackable]
internal partial class D88DGroupExInfoOnly
{
    [ProtoMember(1)] public uint? TribeId { get; set; }

    [ProtoMember(2)] public uint? MoneyForAddGroup { get; set; }
}

[ProtoPackable]
internal partial class D88DGroupGeoInfo
{
    [ProtoMember(1)] public ulong? OwnerUin { get; set; }

    [ProtoMember(2)] public uint? SetTime { get; set; }

    [ProtoMember(3)] public uint? CityId { get; set; }

    [ProtoMember(4)] public long? Longitude { get; set; }

    [ProtoMember(5)] public long? Latitude { get; set; }

    [ProtoMember(6)] public byte[]? GeoContent { get; set; }

    [ProtoMember(7)] public ulong? PoiId { get; set; }
}

[ProtoPackable]
internal partial class D88DGroupHeadPortrait
{
    [ProtoMember(1)] public uint? PicCount { get; set; }

    [ProtoMember(2)] public List<D88DGroupHeadPortraitInfo>? PicInfo { get; set; }

    [ProtoMember(3)] public uint? DefaultId { get; set; }

    [ProtoMember(4)] public uint? VerifyingPicCount { get; set; }

    [ProtoMember(5)] public List<D88DGroupHeadPortraitInfo>? VerifyingPicInfo { get; set; }
}

[ProtoPackable]
internal partial class D88DGroupHeadPortraitInfo
{
    [ProtoMember(1)] public uint? PicId { get; set; }

    [ProtoMember(2)] public uint? LeftX { get; set; }

    [ProtoMember(3)] public uint? LeftY { get; set; }

    [ProtoMember(4)] public uint? RightX { get; set; }

    [ProtoMember(5)] public uint? RightY { get; set; }
}

[ProtoPackable]
internal partial class D88DGroupInfo
{
    [ProtoMember(1)] public ulong? GroupOwner { get; set; }

    [ProtoMember(2)] public uint? GroupCreateTime { get; set; }

    [ProtoMember(3)] public uint? GroupFlag { get; set; }

    [ProtoMember(4)] public uint? GroupFlagExt { get; set; }

    [ProtoMember(5)] public uint? GroupMemberMaxNum { get; set; }

    [ProtoMember(6)] public uint? GroupMemberNum { get; set; }

    [ProtoMember(7)] public uint? GroupOption { get; set; }

    [ProtoMember(8)] public uint? GroupClassExt { get; set; }

    [ProtoMember(9)] public uint? GroupSpecialClass { get; set; }

    [ProtoMember(10)] public uint? GroupLevel { get; set; }

    [ProtoMember(11)] public uint? GroupFace { get; set; }

    [ProtoMember(12)] public uint? GroupDefaultPage { get; set; }

    [ProtoMember(13)] public uint? GroupInfoSeq { get; set; }

    [ProtoMember(14)] public uint? GroupRoamingTime { get; set; }

    [ProtoMember(15)] public string? GroupName { get; set; }

    [ProtoMember(16)] public string? GroupMemo { get; set; }

    [ProtoMember(17)] public string? GroupFingerMemo { get; set; }

    [ProtoMember(18)] public string? GroupClassText { get; set; }

    [ProtoMember(19)] public List<uint>? GroupAllianceCodes { get; set; }

    [ProtoMember(20)] public uint? GroupExtraAdmNum { get; set; }

    [ProtoMember(21)] public ulong? GroupUin { get; set; }

    [ProtoMember(22)] public uint? GroupCurMsgSeq { get; set; }

    [ProtoMember(23)] public uint? GroupLastMsgTime { get; set; }

    [ProtoMember(24)] public string? GroupQuestion { get; set; }

    [ProtoMember(25)] public string? GroupAnswer { get; set; }

    [ProtoMember(26)] public uint? GroupVisitorMaxNum { get; set; }

    [ProtoMember(27)] public uint? GroupVisitorCurNum { get; set; }

    [ProtoMember(28)] public uint? LevelNameSeq { get; set; }

    [ProtoMember(29)] public uint? GroupAdminMaxNum { get; set; }

    [ProtoMember(30)] public uint? GroupAioSkinTimestamp { get; set; }

    [ProtoMember(31)] public uint? GroupBoardSkinTimestamp { get; set; }

    [ProtoMember(32)] public string? GroupAioSkinUrl { get; set; }

    [ProtoMember(33)] public string? GroupBoardSkinUrl { get; set; }

    [ProtoMember(34)] public uint? GroupCoverSkinTimestamp { get; set; }

    [ProtoMember(35)] public string? GroupCoverSkinUrl { get; set; }

    [ProtoMember(36)] public uint? GroupGrade { get; set; }

    [ProtoMember(37)] public uint? ActiveMemberNum { get; set; }

    [ProtoMember(38)] public uint? CertificationType { get; set; }

    [ProtoMember(39)] public string? CertificationText { get; set; }

    [ProtoMember(40)] public string? GroupRichFingerMemo { get; set; }

    [ProtoMember(41)] public List<D88DTagRecord>? TagRecords { get; set; }

    [ProtoMember(42)] public D88DGroupGeoInfo? GroupGeoInfo { get; set; }

    [ProtoMember(43)] public uint? HeadPortraitSeq { get; set; }

    [ProtoMember(44)] public D88DGroupHeadPortrait? HeadPortrait { get; set; }

    [ProtoMember(45)] public uint? ShutupTimestamp { get; set; }

    [ProtoMember(46)] public uint? ShutupTimestampMe { get; set; }

    [ProtoMember(47)] public uint? CreateSourceFlag { get; set; }

    [ProtoMember(48)] public uint? CmduinMsgSeq { get; set; }

    [ProtoMember(49)] public uint? CmduinJoinTime { get; set; }

    [ProtoMember(50)] public uint? CmduinUinFlag { get; set; }

    [ProtoMember(51)] public uint? CmduinFlagEx { get; set; }

    [ProtoMember(52)] public uint? CmduinNewMobileFlag { get; set; }

    [ProtoMember(53)] public uint? CmduinReadMsgSeq { get; set; }

    [ProtoMember(54)] public uint? CmduinLastMsgTime { get; set; }

    [ProtoMember(55)] public uint? GroupTypeFlag { get; set; }

    [ProtoMember(56)] public uint? AppPrivilegeFlag { get; set; }

    [ProtoMember(57)] public D88DGroupExInfoOnly? GroupExInfo { get; set; }

    [ProtoMember(58)] public uint? GroupSecLevel { get; set; }

    [ProtoMember(59)] public uint? GroupSecLevelInfo { get; set; }

    [ProtoMember(60)] public uint? CmduinPrivilege { get; set; }

    [ProtoMember(61)] public string? PoidInfo { get; set; }

    [ProtoMember(62)] public uint? CmduinFlagEx2 { get; set; }

    [ProtoMember(63)] public ulong? ConfUin { get; set; }

    [ProtoMember(64)] public uint? ConfMaxMsgSeq { get; set; }

    [ProtoMember(65)] public uint? ConfToGroupTime { get; set; }

    [ProtoMember(66)] public uint? PasswordRedbagTime { get; set; }

    [ProtoMember(67)] public ulong? SubscriptionUin { get; set; }

    [ProtoMember(68)] public uint? MemberListChangeSeq { get; set; }

    [ProtoMember(69)] public uint? MemberCardSeq { get; set; }

    [ProtoMember(70)] public ulong? RootId { get; set; }

    [ProtoMember(71)] public ulong? ParentId { get; set; }

    [ProtoMember(72)] public uint? TeamSeq { get; set; }

    [ProtoMember(73)] public ulong? HistoryMsgBeginTime { get; set; }

    [ProtoMember(74)] public ulong? InviteNoAuthNumLimit { get; set; }

    [ProtoMember(75)] public uint? CmduinHistoryMsgSeq { get; set; }

    [ProtoMember(76)] public uint? CmduinJoinMsgSeq { get; set; }

    [ProtoMember(77)] public uint? GroupFlagExt3 { get; set; }

    [ProtoMember(78)] public uint? GroupOpenAppid { get; set; }

    [ProtoMember(79)] public uint? IsConfGroup { get; set; }

    [ProtoMember(80)] public uint? IsModifyConfGroupFace { get; set; }

    [ProtoMember(81)] public uint? IsModifyConfGroupName { get; set; }

    [ProtoMember(82)] public uint? NoFingerOpenFlag { get; set; }

    [ProtoMember(83)] public uint? NoCodeFingerOpenFlag { get; set; }

    [ProtoMember(84)] public uint? AutoAgreeJoinGroupUserNumForNormalGroup { get; set; }

    [ProtoMember(85)] public uint? AutoAgreeJoinGroupUserNumForConfGroup { get; set; }

    [ProtoMember(86)] public uint? IsAllowConfGroupMemberNick { get; set; }

    [ProtoMember(87)] public uint? IsAllowConfGroupMemberAtAll { get; set; }

    [ProtoMember(88)] public uint? IsAllowConfGroupMemberModifyGroupName { get; set; }

    [ProtoMember(89)] public string? LongGroupName { get; set; }

    [ProtoMember(90)] public uint? CmduinJoinRealMsgSeq { get; set; }

    [ProtoMember(91)] public uint? IsGroupFreeze { get; set; }

    [ProtoMember(92)] public uint? MsgLimitFrequency { get; set; }

    [ProtoMember(93)] public byte[]? JoinGroupAuth { get; set; }

    [ProtoMember(94)] public uint? HlGuildAppid { get; set; }

    [ProtoMember(95)] public uint? HlGuildSubType { get; set; }

    [ProtoMember(96)] public uint? HlGuildOrgid { get; set; }

    [ProtoMember(97)] public uint? IsAllowHlGuildBinary { get; set; }

    [ProtoMember(98)] public uint? CmduinRingtoneId { get; set; }

    [ProtoMember(99)] public uint? GroupFlagExt4 { get; set; }

    [ProtoMember(100)] public uint? GroupFreezeReason { get; set; }

    [ProtoMember(101)] public uint? IsAllowRecallMsg { get; set; }
}

[ProtoPackable]
internal partial class D88DReqBody
{
    [ProtoMember(1)] public uint? AppId { get; set; }

    [ProtoMember(2)] public List<D88DReqGroupInfo>? Groups { get; set; }

    [ProtoMember(3)] public uint? PcClientVersion { get; set; }
}

[ProtoPackable]
internal partial class D88DReqGroupInfo
{
    [ProtoMember(1)] public ulong? GroupCode { get; set; }

    [ProtoMember(2)] public D88DGroupInfo? GroupInfo { get; set; }

    [ProtoMember(3)] public uint? LastGetGroupNameTime { get; set; }
}

[ProtoPackable]
internal partial class D88DRspBody
{
    [ProtoMember(1)] public List<D88DRspGroupInfo>? Groups { get; set; }

    [ProtoMember(2)] public string? ErrorInfo { get; set; }
}

[ProtoPackable]
internal partial class D88DRspGroupInfo
{
    [ProtoMember(1)] public ulong? GroupCode { get; set; }

    [ProtoMember(2)] public uint? Result { get; set; }

    [ProtoMember(3)] public D88DGroupInfo? GroupInfo { get; set; }
}

[ProtoPackable]
internal partial class D88DTagRecord
{
    [ProtoMember(1)] public ulong? FromUin { get; set; }

    [ProtoMember(2)] public ulong? GroupCode { get; set; }

    [ProtoMember(3)] public byte[]? TagId { get; set; }

    [ProtoMember(4)] public ulong? SetTime { get; set; }

    [ProtoMember(5)] public uint? GoodNum { get; set; }

    [ProtoMember(6)] public uint? BadNum { get; set; }

    [ProtoMember(7)] public uint? TagLen { get; set; }

    [ProtoMember(8)] public byte[]? TagValue { get; set; }
}
