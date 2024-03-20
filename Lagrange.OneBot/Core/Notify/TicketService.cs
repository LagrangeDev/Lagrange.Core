using System.Net;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;

namespace Lagrange.OneBot.Core.Notify;

public class TicketService
{
    private readonly BotContext _context;

    private readonly HttpClient _client;
    
    private readonly CookieContainer _container;

    public TicketService(BotContext context)
    {
        _container = new CookieContainer();
        _context = context;

        var handler = new HttpClientHandler
        {
            CookieContainer = _container
        };
        _client = new HttpClient(handler);
    }

    private async Task<string?> GetSKey()
    {
        const string jump = "https%3A%2F%2Fh5.qzone.qq.com%2Fqqnt%2Fqzoneinpcqq%2Ffriend%3Frefresh%3D0%26clientuin%3D0%26darkMode%3D0&keyindex=19&random=2599";
        string url = $"https://ssl.ptlogin2.qq.com/jump?ptlang=1033&clientuin={_context.BotUin}&clientkey={await _context.GetClientKey()}&u1={jump}";
        await _client.GetAsync(url);

        var cookies = _container.GetAllCookies();
        return cookies["skey"]?.Value;
    }

    public async Task<int> GetCsrfToken()
    {
        string? sKey = await GetSKey();
        if (sKey == null) throw new InvalidDataException();
        
        var hash = 5381;
        for (int i = 0, len = sKey.Length; i < len; ++i)
        {
            hash += (hash << 5) + sKey[i];
        }
        return hash & 2147483647;
    }

    private async Task<string> GetCookies(string domain)
    {
        string? skey = await GetSKey();
        string token = (await _context.FetchCookies([domain]))[0];
        return $"p_uin=o{_context.BotUin}; p_skey={token}; skey={skey}; uin=o{_context.BotUin}";
    }

    public async Task<string> GetAsync(string url)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, url);
        message.Headers.Add("Cookie", await GetCookies(message.RequestUri?.Host ?? ""));

        var response = await _client.SendAsync(message);
        return await response.Content.ReadAsStringAsync();
    }
}