using Lagrange.Core.Internal.Event.EventArg;

namespace Lagrange.Core.Internal.Event;

public partial class EventInvoker
{
    public event LagrangeEvent<BotOnlineEvent>? OnBotOnlineEvent;
    
    public event LagrangeEvent<BotOfflineEvent>? OnBotOfflineEvent;
    
    public event LagrangeEvent<BotLogEvent>? OnBotLogEvent;
    
    public event LagrangeEvent<BotCaptchaEvent>? OnBotCaptchaEvent; 
    
    public event LagrangeEvent<GroupInvitationEvent>? OnGroupInvitationReceived; 

    public event LagrangeEvent<FriendMessageEvent>? OnFriendMessageReceived;
    
    public event LagrangeEvent<GroupMessageEvent>? OnGroupMessageReceived;
    
    public event LagrangeEvent<TempMessageEvent>? OnTempMessageReceived;
    
    public event LagrangeEvent<GroupAdminChangedEvent>? OnGroupAdminChangedEvent;
    
    public event LagrangeEvent<GroupMemberIncreaseEvent>? OnGroupMemberIncreaseEvent;
    
    public event LagrangeEvent<GroupMemberDecreaseEvent>? OnGroupMemberDecreaseEvent;
}