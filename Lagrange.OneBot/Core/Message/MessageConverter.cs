using System.Reflection;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.EventArg;

namespace Lagrange.OneBot.Core.Message;

/// <summary>
/// The class that converts the OneBot message to/from MessageEntity of Lagrange.Core
/// </summary>
public sealed class MessageConverter
{
    public Dictionary<Type, Type> EntityToSegment { get; set; }
    
    public MessageConverter(BotContext bot, ILagrangeWebService service)
    {
        var invoker = bot.Invoker;
        
        invoker.OnFriendMessageReceived += OnFriendMessageReceived;
        invoker.OnGroupMessageReceived += OnGroupMessageReceived;
        invoker.OnTempMessageReceived += OnTempMessageReceived;

        EntityToSegment = new Dictionary<Type, Type>();
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(ISegment)))
            {
            }
        }
    }
    
    private void OnFriendMessageReceived(BotContext bot, FriendMessageEvent e)
    {
        
    }
    
    private void OnGroupMessageReceived(BotContext bot, GroupMessageEvent e)
    {
        
    }
    
    private void OnTempMessageReceived(BotContext bot, TempMessageEvent e)
    {
        
    }
}