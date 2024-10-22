using ProtoBuf;

#pragma warning disable CS8618
// Resharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1Response
{
    [ProtoMember(1)] public int Field1 { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x9154_1ResponseContent CommonFace { get; set; }

    [ProtoMember(3)] public OidbSvcTrpcTcp0x9154_1ResponseContent SpecialBigFace { get; set; }

    [ProtoMember(4)] public OidbSvcTrpcTcp0x9154_1ResponsesMagicEmojiContent SpecialMagicFace { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponseContent
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x9154_1ResponseContentEmojiList[] EmojiList { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x9154_1ResponseContentResourceUrl? ResourceUrl { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponsesMagicEmojiContent
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x9154_1ResponsesMagicEmojiContentList Field1 { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x9154_1ResponseContentResourceUrl? ResourceUrl { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponsesMagicEmojiContentList
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0x9154_1ResponseContentEmoji[] EmojiList { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponseContentEmojiList
{
    [ProtoMember(1)] public string EmojiPackName { get; set; }

    [ProtoMember(2)] public OidbSvcTrpcTcp0x9154_1ResponseContentEmoji[] EmojiDetail { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponseContentEmoji
{
    [ProtoMember(1)] public string QSid { get; set; }

    [ProtoMember(2)] public string? QDes { get; set; }

    [ProtoMember(3)] public string? EMCode { get; set; }
    
    [ProtoMember(4)] public int? QCid { get; set; }

    [ProtoMember(5)] public int? AniStickerType { get; set; }

    [ProtoMember(6)] public int? AniStickerPackId { get; set; }

    [ProtoMember(7)] public int? AniStickerId { get; set; }

    [ProtoMember(8)] public OidbSvcTrpcTcp0x9154_1ResponseContentResourceUrl? Url { get; set; }
    
    [ProtoMember(9)] public string[]? EmojiNameAlias { get; set; }
    
    [ProtoMember(10)] public int? Unknown10 { get; set; }
    
    [ProtoMember(13)] public int? AniStickerWidth { get; set; }
    
    [ProtoMember(14)] public int? AniStickerHeight { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x9154_1ResponseContentResourceUrl
{
    [ProtoMember(1)] public string? BaseUrl { get; set; }

    [ProtoMember(2)] public string? AdvUrl { get; set; }
}