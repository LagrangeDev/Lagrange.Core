using System.Net;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.OneBot.Utility;

// Resharper disable InconsistentNaming

public class TicketHelper
{
    private static readonly HttpClient _client;

    private static readonly CookieContainer _container = new();
    
    static TicketHelper()
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = _container;
        
        _client = new HttpClient(handler);
        _client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.37.0");
    }
    
    public static async Task<string?> GetSKey(BotContext context)
    {
        const string jump = "https%3A%2F%2Fh5.qzone.qq.com%2Fqqnt%2Fqzoneinpcqq%2Ffriend%3Frefresh%3D0%26clientuin%3D0%26darkMode%3D0&keyindex=19&random=2599";
        string url = $"https://ssl.ptlogin2.qq.com/jump?ptlang=1033&clientuin={context.BotUin}&clientkey={await context.GetClientKey()}&u1={jump}";
        await _client.GetAsync(url);

        var cookies = _container.GetAllCookies();
        return cookies["skey"]?.Value;
    }

    public static int GetCSRFToken(string sKey)
    {
        var hash = 5381;
        for (int i = 0, len = sKey.Length; i < len; ++i)
        {
            hash += (hash << 5) + sKey[i];
        }
        return hash & 2147483647;
    }

    public async Task<string> GetCookies(BotContext context, string domain)
    {
        string? skey = await GetSKey(context);
        string token = (await context.FetchCookies([domain]))[0];
        return $"p_uin=o{context.BotUin}; p_skey=o{token}; skey={skey}; uin=o{context.BotUin}";
    }
}