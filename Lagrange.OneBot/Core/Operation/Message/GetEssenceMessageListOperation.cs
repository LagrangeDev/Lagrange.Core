using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_essence_msg_list")]
public class GetEssenceMessageListOperation(TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["group_id"]?.GetValue<uint>() is { } groupUin)
        {
            int bkn = await ticket.GetCsrfToken();
            int page = 0;
            var essence = new List<OneBotEssenceMessage>();

            while (true)
            {
                string url = $"https://qun.qq.com/cgi-bin/group_digest/digest_list?random=7800&X-CROSS-ORIGIN=fetch&group_code={groupUin}&page_start={page}&page_limit=20&bkn={bkn}";
                var response = await ticket.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                string raw = await response.Content.ReadAsStringAsync();
                var @object = JsonSerializer.Deserialize<RequestBody>(raw);
                if (@object?.Data.MsgList == null) break;

                foreach (var msg in @object.Data.MsgList)
                {
                    essence.Add(new OneBotEssenceMessage
                    {
                        SenderId = uint.Parse(msg.SenderUin),
                        SenderNick = msg.SenderNick,
                        SenderTime = msg.SenderTime,
                        OperatorId = uint.Parse(msg.AddDigestUin),
                        OperatorNick = msg.AddDigestNick,
                        OperatorTime = msg.AddDigestTime,
                        MessageId = MessageRecord.CalcMessageHash(msg.MsgRandom, msg.MsgSeq),
                        Content = ConvertToSegment(msg.MsgContent)
                    });
                }
                
                if (@object.Data.IsEnd) break;
                
                page++;
            }

            return new OneBotResult(essence, 0, "ok");
        }

        throw new Exception();
    }

    private static List<OneBotSegment> ConvertToSegment(IEnumerable<JsonObject> elements)
    {
        var segments = new List<OneBotSegment>();
        foreach (var msg in elements)
        {
            uint type = msg["msg_type"]?.GetValue<uint>() ?? throw new InvalidDataException("Invalid type");
            var segment = type switch
            {
                1 => new OneBotSegment("text", new TextSegment(msg["text"]?.GetValue<string>() ?? "")),
                2 => new OneBotSegment("face", new FaceSegment(msg["face_index"]?.GetValue<int?>() ?? 0)),
                3 => new OneBotSegment("image", new ImageSegment(msg["image_url"]?.GetValue<string>() ?? "")),
                4 => new OneBotSegment("video", new VideoSegment(msg["file_thumbnail_url"]?.GetValue<string>() ?? "")),
                _ => throw new InvalidDataException("Unknown type found in essence msg")
            };
            segments.Add(segment);
        }

        return segments;
    }
}

[Serializable]
file class RequestBody
{
    [JsonPropertyName("retcode")] public long Retcode { get; set; }

    [JsonPropertyName("retmsg")] public string Retmsg { get; set; }

    [JsonPropertyName("data")] public Data Data { get; set; }
}

[Serializable]
file class Data
{
    [JsonPropertyName("msg_list")] public MsgList[]? MsgList { get; set; }

    [JsonPropertyName("is_end")] public bool IsEnd { get; set; }

    [JsonPropertyName("group_role")] public long GroupRole { get; set; }

    [JsonPropertyName("config_page_url")] public Uri ConfigPageUrl { get; set; }
}

[Serializable]
file class MsgList
{
    [JsonPropertyName("group_code")] public string GroupCode { get; set; }

    [JsonPropertyName("msg_seq")] public uint MsgSeq { get; set; }
        
    [JsonPropertyName("msg_random")] public uint MsgRandom { get; set; }

    [JsonPropertyName("sender_uin")] public string SenderUin { get; set; }
        
    [JsonPropertyName("sender_nick")] public string SenderNick { get; set; }

    [JsonPropertyName("sender_time")] public uint SenderTime { get; set; }

    [JsonPropertyName("add_digest_uin")] public string AddDigestUin { get; set; }

    [JsonPropertyName("add_digest_nick")] public string AddDigestNick { get; set; }

    [JsonPropertyName("add_digest_time")] public uint AddDigestTime { get; set; }
        
    [JsonPropertyName("msg_content")] public JsonObject[] MsgContent { get; set; }
        
    [JsonPropertyName("can_be_removed")] public bool CanBeRemoved { get; set; }
}