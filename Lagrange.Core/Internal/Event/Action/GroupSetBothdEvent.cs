namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetBothdEvent : ProtocolEvent
{
    public uint BotId { get; set; }
    
    public uint GroupUin { get; set; }

    private GroupSetBothdEvent(uint botId, uint groupuin) : base(0)
    {
        BotId = botId;
        GroupUin = groupuin;
    }

    private GroupSetBothdEvent(int resultCode) : base(resultCode) { }
    
    public static GroupSetBothdEvent Create(uint botId, uint groupuin) => new(botId, groupuin);

    public static GroupSetBothdEvent Result(int resultCode) => new(resultCode);
}