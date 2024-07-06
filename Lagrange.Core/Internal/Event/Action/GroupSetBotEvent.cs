namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetBotEvent : ProtocolEvent
{
    public uint BotId { get; set; }
    
    public uint On { get; set; }
    
    public uint GroupUin { get; set; }

    private GroupSetBotEvent(uint botId, uint on, uint groupuin) : base(0)
    {
        BotId = botId;
        On = on;
        GroupUin = groupuin;
    }

    private GroupSetBotEvent(int resultCode) : base(resultCode) { }
    
    public static GroupSetBotEvent Create(uint botId, uint on, uint groupuin) => new(botId, on, groupuin);

    public static GroupSetBotEvent Result(int resultCode) => new(resultCode);
}