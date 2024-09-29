using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Common;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_music_ark")]
public class GetMusicArkOperation(TicketService ticket) : IOperation
{
    private static readonly JsonSerializerOptions _options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetMusicArk>(SerializerOptions.DefaultOptions) is not { } musicArk) throw new Exception();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://docs.qq.com/api/user/qq/login")
        {
            Headers =
            {
                { "Accept", "application/json, text/plain, * /*" },
                { "referer", "https://docs.qq.com" },
            }
        };

        using var response = await ticket.SendAsync(request, "docs.qq.com");
        var json = await response.Content.ReadFromJsonAsync<JsonObject>();

        string? uid = json?["result"]?["uid"]?.ToString();
        string? uidKey = json?["result"]?["uid_key"]?.ToString();

        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(uidKey)) throw new Exception();

        var docsArk = new LightApp
        {
            App = "com.tencent.tdoc.qqpush",
            Config = new Config
            {
                Ctime = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Forward = 1,
                Token = "",
                Type = "normal",
            },
            Extra = new Extra
            {
                AppId = 1,
                AppType = 0,
                Uin = context.BotUin
            },
            Meta = new Meta
            {
                Music = new Music
                {
                    Action = "",
                    AndroidPkgName = "",
                    AppType = 1,
                    AppId = 0,
                    Ctime = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Desc = musicArk.Desc,
                    JumpUrl = musicArk.JumpUrl, // 在 ntqqpc 可正常使用 手机 噗叽 pia 了
                    MusicUrl = musicArk.MusicUrl, // 在 手机 可正常使用 ntqqpc 噗叽 pia 了
                    Preview = musicArk.Preview, // 需白
                    SourceMsgId = "0",
                    SourceIcon = musicArk.SourceIcon, // 需白
                    SourceUrl = "",
                    Tag = musicArk.Tag,
                    Title = musicArk.Title,
                    Uin = context.BotUin
                }
            },
            Prompt = $"[分享]{musicArk.Title}",
            Ver = "0.0.0.1",
            View = "music"
        };

        var arksRequest = new HttpRequestMessage(HttpMethod.Post, "https://docs.qq.com/v2/push/ark_sig")
        {
            Headers =
            {
                { "Cookie", (await ticket.GetCookies("docs.qq.com")) + $"uid={uid}; uid_key={uidKey}" }
            },
            Content = JsonContent.Create(new JsonObject {
                {"ark", JsonSerializer.Serialize(docsArk, _options)},
                {"object_id", "YjONkUwkdtFr"}
            })
        };

        using var arksResponse = await ticket.SendAsync(arksRequest);
        var arksJson = await arksResponse.Content.ReadFromJsonAsync<JsonObject>();
        string? arkWithSig = arksJson?["result"]?["ark_with_sig"]?.ToString();

        return arkWithSig != null ? new OneBotResult(arkWithSig, 0, "ok") : new OneBotResult(null, arksJson!["retcode"]!.GetValue<int>(), arksJson!["msg"]!.ToString());
    }
}