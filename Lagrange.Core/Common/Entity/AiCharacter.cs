namespace Lagrange.Core.Common.Entity;

[Serializable]
public class AiCharacter
{
    public string VoiceId { get; set; }

    public string CharacterName { get; set; }

    public string CharacterVoiceUrl { get; set; }

    public AiCharacter(string voiceId, string characterName, string characterVoiceUrl)
    {
        VoiceId = voiceId;
        CharacterName = characterName;
        CharacterVoiceUrl = characterVoiceUrl;
    }
}

[Serializable]
public class AiCharacterList
{
    public string Type { get; set; }

    public List<AiCharacter> Characters { get; set; }

    public AiCharacterList(string type, List<AiCharacter> characters)
    {
        Type = type;
        Characters = characters;
    }
}