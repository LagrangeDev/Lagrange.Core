using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;
[Serializable]
public class OneBotAiCharacter(AiCharacter aiCharacter)
{
    [JsonPropertyName("character_id")] public string CharacterId { get; set; } = aiCharacter.CharacterId;

    [JsonPropertyName("character_name")] public string CharacterName { get; set; } = aiCharacter.CharacterName;

    [JsonPropertyName("preview_url")] public string PreviewUrl { get; set; } = aiCharacter.CharacterVoiceUrl;
}