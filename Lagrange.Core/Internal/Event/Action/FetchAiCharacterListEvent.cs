using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class FetchAiCharacterListEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public uint ChatType { get; set; }

    public List<AiCharacterList>? AiCharacters { get; set; }
    
    public string ErrorMessage { get; set; }=string.Empty;

    private FetchAiCharacterListEvent(uint chatType, uint groupUin = 42) : base(true)
    {
        ChatType = chatType;
        GroupUin = groupUin;
        AiCharacters = new List<AiCharacterList>();
    }

    private FetchAiCharacterListEvent(int resultCode, List<AiCharacterList>? aiCharacters,string errMsg) : base(resultCode)
    {
        AiCharacters = aiCharacters;
        ErrorMessage = errMsg;
    }

    public static FetchAiCharacterListEvent Create(uint chatType, uint groupUin) => new(chatType, groupUin);

    public static FetchAiCharacterListEvent Result(int resultCode, List<AiCharacterList>? aiCharacters,string errMsg) =>
        new(resultCode, aiCharacters, errMsg);
}