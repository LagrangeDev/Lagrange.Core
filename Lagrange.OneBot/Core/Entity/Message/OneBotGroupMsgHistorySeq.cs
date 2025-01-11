using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupMsgHistorySeq
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("start")] public int Start { get; set; }

    [JsonPropertyName("end")] public int End { get; set; }

}