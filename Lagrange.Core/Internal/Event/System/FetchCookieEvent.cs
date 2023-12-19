#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.System;

internal class FetchCookieEvent : ProtocolEvent
{
    public List<string> Domains { get; set; }
    
    public List<string> Cookies { get; set; }

    private FetchCookieEvent(List<string> domains) : base(true)
    {
        Domains = domains;
    }

    private FetchCookieEvent(int resultCode, List<string> cookies) : base(resultCode)
    {
        Cookies = cookies;
    }
    
    public static FetchCookieEvent Create(List<string> domains) => new(domains);
    
    public static FetchCookieEvent Result(int resultCode, List<string> cookies) => new(resultCode, cookies);
}