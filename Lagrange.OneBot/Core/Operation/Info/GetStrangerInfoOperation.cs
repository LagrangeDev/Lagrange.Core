using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_stranger_info")]
public class GetStrangerInfoOperation : IOperation
{
    private static readonly Dictionary<uint, string> _statusMessage = new()
    {
        { 1, "在线" },
        { 3, "离开" },
        { 4, "隐身/离线" },
        { 5, "忙碌" },
        { 6, "Q我吧" },
        { 7, "请勿打扰" },
        { 263169, "听歌中" },
        { 15205121, "我的电量" },
        { 16713473, "做好事" },
        { 13829889, "出去浪" },
        { 14616321, "去旅行" },
        { 14550785, "被掏空" },
        { 14747393, "今日步数" },
        { 394241, "今日天气" },
        { 14878465, "我crush了" },
        { 14026497, "爱你" },
        { 1770497, "恋爱中" },
        { 3081217, "好运锦鲤" },
        { 11600897, "水逆退散" },
        { 2098177, "嗨到飞起" },
        { 2229249, "元气满满" },
        { 2556929, "一言难尽" },
        { 13698817, "难得糊涂" },
        { 7931137, "emo中" },
        { 2491393, "我太难了" },
        { 14485249, "我想开了" },
        { 1836033, "我没事" },
        { 2425857, "想静静" },
        { 2294785, "悠哉哉" },
        { 15926017, "信号弱" },
        { 16253697, "睡觉中" },
        { 14419713, "肝作业" },
        { 16384769, "学习中" },
        { 15140609, "搬砖中" },
        { 1312001, "摸鱼中" },
        { 2360321, "无聊中" },
        { 197633, "timi中" },
        { 15271681, "一起元梦" },
        { 15337217, "求星搭子" },
        { 525313, "熬夜中" },
        { 16581377, "追剧中" },
        { 13633281, "自定义状态"}
    };

    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetStrangerInfo>(SerializerOptions.DefaultOptions) is { } stranger)
        {
            if (await context.FetchUserInfo(stranger.UserId, stranger.NoCache) is { } info)
            {
                return new OneBotResult(new OneBotStranger
                {
                    UserId = info.Uin,
                    Avatar = info.Avatar,
                    QId = info.Qid,
                    NickName = info.Nickname,
                    Sex = info.Gender.ToOneBotString(),
                    Age = info.Age,
                    Level = info.Level,
                    Sign = info.Sign ?? string.Empty,
                    Status = new()
                    {
                        StatusId = info.Status.StatusId,
                        FaceId = info.Status.FaceId,
                        Message = info.Status.StatusId != 13633281
                            ? _statusMessage.GetValueOrDefault(info.Status.StatusId)
                            : info.Status.Msg
                    },
                    RegisterTime = info.RegisterTime
                }, 0, "ok");
            }

            return new OneBotResult(null, -1, "failed");
        }
        throw new Exception();
    }
}