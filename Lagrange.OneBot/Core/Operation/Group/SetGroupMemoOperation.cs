using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("_send_group_notice")]
public class SetGroupMemoOperation(TicketService ticket) : IOperation
{
    private const string Url = "https://web.qun.qq.com/cgi-bin/announce/add_qun_notice?bkn=";
    private const string UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.1.2; PCRT00 Build/N2G48H)";

    private async Task<string?> SendGroupNoticeSimple(OneBotSetGroupMemo memo)
    {
        int bkn = await ticket.GetCsrfToken();
        string body = $"qid={memo.GroupId}&bkn={bkn}&text={Uri.EscapeDataString(memo.Content)}&pinned=0&type=1&settings={{\"is_show_edit_card\":0,\"tip_window_type\":1,\"confirm_required\":1}}";
        string url = Url + bkn;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.UserAgent.ParseAdd(UserAgent);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await ticket.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            var sendResponse = JsonSerializer.Deserialize<NoticeSendResponse>(content);
            return sendResponse?.NoticeId;
        }
        catch
        {
            return null;
        }
    }

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetGroupMemo>(SerializerOptions.DefaultOptions) is { } memo)
        {
            var noticeId = string.IsNullOrEmpty(memo.Image)
                ? await SendGroupNoticeSimple(memo)
                : throw new NotImplementedException();

            return noticeId is null
                ? new OneBotResult(null, -1, "failed")
                : new OneBotResult(noticeId, 0, "ok");
        }

        throw new Exception();
    }
}

[Serializable]
file class NoticeSendResponse
{
    [JsonPropertyName("new_fid")] public string NoticeId { get; set; } = string.Empty;
}

[Serializable]
file class NoticeImageUploadResponse
{
    [JsonPropertyName("ec")] public int ErrorCode { get; set; }
    [JsonPropertyName("em")] public string ErrorMessage { get; set; } = string.Empty;
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
}

[Serializable]
file class GroupNoticeImage
{
    [JsonPropertyName("h")] public string Height { get; set; } = string.Empty;
    [JsonPropertyName("w")] public string Width { get; set; } = string.Empty;
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
}