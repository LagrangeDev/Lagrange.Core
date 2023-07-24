using Lagrange.Core.Common.Entity;
using Lagrange.Core.Core.Context.Attributes;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Message;
using System.Text;

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[BusinessLogic("OperationLogic", "Manage the user operation of the bot")]
internal class OperationLogic : LogicBase
{
    private const string Tag = nameof(OperationLogic);
    
    internal OperationLogic(ContextCollection collection) : base(collection) { }

    public override async Task Incoming(ProtocolEvent e)
    {
        
    }

    public async Task<List<string>> GetCookies(List<string> domains)
    {
        var fetchCookieEvent = FetchCookieEvent.Create(domains);
        var events = await Collection.Business.SendEvent(fetchCookieEvent);
        return events.Count != 0 ? ((FetchCookieEvent)events[0]).Cookies : new List<string>();
    }

    public async Task<List<BotFriend>> FetchFriends()
    {
        var fetchFriendsEvent = FetchFriendsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchFriendsEvent);
        return events.Count != 0 ? ((FetchFriendsEvent)events[0]).Friends : new List<BotFriend>();
    }

    public async Task<bool> SendMessage(MessageChain chain)
    {
        var sendMessageEvent = SendMessageEvent.Create(chain);
        var events = await Collection.Business.SendEvent(sendMessageEvent);
        return events.Count != 0 && ((SendMessageEvent)events[0]).ResultCode == 0;
    }
    
    public async Task<bool> GetHighwayAddress()
    {
        var highwayUrlEvent = HighwayUrlEvent.Create();
        var events = await Collection.Business.SendEvent(highwayUrlEvent);
        return events.Count != 0 && ((HighwayUrlEvent)events[0]).ResultCode == 0;
    }

    private static int CalculateBkn(string sKey) => 
        (int)sKey.Aggregate<char, long>(5381, (current, t) => current + (current << 5) + t) & int.MaxValue;
    
    public int GetCsrfToken() => CalculateBkn(Encoding.ASCII.GetString(Collection.Keystore.Session.D2Key));
}