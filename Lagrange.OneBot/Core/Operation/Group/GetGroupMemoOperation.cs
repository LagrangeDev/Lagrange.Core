using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("_get_group_notice")]
public class GetGroupMemoOperation(TicketService ticket) : IOperation
{

    private readonly HttpClient _client = new(new HttpClientHandler { UseCookies = false });

    private const string Url = "https://web.qun.qq.com/cgi-bin/announce/get_t_list";

    private async Task<IEnumerable<OneBotGroupNotice>?> GetGroupNotice(OneBotGetGroupMemo memo)
    {
        var url = $"{Url}?bkn={await ticket.GetCsrfToken()}&qid={memo.GroupId}&ft=23&ni=1&n=1&i=1&log_read=1&platform=1&s=-1&n=20";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        try
        {
            var response = await ticket.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var notices = JsonSerializer.Deserialize<GroupNoticeResponse>(content)!;
            return notices.Feeds.Concat(notices.Inst)
                .Select(feed => new OneBotGroupNotice(feed.NoticeId, feed.SenderId, feed.PublishTime,
                    new OneBotGroupNoticeMessage(feed.Message.Text, feed.Message.Images.Select(image => new OneBotGroupNoticeImage(image.Id, image.Height, image.Width)))
                ));
        }
        catch
        {
            return null;
        }
    }

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetGroupMemo>(SerializerOptions.DefaultOptions) is { } memo)
        {
            var notices = await GetGroupNotice(memo);
            return notices is null
                ? new OneBotResult(null, -1, "failed")
                : new OneBotResult(notices, 0, "ok");
        }

        throw new Exception();
    }
}

[Serializable]
file class GroupNoticeImage
{
    [JsonPropertyName("h")] public string Height { get; set; } = string.Empty;

    [JsonPropertyName("w")] public string Width { get; set; } = string.Empty;

    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
}

[Serializable]
file class GroupNoticeMessage
{
    [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;

    [JsonPropertyName("pics")] public IEnumerable<GroupNoticeImage> Images { get; set; } = [];
}

[Serializable]
file class GroupNoticeFeed
{
    [JsonPropertyName("fid")] public string NoticeId { get; set; } = string.Empty;

    [JsonPropertyName("u")] public uint SenderId { get; set; }

    [JsonPropertyName("pubt")] public long PublishTime { get; set; }

    [JsonPropertyName("msg")] public GroupNoticeMessage Message { get; set; } = new();
}

[Serializable]
file class GroupNoticeResponse
{
    [JsonPropertyName("feeds")] public IEnumerable<GroupNoticeFeed> Feeds { get; set; } = [];

    [JsonPropertyName("inst")] public IEnumerable<GroupNoticeFeed> Inst { get; set; } = [];
}