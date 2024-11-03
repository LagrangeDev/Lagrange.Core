using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotAiCharacter(AiCharacter aiCharacter)
{
    [JsonPropertyName("character_id")] public string CharacterId { get; set; } = aiCharacter.VoiceId;

    [JsonPropertyName("character_name")] public string CharacterName { get; set; } = aiCharacter.CharacterName;

    [JsonPropertyName("preview_url")] public string PreviewUrl { get; set; } = aiCharacter.CharacterVoiceUrl;
}

public class OneBotAiCharacters(AiCharacterList aiCharacters)
{
    [JsonPropertyName("type")] public string Type { get; set; } = aiCharacters.Type;

    [JsonPropertyName("characters")]
    public List<OneBotAiCharacter> Characters { get; set; } =
        aiCharacters.Characters.Select(x => new OneBotAiCharacter(x)).ToList();
}