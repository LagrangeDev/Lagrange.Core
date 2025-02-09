using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;

public class OnebotMessageStyle
{
    [JsonPropertyName("bubble_id")] public ulong BubbleId { get; set; }

    [JsonPropertyName("pendant_id")] public ulong PendantId { get; set; }

    [JsonPropertyName("font_id")] public ushort FontId { get; set; }

    [JsonPropertyName("font_effect_id")] public uint FontEffectId { get; set; }

    [JsonPropertyName("is_cs_font_effect_enabled")] public bool IsCsFontEffectEnabled { get; set; }

    [JsonPropertyName("bubble_diy_text_id")] public uint BubbleDiyTextId { get; set; }

    public OnebotMessageStyle() {}

    public OnebotMessageStyle(MessageStyle style)
    {
        BubbleId = style.BubbleId;
        PendantId = style.PendantId;
        FontId = style.FontId;
        FontEffectId = style.FontEffectId;
        IsCsFontEffectEnabled = style.IsCsFontEffectEnabled;
        BubbleDiyTextId = style.BubbleDiyTextId;
    }
}
