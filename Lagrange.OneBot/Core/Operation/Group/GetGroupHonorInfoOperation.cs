using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("get_group_honor_info")]
public partial class GetGroupHonorInfoOperation(TicketService ticket) : IOperation
{
    [GeneratedRegex(@"window\.__INITIAL_STATE__\s*?=\s*?(\{.*?);")]
    public static partial Regex HonorRegex();

    private static readonly Dictionary<string, string> Keys = new()
    {
        { "group_id", "group_id" }, { "current_talkative", "currentTalkative" }, { "talkative_list", "talkativeList" },
        { "performer_list", "" }, { "legend_list", "legendList" }, { "strong_newbie_list", "strongnewbieList" },
        { "emotion_list", "emotionList" }
    };

    private static readonly Dictionary<string, int> HonorToType = new()
    {
        { "talkative", 1 }, { "performer", 2 }, { "legend", 3 }, { "strong_newbie", 5 }, { "emotion", 6 },
    };

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotHonorInfo>(SerializerOptions.DefaultOptions) is { } honor)
        {
            var result = new JsonObject();
            var honors = honor.Type == "all"
                ? HonorToType.Select(x => x.Key).ToArray()
                : [honor.Type];

            foreach (string honorRaw in honors)
            {
                string url = $"https://qun.qq.com/interactive/honorlist?gc={honor.GroupId}&type={HonorToType[honorRaw]}";
                var response = await ticket.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                string raw = await response.Content.ReadAsStringAsync();
                var match = HonorRegex().Match(raw);

                if (JsonSerializer.Deserialize<JsonObject>(match.Groups[1].Value) is { } json)
                {
                    foreach (var (key, value) in Keys)
                    {
                        if (json[value] is { } node) result.TryAdd(key, node.Deserialize<JsonNode>()); // 神经病
                    }
                }
            }

            return new OneBotResult(result, 0, "ok");
        }

        throw new Exception();
    }
}
