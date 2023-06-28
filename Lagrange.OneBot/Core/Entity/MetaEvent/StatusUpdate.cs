using Lagrange.OneBot.Core.Entity.Generic;

namespace Lagrange.OneBot.Core.Entity.MetaEvent;

internal class StatusUpdate : OneBotRequest
{
    private const string EventType = "meta";
    
    private const string EventDetailType = "status_update";

    private const string EventSubType = "";

    public StatusUpdate() : base(EventType, EventDetailType, EventSubType)
    {
        
    }
}