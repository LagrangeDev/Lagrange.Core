namespace Lagrange.Core.Common.Entity;

[Serializable]
public class SysFaceEntry
{
    public string QSid { get; set; }

    public string? QDes { get; set; }

    public string? EMCode { get; set; }

    public int? QCid { get; set; }

    public int? AniStickerType { get; set; }

    public int? AniStickerPackId { get; set; }

    public int? AniStickerId { get; set; }

    public string? Url { get; set; }

    public string[]? EmojiNameAlias { get; set; }

    public int? AniStickerWidth { get; set; }

    public int? AniStickerHeight { get; set; }

    public SysFaceEntry(string qSid, string? qDes, string? emCode, int? qCid, int? aniStickerType,
        int? aniStickerPackId, int? aniStickerId, string? url, string[]? emojiNameAlias, int? aniStickerWidth,
        int? aniStickerHeight)
    {
        QSid = qSid;
        QDes = qDes;
        EMCode = emCode;
        QCid = qCid;
        AniStickerType = aniStickerType;
        AniStickerPackId = aniStickerPackId;
        AniStickerId = aniStickerId;
        Url = url;
        EmojiNameAlias = emojiNameAlias;
        AniStickerWidth = aniStickerWidth;
        AniStickerHeight = aniStickerHeight;
    }
}

[Serializable]
public class SysFacePackEntry
{
    public string EmojiPackName { get; set; }

    public SysFaceEntry[] Emojis { get; set; }

    public SysFacePackEntry(string emojiPackName, SysFaceEntry[] emojis)
    {
        EmojiPackName = emojiPackName;
        Emojis = emojis;
    }
    
    public uint[] GetUniqueSuperQSids((int AniStickerType, int AniStickerPackId)[] excludeAniStickerTypesAndPackIds)
        => Emojis
            .Where(e => e.AniStickerType is not null
                        && e.AniStickerPackId is not null 
                        && !excludeAniStickerTypesAndPackIds.Contains((e.AniStickerType.Value, e.AniStickerPackId.Value)))
            .Select(e => uint.Parse(e.QSid))
            .Distinct()
            .ToArray();
}