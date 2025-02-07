namespace Lagrange.Core.Common.Entity;

public class MessageStyle
{
    public ulong BubbleId { get; set; }

    public ulong PendantId { get; set; }

    public ushort FontId { get; set; }

    /// <summary>
    /// Font effect id, 2000 for super large font
    /// </summary>
    public uint FontEffectId { get; set; }

    /// <summary>
    /// Whether the "font grows and shrinks" effect is enabled
    /// </summary>
    public bool IsCsFontEffectEnabled { get; set; }

    /// <summary>
    /// Custom bubble sticker/text archive id
    /// </summary>
    public uint BubbleDiyTextId { get; set; }
}
