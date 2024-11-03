﻿using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetAiCharacters
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = 42;
    
    [JsonPropertyName("chat_type")] public uint ChatType { get; set; }
}