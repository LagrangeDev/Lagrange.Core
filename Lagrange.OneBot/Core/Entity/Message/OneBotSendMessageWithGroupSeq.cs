using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotSendMessageWithGroupSeq
{
    [JsonPropertyName("source_group_id")] public uint SourceGroupId { get; set; }

    [JsonPropertyName("source_seq")] public int SourceMsgSeq { get; set; }
    
    [JsonPropertyName("target_group_id")] public uint TargetGroupId { get; set; }
}