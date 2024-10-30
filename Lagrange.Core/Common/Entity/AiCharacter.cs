namespace Lagrange.Core.Common.Entity

;
[Serializable]
public class AiCharacter
{
    public string CharacterId { get; set; }

     public string CharacterName { get; set; }

     public string CharacterVoiceUrl { get; set; }

     public AiCharacter(string characterId, string characterName, string characterVoiceUrl)
     {
         CharacterId = characterId;
         CharacterName = characterName;
         CharacterVoiceUrl = characterVoiceUrl;
     }
}