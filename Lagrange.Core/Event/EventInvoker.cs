using System.Runtime.CompilerServices;
using Lagrange.Core.Event.EventArg;

namespace Lagrange.Core.Event;

public partial class EventInvoker : IDisposable
{
    private const string Tag = "EventInvoker";

    private readonly Dictionary<Type, Action<EventBase>> _events;

    public delegate void LagrangeEvent<in TEvent>(BotContext context, TEvent e) where TEvent : EventBase;

    internal EventInvoker(BotContext context)
    {
        _events = new Dictionary<Type, Action<EventBase>>();

        RegisterEvent((BotOnlineEvent e) => OnBotOnlineEvent?.Invoke(context, e));
        RegisterEvent((BotOfflineEvent e) => OnBotOfflineEvent?.Invoke(context, e));
        RegisterEvent((BotLogEvent e) => OnBotLogEvent?.Invoke(context, e));
        RegisterEvent((BotCaptchaEvent e) => OnBotCaptchaEvent?.Invoke(context, e));
        RegisterEvent((BotNewDeviceVerifyEvent e) => OnBotNewDeviceVerify?.Invoke(context, e));
        RegisterEvent((GroupInvitationEvent e) => OnGroupInvitationReceived?.Invoke(context, e));
        RegisterEvent((FriendMessageEvent e) => OnFriendMessageReceived?.Invoke(context, e));
        RegisterEvent((GroupMessageEvent e) => OnGroupMessageReceived?.Invoke(context, e));
        RegisterEvent((TempMessageEvent e) => OnTempMessageReceived?.Invoke(context, e));
        RegisterEvent((GroupAdminChangedEvent e) => OnGroupAdminChangedEvent?.Invoke(context, e));
        RegisterEvent((GroupMemberIncreaseEvent e) => OnGroupMemberIncreaseEvent?.Invoke(context, e));
        RegisterEvent((GroupMemberDecreaseEvent e) => OnGroupMemberDecreaseEvent?.Invoke(context, e));
        RegisterEvent((FriendRequestEvent e) => OnFriendRequestEvent?.Invoke(context, e));
        RegisterEvent((GroupInvitationRequestEvent e) => OnGroupInvitationRequestEvent?.Invoke(context, e));
        RegisterEvent((GroupJoinRequestEvent e) => OnGroupJoinRequestEvent?.Invoke(context, e));
        RegisterEvent((GroupMuteEvent e) => OnGroupMuteEvent?.Invoke(context, e));
        RegisterEvent((GroupMemberMuteEvent e) => OnGroupMemberMuteEvent?.Invoke(context, e));
        RegisterEvent((GroupRecallEvent e) => OnGroupRecallEvent?.Invoke(context, e));
        RegisterEvent((FriendRecallEvent e) => OnFriendRecallEvent?.Invoke(context, e));
        RegisterEvent((DeviceLoginEvent e) => OnDeviceLoginEvent?.Invoke(context, e));
        RegisterEvent((FriendPokeEvent e) => OnFriendPokeEvent?.Invoke(context, e));
        RegisterEvent((GroupPokeEvent e) => OnGroupPokeEvent?.Invoke(context, e));
        RegisterEvent((GroupEssenceEvent e) => OnGroupEssenceEvent?.Invoke(context, e));
        RegisterEvent((GroupReactionEvent e) => OnGroupReactionEvent?.Invoke(context, e));
        RegisterEvent((GroupNameChangeEvent e) => OnGroupNameChangeEvent?.Invoke(context, e));
        RegisterEvent((GroupTodoEvent e) => OnGroupTodoEvent?.Invoke(context, e));
        RegisterEvent((GroupMemberEnterEvent e) => OnGroupMemberEnterEvent?.Invoke(context, e));
        RegisterEvent((PinChangedEvent e) => OnPinChangedEvent?.Invoke(context, e));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RegisterEvent<TEvent>(Action<TEvent> action) where TEvent : EventBase => _events[typeof(TEvent)] = e => action((TEvent)e);

    internal void PostEvent(EventBase e)
    {
        Task.Run(() =>
        {
            try
            {
                if (_events.TryGetValue(e.GetType(), out var action)) action(e);
                else PostEvent(new BotLogEvent(Tag, LogLevel.Warning, $"Event {e.GetType().Name} is not registered but pushed to invoker"));
            }
            catch (Exception ex)
            {
                PostEvent(new BotLogEvent(Tag, LogLevel.Exception, $"{ex.StackTrace}\n{ex.Message}"));
            }
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _events.Clear();
    }
}