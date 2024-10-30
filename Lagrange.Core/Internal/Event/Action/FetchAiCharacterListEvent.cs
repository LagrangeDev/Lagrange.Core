using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;

namespace Lagrange.Core.Internal.Event.Action;

internal class FetchAiCharacterListEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public List<AiCharacter> AiCharacters { get; set; }

    private FetchAiCharacterListEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
        AiCharacters = new List<AiCharacter>();
    }

    private FetchAiCharacterListEvent(List<AiCharacter> aiCharacters) : base(0) { AiCharacters = aiCharacters; }

    public static FetchAiCharacterListEvent Create(uint groupUin) => new(groupUin);

    public static FetchAiCharacterListEvent Result(List<AiCharacter> aiCharacters) => new(aiCharacters);
}