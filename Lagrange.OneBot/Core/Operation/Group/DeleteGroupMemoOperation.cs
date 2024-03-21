using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("_del_group_notice")]
public class DeleteGroupMemoOperation(TicketService ticket) : IOperation
{
    private const string Url = "https://web.qun.qq.com/cgi-bin/announce/del_feed";
    private const string Domain = "qun.qq.com";

    private async Task<bool> DeleteGroupNotice(OneBotDeleteGroupMemo memo)
    {
        string url = $"{Url}?fid={memo.NoticeId}&qid={memo.GroupId}&bkn={await ticket.GetCsrfToken()}&ft=23&op=1";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        try
        {
            var response = await ticket.SendAsync(request, Domain);
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
            return await DeleteGroupNotice(memo)
                ? new OneBotResult(null, 0, "ok")
                : new OneBotResult(null, -1, "failed");
        }

        throw new Exception();
    }
}
