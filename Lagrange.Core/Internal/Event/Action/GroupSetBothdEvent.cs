namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetBothdEvent : ProtocolEvent
{
    public uint BotId { get; set; }
    
    public uint GroupUin { get; set; }

    public string? Data { get; set; }

    private GroupSetBothdEvent(uint botId, uint groupuin, string? data) : base(0)
    {
        BotId = botId;
        GroupUin = groupuin;
        Data = data;
    }

    private GroupSetBothdEvent(int resultCode) : base(resultCode) { }
    
    public static GroupSetBothdEvent Create(uint botId, uint groupuin, string? data) => new(botId, groupuin,data);

    public static GroupSetBothdEvent Result(int resultCode) => new(resultCode);
}