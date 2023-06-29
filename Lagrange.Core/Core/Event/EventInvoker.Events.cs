using Lagrange.Core.Core.Event.EventArg;

namespace Lagrange.Core.Core.Event;

public partial class EventInvoker
{
    public event LagrangeEvent<BotOnlineEvent>? OnBotOnlineEvent;
    
    public event LagrangeEvent<BotOfflineEvent>? OnBotOfflineEvent;
    
    public event LagrangeEvent<BotLogEvent>? OnBotLogEvent;

    public event LagrangeEvent<FriendMessageEvent>? OnFriendMessageReceived;
    
    public event LagrangeEvent<GroupMessageEvent>? OnGroupMessageReceived;
    
    public event LagrangeEvent<TempMessageEvent>? OnTempMessageReceived;
}