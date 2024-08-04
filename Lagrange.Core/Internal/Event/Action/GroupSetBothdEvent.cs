namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetBothdEvent : ProtocolEvent
{
    public uint BotId { get; set; }
    
    public uint GroupUin { get; set; }

    public string? Data_1 { get; set; }


    public string? Data_2 { get; set; }

    private GroupSetBothdEvent(uint botId, uint groupuin, string? data_1, string? data_2) : base(0)
    {
        BotId = botId;
        GroupUin = groupuin;
        Data_1 = data_1;
        Data_2 = data_2;
    }

    private GroupSetBothdEvent(int resultCode) : base(resultCode) { }
    
    public static GroupSetBothdEvent Create(uint botId, uint groupuin, string? data_1, string? data_2) => new(botId, groupuin,data_1,data_2);

    public static GroupSetBothdEvent Result(int resultCode) => new(resultCode);
}