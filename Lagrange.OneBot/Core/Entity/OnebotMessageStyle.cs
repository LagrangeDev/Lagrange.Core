using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;

public class OnebotMessageStyle(MessageStyle style)
{
    [JsonPropertyName("bubble_id")] public ulong BubbleId { get; set; } = style.BubbleId;

    [JsonPropertyName("pendant_id")] public ulong PendantId { get; set; } = style.PendantId;

    [JsonPropertyName("font_id")] public ushort FontId { get; set; } = style.FontId;

    [JsonPropertyName("font_effect_id")] public uint FontEffectId { get; set; } = style.FontEffectId;

    [JsonPropertyName("is_cs_font_effect_enabled")] public bool IsCsFontEffectEnabled { get; set; } = style.IsCsFontEffectEnabled;

    [JsonPropertyName("bubble_diy_text_id")] public uint BubbleDiyTextId { get; set; } = style.BubbleDiyTextId;
}