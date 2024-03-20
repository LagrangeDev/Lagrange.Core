using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation;
using Lagrange.OneBot.Core.Operation.Converters;

[Operation("_del_group_notice")]
public class DeleteGroupMemoOperation(TicketService ticket) : IOperation
{
    private readonly HttpClient _client = new(new HttpClientHandler { UseCookies = false });

    private const string _url = "https://web.qun.qq.com/cgi-bin/announce/del_feed";

    private async Task<bool> DeleteGroupNotice(OneBotDeleteGroupMemo memo)
    {
        var url = $"{_url}?fid={memo.NoticeId}&qid={memo.GroupId}&bkn={ticket.GetCsrfToken()}&ft=23&op=1";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Cookie", await ticket.GetCookies("qun.qq.com"));
        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotDeleteGroupMemo>(SerializerOptions.DefaultOptions) is { } memo)
        {
            return (await DeleteGroupNotice(memo))
                ? new OneBotResult(null, -1, "failed")
                : new OneBotResult(null, 0, "ok");
        }

        throw new Exception();
    }
}
