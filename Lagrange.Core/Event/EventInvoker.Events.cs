using Lagrange.Core.Event.EventArg;

namespace Lagrange.Core.Event;

public partial class EventInvoker
{
    public event LagrangeEvent<BotOnlineEvent>? OnBotOnlineEvent;
    
    public event LagrangeEvent<BotOfflineEvent>? OnBotOfflineEvent;
    
    public event LagrangeEvent<BotLogEvent>? OnBotLogEvent;
    
    public event LagrangeEvent<BotCaptchaEvent>? OnBotCaptchaEvent;

    public event LagrangeEvent<BotNewDeviceVerifyEvent>? OnBotNewDeviceVerify; 
    
    public event LagrangeEvent<GroupInvitationEvent>? OnGroupInvitationReceived; 

    public event LagrangeEvent<FriendMessageEvent>? OnFriendMessageReceived;
    
    public event LagrangeEvent<GroupMessageEvent>? OnGroupMessageReceived;
    
    public event LagrangeEvent<TempMessageEvent>? OnTempMessageReceived;
    
    public event LagrangeEvent<GroupAdminChangedEvent>? OnGroupAdminChangedEvent;
    
    public event LagrangeEvent<GroupMemberIncreaseEvent>? OnGroupMemberIncreaseEvent;
    
    public event LagrangeEvent<GroupMemberDecreaseEvent>? OnGroupMemberDecreaseEvent;
    
    public event LagrangeEvent<FriendRequestEvent>? OnFriendRequestEvent;

    public event LagrangeEvent<GroupInvitationRequestEvent>? OnGroupInvitationRequestEvent;
    
    public event LagrangeEvent<GroupJoinRequestEvent>? OnGroupJoinRequestEvent;
    
    public event LagrangeEvent<GroupMuteEvent>? OnGroupMuteEvent;
    
    public event LagrangeEvent<GroupMemberMuteEvent>? OnGroupMemberMuteEvent;
    
    public event LagrangeEvent<GroupRecallEvent>? OnGroupRecallEvent;
    
    public event LagrangeEvent<FriendRecallEvent>? OnFriendRecallEvent;
    
    public event LagrangeEvent<DeviceLoginEvent>? OnDeviceLoginEvent;

    public event LagrangeEvent<FriendPokeEvent>? OnFriendPokeEvent;
}