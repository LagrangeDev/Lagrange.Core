using Lagrange.Core.Common.Entity;
using Realms;

namespace Lagrange.OneBot.Database;

public partial class MessageStyleRecord : IRealmObject
{
    [MapTo(nameof(BubbleId))]
    public long BubbleIdLong { get; set; }
    public ulong BubbleId { get => (ulong)BubbleIdLong; set => BubbleIdLong = (long)value; }

    [MapTo(nameof(PendantId))]
    public long PendantIdLong { get; set; }
    public ulong PendantId { get => (ulong)PendantIdLong; set => PendantIdLong = (long)value; }

    [MapTo(nameof(FontIdShort))]
    public short FontIdShort { get; set; }
    public ushort FontId { get => (ushort)FontIdShort; set => FontIdShort = (short)value; }

    [MapTo(nameof(FontEffectId))]
    public int FontEffectIdInt { get; set; }
    public uint FontEffectId { get => (uint)FontEffectIdInt; set => FontEffectIdInt = (int)value; }

    public bool IsCsFontEffectEnabled { get; set; }

    [MapTo(nameof(BubbleDiyTextId))]
    public int BubbleDiyTextIdInt { get; set; }
    public uint BubbleDiyTextId { get => (uint)BubbleDiyTextIdInt; set => BubbleDiyTextIdInt = (int)value; }


    public static implicit operator MessageStyleRecord(MessageStyle style) => new()
    {
        BubbleId = style.BubbleId,
        PendantId = style.PendantId,
        FontId = style.FontId,
        FontEffectId = style.FontEffectId,
        IsCsFontEffectEnabled = style.IsCsFontEffectEnabled,
        BubbleDiyTextId = style.BubbleDiyTextId
    };

    public static implicit operator MessageStyle(MessageStyleRecord record) => new()
    {
        BubbleId = record.BubbleId,
        PendantId = record.PendantId,
        FontId = record.FontId,
        FontEffectId = record.FontEffectId,
        IsCsFontEffectEnabled = record.IsCsFontEffectEnabled,
        BubbleDiyTextId = record.BubbleDiyTextId
    };
}