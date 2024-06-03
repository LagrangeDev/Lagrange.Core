using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Common;
#pragma warning disable CS8618

[Serializable]
public class LocationSearch
{
    [JsonPropertyName("address")] public string Address { get; set; }

    [JsonPropertyName("enum_relation_type")] public long EnumRelationType { get; set; }

    [JsonPropertyName("from")] public string From { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("lat")] public string Lat { get; set; }

    [JsonPropertyName("lng")] public string Lng { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}